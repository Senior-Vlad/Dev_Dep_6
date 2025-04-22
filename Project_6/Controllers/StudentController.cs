using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_6.Models;

namespace Users.user.Downloads.Dev_Dep_6.Project_6.bin
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult MyInfo()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Role != "student")
            {
                return RedirectToAction("Login", "Auth");
            }

            var userInfo = _context.UserInfos.FirstOrDefault(info => info.UserId == user.Id);

            if (userInfo == null)
            {
                ViewBag.Message = "No personal inforamtion found.";
                return View();
            }
            return View(userInfo);
        }

        [HttpPost]
        public IActionResult UpdateStudentInfo(UserInfo model)
        {
            var userInfo = _context.UserInfos.FirstOrDefault(u => u.Id == model.Id);

            if (userInfo == null)
            {
                ViewBag.Message = "User does not found!";
                return RedirectToAction("MyInfo");
            }

            userInfo.FirstName = model.FirstName;
            userInfo.LastName = model.LastName;
            userInfo.Email = model.Email;
            userInfo.PhoneNumber = model.PhoneNumber;
            userInfo.YearOfStudy = model.YearOfStudy;
            userInfo.Faculty = model.Faculty;
            userInfo.Major = model.Major;
            userInfo.CreationDate = DateTime.Now;

            int result = _context.SaveChanges();
            Console.WriteLine("Rows affected: " + result);

            ViewBag.Message = "Information updated successfully!";
            TempData["Message"] = "Information updated successfully!";

            return RedirectToAction("MyInfo");
        }
    }
}
