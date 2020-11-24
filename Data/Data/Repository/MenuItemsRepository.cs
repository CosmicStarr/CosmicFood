using CosmicFood.Data.Data;
using Data.Data.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Data.Repository
{
    public class MenuItemsRepository : Repository<MenuItems>, IMenuItemsRepository
    {
        private readonly ApplicationDbContext _db;
        public MenuItemsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MenuItems menuItems)
        {
            var objfromdb = _db.GetMenuItems.FirstOrDefault(m => m.Id == menuItems.Id);
            objfromdb.Name = menuItems.Name;
            objfromdb.Description = menuItems.Description;
            objfromdb.Price = menuItems.Price;
            objfromdb.FoodTypeId = menuItems.FoodTypeId;
            objfromdb.CategoryId = menuItems.CategoryId;
            if (objfromdb.Image != null)
            {
                objfromdb.Image = menuItems.Image;
            }
            _db.SaveChanges();

        }
    }
}
