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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void ChangeOrderStatus(int orderHeaderId, string status)
        {
            var orderFromDb = _db.OrderHeader.FirstOrDefault(order => order.Id == orderHeaderId);
            orderFromDb.Status = status;
            _db.SaveChanges();
        }
    }
}
