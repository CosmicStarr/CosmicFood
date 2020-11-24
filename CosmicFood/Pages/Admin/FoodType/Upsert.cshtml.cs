using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CosmicFood.Pages.Admin.FoodType
{
    [Authorize(Roles = StaticDetails.ManagerRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpsertModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public Models.FoodType FoodType { get; set; }
        public IActionResult OnGet(int? Id)
        {
            FoodType = new Models.FoodType();
            if (Id != null)
            {
                _unitOfWork.FoodTypeRepository.GetFirstOrDefault(f => f.Id == Id.GetValueOrDefault());
                if (FoodType == null)
                {
                    NotFound();
                }
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (FoodType.Id == 0)
            {
                _unitOfWork.FoodTypeRepository.Add(FoodType);
            }
            else
            {
                _unitOfWork.FoodTypeRepository.Update(FoodType);
            }
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }
}
