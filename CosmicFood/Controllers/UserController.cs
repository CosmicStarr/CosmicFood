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
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.ApplicationUserRepository.GetAll() });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody] string Id)
        {
            var objfromdb = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == Id);
            if (objfromdb == null)
            {
                return Json(new { success = false, message = "Somthing went wrong while Locking or Unlocking." });
            }
            if (objfromdb.LockoutEnd != null && objfromdb.LockoutEnd > DateTime.Now)
            {
                objfromdb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objfromdb.LockoutEnd = DateTime.Now.AddYears(100);
            }
            _unitOfWork.Save();
            return Json(new { success = true, message = "Wonderful! Operation is a success!" });
        }
    }
}
