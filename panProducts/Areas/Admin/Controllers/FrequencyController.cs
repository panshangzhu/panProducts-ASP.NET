using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using products.DataAccess.Data.Repository.IRepository;
using products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panProducts.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();
            if (id == null)
            {
                return View(frequency);
            }
            frequency = _unitOfWork.Frequency.Get(id.GetValueOrDefault());
            if (frequency == null)
            {
                return NotFound();
            }
            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if(ModelState.IsValid)
            {
                if(frequency.Id == 0)
                {
                    _unitOfWork.Frequency.Add(frequency);
                } else
                {
                    _unitOfWork.Frequency.Update(frequency);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Frequency.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Frequency objFound = _unitOfWork.Frequency.Get(id);
            if (objFound == null)
            {
                return Json(new { success = false, message = "Error while deleting." });
            }
            _unitOfWork.Frequency.Remove(objFound);
            _unitOfWork.Save();
            return Json(new
            {
                success = true,
                message = "Delete Successfully."
            });
        }
    }
}
