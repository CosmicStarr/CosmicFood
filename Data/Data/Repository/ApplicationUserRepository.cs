using CosmicFood.Data.Data;
using Data.Data.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


    }
}
