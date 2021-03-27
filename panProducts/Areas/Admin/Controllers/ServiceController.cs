using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using products.DataAccess.Data.Repository.IRepository;
using products.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace panProducts.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ServiceController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        // upload files (images)
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public ServiceVM SerVM{get; set;}

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            SerVM = new ServiceVM()
            {
                Service = new products.Models.Service(),
                CategoryList = _unitOfWork.Category.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown(),
            };

            if(id != null)
            {
                SerVM.Service = _unitOfWork.Service.Get(id.GetValueOrDefault());
            }

            return View(SerVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if(ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(SerVM.Service.id == 0)
                {
                    // new
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\services");
                    var extention = Path.GetExtension(files[0].FileName);

                    using (var fileSteams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileSteams);
                    }
                    SerVM.Service.ImageUrl = @"\images\services\" + fileName + extention;
                    _unitOfWork.Service.Add(SerVM.Service);
                } else
                {
                    // Edit
                    var serviceFromDb = _unitOfWork.Service.Get(SerVM.Service.id);
                    if(files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\services");
                        var extention_new = Path.GetExtension(files[0].FileName);

                        var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                        using (var fileSteams = new FileStream(Path.Combine(uploads, fileName + extention_new), FileMode.Create))
                        {
                            files[0].CopyTo(fileSteams);
                        }
                        SerVM.Service.ImageUrl = @"\images\services" + fileName + extention_new;
                    } else
                    {
                        SerVM.Service.ImageUrl = serviceFromDb.ImageUrl;
                    }
                    _unitOfWork.Service.Update(SerVM.Service);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                SerVM.CategoryList = _unitOfWork.Category.GetCategoryListForDropDown();
                SerVM.FrequencyList = _unitOfWork.Frequency.GetFrequencyListForDropDown();
                return View(SerVM);
            }
        }

        #region API
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Service.GetAll(includeProperties:"Category,Frequency") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var serviceFromDb = _unitOfWork.Service.Get(id);
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, serviceFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if(serviceFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Service.Remove(serviceFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "deleted" });
        }

        #endregion
    }
}
