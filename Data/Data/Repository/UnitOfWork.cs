using CosmicFood.Data.Data;
using Data.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(db);
            FoodTypeRepository = new FoodTypeRepository(db);
            MenuItemsRepository = new MenuItemsRepository(db);
            ApplicationUserRepository = new ApplicationUserRepository(db);
            ShoppingCartRepository = new ShoppingCartRepository(db);
            OrderDetailsRepository = new OrderDetailsRepository(db);
            OrderHeaderRepository = new OrderHeaderRepository(db);
        }
        public ICategoryRepository Category { get; private set; }

        public IFoodTypeRepository FoodTypeRepository { get; private set; }

        public IMenuItemsRepository MenuItemsRepository { get; private set; }

        public IShoppingCartRepository ShoppingCartRepository { get; private set; }

        public IApplicationUserRepository ApplicationUserRepository { get; private set; }

        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }

        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
