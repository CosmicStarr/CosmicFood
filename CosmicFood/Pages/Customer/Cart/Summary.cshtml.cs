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
using Stripe;
using Utility;

namespace CosmicFood.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public OrderdetailsCart OrderdetailsCart { get; set; }
        public IActionResult OnGet()
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

            ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(a => a.Id == claims.Value);
            OrderdetailsCart.OrderHeader.PickUpName = applicationUser.FirstName;
            OrderdetailsCart.OrderHeader.PickUpDate = DateTime.Now;
            OrderdetailsCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;

            return Page();
        }

        public IActionResult OnPost(string StripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            OrderdetailsCart.CartList = _unitOfWork.ShoppingCartRepository.GetAll(c => c.ApplicationUserID == claims.Value).ToList();

            OrderdetailsCart.OrderHeader.PaymentStatus = StaticDetails.PaymentPending;
            OrderdetailsCart.OrderHeader.OrderDate = DateTime.Now;
            OrderdetailsCart.OrderHeader.UserId = claims.Value;
            OrderdetailsCart.OrderHeader.Status = StaticDetails.PaymentPending;
            OrderdetailsCart.OrderHeader.PickUpTime = Convert.ToDateTime(OrderdetailsCart.OrderHeader.PickUpDate.ToShortDateString() + " " + OrderdetailsCart.OrderHeader.PickUpTime.ToShortTimeString());

            List<OrderdetailsCart> details = new List<OrderdetailsCart>();
            _unitOfWork.OrderHeaderRepository.Add(OrderdetailsCart.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in OrderdetailsCart.CartList)
            {
                item.MenuItems = _unitOfWork.MenuItemsRepository.GetFirstOrDefault(m => m.Id == item.MenuItemID);
                OrderDetails orderDetails = new OrderDetails()
                {
                    MenuItemId = item.MenuItemID,
                    OrderId = OrderdetailsCart.OrderHeader.Id,
                    Descripiton = item.MenuItems.Description,
                    Name = item.MenuItems.Name,
                    Price = item.MenuItems.Price,
                    Count = item.Count
                };
                OrderdetailsCart.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price);
                _unitOfWork.OrderDetailsRepository.Add(orderDetails);
            }
            OrderdetailsCart.OrderHeader.OrderTotal = Convert.ToDouble(String.Format("{0:.##}", OrderdetailsCart.OrderHeader.OrderTotal));
            _unitOfWork.ShoppingCartRepository.RemoveRange(OrderdetailsCart.CartList);
            HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, 0);
            _unitOfWork.Save();

            if (StripeToken != null)
            {
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(OrderdetailsCart.OrderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Order ID :" + OrderdetailsCart.OrderHeader.Id,
                    Source = StripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                OrderdetailsCart.OrderHeader.TransactionId = charge.Id;
                if (charge.Status.ToLower() == "Succeeded")
                {
                    OrderdetailsCart.OrderHeader.PaymentStatus = StaticDetails.PaymentApproved;
                    OrderdetailsCart.OrderHeader.Status = StaticDetails.StatusSubmitted;
                }
                else
                {
                    OrderdetailsCart.OrderHeader.PaymentStatus = StaticDetails.PaymentRejected;
                }
            }
            else
            {
                OrderdetailsCart.OrderHeader.PaymentStatus = StaticDetails.PaymentRejected;

            }
            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/OrderConfirmation", new { id = OrderdetailsCart.OrderHeader.Id });

        }
    }
}
