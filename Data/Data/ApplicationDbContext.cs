using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace CosmicFood.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Category> GetCategories { get; set; }
        public DbSet<FoodType> GetFood { get; set; }
        public DbSet<MenuItems> GetMenuItems { get; set; }
        public DbSet<ShoppingCart> GetShoppings { get; set; }
        public DbSet<ApplicationUser> GetApplicationUsers { get; set; }
        public DbSet<OrderHeader> GetOrderHeaders { get; set; }
        public DbSet<OrderDetails> GetOrderDetails { get; set; }
    }
}
