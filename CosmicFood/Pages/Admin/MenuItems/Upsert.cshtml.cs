using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.ViewModels;
using Utility;

namespace CosmicFood.Pages.Admin.MenuItems
{
    [Authorize(Roles = StaticDetails.ManagerRole)]
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public MenuItemsViewModel MenuItems { get; set; }

        public IActionResult OnGet(int? id)
        {
            MenuItems = new MenuItemsViewModel
            {
                CategoryList = _unitOfWork.Category.GetListForDropDown(),
                FoodTypeList = _unitOfWork.FoodTypeRepository.GetListForDropdown(),
                Menu = new Models.MenuItems()
            };
            if (id != null)
            {
                MenuItems.Menu = _unitOfWork.MenuItemsRepository.GetFirstOrDefault(u => u.Id == id);
                if (MenuItems.Menu == null)
                {
                    return NotFound();
                }
            }
            return Page();

        }


        public IActionResult OnPost()
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (MenuItems.Menu.Id == 0)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(webRootPath, @"Images\MenuItems");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                MenuItems.Menu.Image = @"\Images\MenuItems\" + fileName + extension;

                _unitOfWork.MenuItemsRepository.Add(MenuItems.Menu);
            }
            else
            {
                //Edit Menu Item
                var objFromDb = _unitOfWork.MenuItemsRepository.Get(MenuItems.Menu.Id);
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"Images\MenuItems");
                    var extension = Path.GetExtension(files[0].FileName);

                    var imagePath = Path.Combine(webRootPath, objFromDb.Image.TrimStart('\\'));

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    MenuItems.Menu.Image = @"\Images\MenuItems\" + fileName + extension;
                }
                else
                {
                    MenuItems.Menu.Image = objFromDb.Image;
                }


                _unitOfWork.MenuItemsRepository.Update(MenuItems.Menu);
            }
            _unitOfWork.Save();
            return RedirectToPage("./Index");
        }
    }
}
