using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmicFood.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;
        public MenuItemsController(IUnitOfWork unitOfWork, IWebHostEnvironment HostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = HostEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.MenuItemsRepository.GetAll(includeproperties: "Category,FoodType") });
        }
        [HttpDelete("Id")]
        public IActionResult Delete(int? Id)
        {
            var objfromdb = _unitOfWork.MenuItemsRepository.GetFirstOrDefault(m => m.Id == Id);
            try
            {
                if (objfromdb == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }
                var imagePath = Path.Combine(_HostEnvironment.WebRootPath, objfromdb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _unitOfWork.MenuItemsRepository.Remove(objfromdb);
                _unitOfWork.Save();

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            return Json(new { success = true, message = "Wonderful! Its gone." });
        }

    }
}
