using CosmicFood.Data.Data;
using Data.Data.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Data.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext DB;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            DB = db;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }
    }
}
