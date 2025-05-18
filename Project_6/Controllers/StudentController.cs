using System;
using System.Collections.Generic;
using System.Linq;
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


        public IActionResult CreateZgloszenie()
        {
            // var username = HttpContext.Session.GetString("Username");
            // var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // if (user == null || user.Role != "student" || user.Role != "secretariate")
            // {
            //     return RedirectToAction("Login", "Auth");
            // }


            return View();
        }

        public IActionResult MyInfo()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Role != "student")
                return RedirectToAction("Login", "Auth");

            var userInfo = _context.UserInfos.FirstOrDefault(info => info.UserId == user.Id);

            if (userInfo == null)
            {
                ViewBag.Message = "No personal information found.";
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
                ViewBag.Message = "User not found!";
                return RedirectToAction("MyInfo");
            }

            userInfo.FirstName = model.FirstName;
            userInfo.LastName = model.LastName;
            userInfo.Email = model.Email;
            userInfo.PhoneNumber = model.PhoneNumber;
            userInfo.YearOfStudy = model.YearOfStudy;
            userInfo.Faculty = model.Faculty;
            userInfo.Major = model.Major;
            // userInfo.CreationDate = DateTime.Now;

            _context.SaveChanges();

            TempData["Message"] = "Information updated successfully!";
            return RedirectToAction("MyInfo");
        }

        // 👉 Показати форму
        [HttpGet]
        public IActionResult Zgloszenie()
        {
            return View();
        }

        // 👉 Обробити форму
        [HttpPost]
        public IActionResult Zgloszenie(Zgloszenie model)
        {
            Console.WriteLine("POST: Zgloszenie was triggered");

            var username = HttpContext.Session.GetString("Username");
            Console.WriteLine($"🧠 Username in session: {username}");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                Console.WriteLine("❌ User from session not found.");
                return RedirectToAction("Login", "Auth");
            }

            model.StudentId = user.Id;
            model.UserId = user.Id;
            model.DataDodania = DateTime.Now;

            var status = new ZgloszenieStatus
            {
                Status = "Nowe",
                DataZmiany = DateTime.Now,
                Zgloszenie = model
            };

            model.Statusy = new List<ZgloszenieStatus> { status };

            if (ModelState.IsValid)
            {
                _context.Zgloszenia.Add(model);
                //_context.ZgloszenieStatuses.Add(status);
                _context.SaveChanges();
                Console.WriteLine("✅ Zgloszenie saved successfully.");
                return RedirectToAction("MyInfo");
            }

            Console.WriteLine("❌ ModelState is invalid:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($" - {error.ErrorMessage}");
            }

            return View(model);
        }
    }
}
