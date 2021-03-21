using Microsoft.AspNetCore.Mvc.Rendering;
using panProducts.DataAccess.Data;
using products.DataAccess.Data.Repository.IRepository;
using products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace products.DataAccess.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

    }
}
