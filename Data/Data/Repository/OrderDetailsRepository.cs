using CosmicFood.Data.Data;
using Data.Data.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderDetails OrderDetails)
        {
            var objFromdb = _db.GetOrderDetails.FirstOrDefault(o => o.Id == OrderDetails.Id);
            _db.GetOrderDetails.Update(objFromdb);
            _db.SaveChanges();
        }
    }
}
