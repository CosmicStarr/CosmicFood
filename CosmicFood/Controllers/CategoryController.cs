using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmicFood.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { Data = _unitOfWork.Category.GetAll() });
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objfromDb = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (objfromDb == null)
            {
                return Json(new { success = false, message = "Somthing went completely wrong while deleting" });
            }
            _unitOfWork.Category.Remove(objfromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Wonderingful! Its gone." });

        }
    }
}
