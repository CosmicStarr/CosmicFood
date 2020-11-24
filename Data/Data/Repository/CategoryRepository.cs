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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext DB;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            DB = db;
        }
        public IEnumerable<SelectListItem> GetListForDropDown()
        {
            return DB.GetCategories.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }

        public void Update(Category category)
        {
            var objFromDb = DB.GetCategories.FirstOrDefault(g => g.Id == category.Id);
            objFromDb.Name = category.Name;
            objFromDb.DisplayOrder = category.DisplayOrder;
            DB.SaveChanges();
        }
    }
}
