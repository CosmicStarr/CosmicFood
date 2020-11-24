using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Data.Repository.IRepository
{
    public interface IMenuItemsRepository : IRepository<MenuItems>
    {
        void Update(MenuItems menuItems);
    }
}
