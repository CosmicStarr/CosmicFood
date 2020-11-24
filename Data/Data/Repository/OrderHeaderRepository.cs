using CosmicFood.Data.Data;
using Data.Data.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader OrderHeader)
        {
            var objformdb = _db.GetOrderHeaders.FirstOrDefault(o => o.Id == OrderHeader.Id);
            _db.GetOrderHeaders.Update(objformdb);
            _db.SaveChanges();

        }
    }
}
