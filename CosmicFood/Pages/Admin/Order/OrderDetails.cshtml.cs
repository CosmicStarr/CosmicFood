using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.ViewModels;

namespace CosmicFood.Pages.Admin.Order
{
    public class OrderDetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public OrderDetailsVM orderDetailsVM { get; set; }
        public void OnGet(int Id)
        {
            orderDetailsVM = new OrderDetailsVM()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(o => o.Id == Id),
                OrderDetails = _unitOfWork.OrderDetailsRepository.GetAll(m => m.OrderId == Id).ToList()
            };

            orderDetailsVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(a => a.Id == orderDetailsVM.OrderHeader.UserId);
            
        }
    }
}
