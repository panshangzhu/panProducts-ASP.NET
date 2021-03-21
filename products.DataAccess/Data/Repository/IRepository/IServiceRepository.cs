using Microsoft.AspNetCore.Mvc.Rendering;
using products.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace products.DataAccess.Data.Repository.IRepository
{
    public interface IServiceRepository: IRepository<Service>
    {

        void Update(Service service);
    }
}
