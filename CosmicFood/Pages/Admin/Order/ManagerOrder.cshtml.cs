using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.ViewModels;
using Utility;

namespace CosmicFood.Pages.Admin.Order
{
    public class ManagerOrderModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManagerOrderModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public List<OrderDetailsVM> OrderDetailsVM { get; set; }

      
        public void OnGet()
        {
            OrderDetailsVM = new List<OrderDetailsVM>();

            List<OrderHeader> orderHeader = _unitOfWork.OrderHeaderRepository.
                GetAll(o => o.Status == StaticDetails.PaymentPending|| o.Status == StaticDetails.StatusinProcess).
                OrderByDescending(u => u.PickUpTime).ToList();

            foreach (var item in orderHeader)
            {
                OrderDetailsVM individual = new OrderDetailsVM
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetailsRepository.GetAll(o => o.OrderId == item.Id).ToList()
                };
                OrderDetailsVM.Add(individual);
            }

        }
    }
}
