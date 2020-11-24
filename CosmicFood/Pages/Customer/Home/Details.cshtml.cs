using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Utility;

namespace CosmicFood.Pages.Customer.Home
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public ShoppingCart ShoppingCart { get; set; }
        public void OnGet(int Id)
        {
            ShoppingCart = new ShoppingCart()
            {
                MenuItems = _unitOfWork.MenuItemsRepository.GetFirstOrDefault(includeproperties: "Category,FoodType", filter: m => m.Id == Id),
                MenuItemID = Id
            };
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ShoppingCart.ApplicationUserID = claim.Value;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.ApplicationUserID == ShoppingCart.ApplicationUserID &&
                                            c.MenuItemID == ShoppingCart.MenuItemID);

                if (cartFromDb == null)
                {
                    _unitOfWork.ShoppingCartRepository.Add(ShoppingCart);
                }
                else
                {
                    _unitOfWork.ShoppingCartRepository.IncrementCount(cartFromDb, ShoppingCart.Count);
                }
                _unitOfWork.Save();

                var count = _unitOfWork.ShoppingCartRepository.GetAll(c => c.ApplicationUserID == ShoppingCart.ApplicationUserID).ToList().Count;
                HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, count);
                return RedirectToPage("Index");

            }
            else
            {
                ShoppingCart.MenuItems = _unitOfWork.MenuItemsRepository.GetFirstOrDefault(includeproperties: "Category,FoodType", filter: c => c.Id == ShoppingCart.MenuItemID);
                return Page();
            }
        }
    }
}
