using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;
using Utility;

namespace CosmicFood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get(string status = null)
        {
            List<OrderDetailsVM> orderDetailsVM = new List<OrderDetailsVM>();
            IEnumerable<OrderHeader> orderHeaderList; 

            if(User.IsInRole(StaticDetails.CustomerRole))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaderList = _unitOfWork.OrderHeaderRepository.GetAll(o => o.UserId == claims.Value,null,"ApplicationUser");
            }
            else
            {
                orderHeaderList = _unitOfWork.OrderHeaderRepository.GetAll(null, null, "ApplicationUser");
            }

            if (status == "Cancelled")
            {
                orderHeaderList = orderHeaderList.Where(o => o.Status == StaticDetails.StatusCancelled || o.Status == StaticDetails.StatusRefunded || o.Status == StaticDetails.PaymentRejected);
            }
            else
            {
                if (status == "Completed")
                {
                    orderHeaderList = orderHeaderList.Where(o => o.Status == StaticDetails.StatusCompleted);
                }
                else
                {
                    orderHeaderList = orderHeaderList.Where(o => o.Status == StaticDetails.StatusReady || o.Status == StaticDetails.StatusinProcess || o.Status == StaticDetails.PaymentPending || o.Status == StaticDetails.StatusSubmitted);

                }
            }

            foreach (var item in orderHeaderList)
            {
                OrderDetailsVM individual = new OrderDetailsVM
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetailsRepository.GetAll(o => o.OrderId == item.Id).ToList()
                };
                orderDetailsVM.Add(individual);
            }
            return Json(new { data = orderDetailsVM });

        }
    }
}
