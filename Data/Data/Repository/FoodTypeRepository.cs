using CosmicFood.Data.Data;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Data.Repository
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public FoodTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetListForDropdown()
        {
            return _db.GetFood.Select(f => new SelectListItem()
            {
                Text = f.Name,
                Value = f.Id.ToString()
            });
        }

        public void Update(FoodType FoodType)
        {
            var objfromdb = _db.GetFood.FirstOrDefault(f => f.Id == FoodType.Id);
            objfromdb.Name = FoodType.Name;
            _db.SaveChanges();
        }
    }
}
