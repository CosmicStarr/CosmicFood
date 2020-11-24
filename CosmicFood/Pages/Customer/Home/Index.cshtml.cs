using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Utility;

namespace CosmicFood.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<MenuItems> MenuItemsList { get; set; }
        public IEnumerable<Category> CategoriesList { get; set; }
        public void OnGet()
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                int CartCount = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserID == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, CartCount);
            }


            MenuItemsList = _unitOfWork.MenuItemsRepository.GetAll(null, null, "Category,FoodType");
            CategoriesList = _unitOfWork.Category.GetAll(null, q => q.OrderBy(c => c.DisplayOrder), null);
        }
    }
}
