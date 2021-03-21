using Microsoft.AspNetCore.Mvc.Rendering;
using products.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace products.DataAccess.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<SelectListItem> GetCategoryListForDropDown();

        void Update(Category category);
    }
}
