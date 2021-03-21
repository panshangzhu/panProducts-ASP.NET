using Microsoft.AspNetCore.Mvc.Rendering;
using products.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace products.DataAccess.Data.Repository.IRepository
{
    public interface IFrequencyRepository: IRepository<Frequency>
    {
        IEnumerable<SelectListItem> GetFrequencyListForDropDown();

        void Update(Frequency frequency);
    }
}
