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
using Models.ViewModels;
using Utility;

namespace CosmicFood.Pages.Customer.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OrderdetailsCart OrderdetailsCart { get; set; }
        public void OnGet()
        {
            OrderdetailsCart = new OrderdetailsCart()
            {
                OrderHeader = new Models.OrderHeader(),
                CartList = new List<ShoppingCart>()
            };
            OrderdetailsCart.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims != null)
            {
                IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCartRepository.GetAll(c => c.ApplicationUserID == claims.Value);
                if (cart != null)
                {
                    OrderdetailsCart.CartList = cart.ToList();
                }
                foreach (var item in OrderdetailsCart.CartList)
                {
                    item.MenuItems = _unitOfWork.MenuItemsRepository.GetFirstOrDefault(m => m.Id == item.MenuItemID);
                    OrderdetailsCart.OrderHeader.OrderTotal += (item.MenuItems.Price * item.Count);
                }
            }
        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.ID == cartId);
            _unitOfWork.ShoppingCartRepository.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.ID == cartId);
            if (cart.Count == 1)
            {
                _unitOfWork.ShoppingCartRepository.Remove(cart);
                _unitOfWork.Save();

                var cout = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserID == cart.ApplicationUserID).ToList().Count;
                HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, cout);
            }
            else
            {
                _unitOfWork.ShoppingCartRepository.DecrementCount(cart, 1);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Cart/Index");
        }
        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(c => c.ID == cartId);
            _unitOfWork.ShoppingCartRepository.Remove(cart);
            _unitOfWork.Save();

            var cout = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserID == cart.ApplicationUserID).ToList().Count;
            HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, cout);
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}
