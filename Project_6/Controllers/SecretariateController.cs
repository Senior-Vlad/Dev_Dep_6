using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_6.Models;
using Project_6.ViewModels;
using Users.user.Downloads.Dev_Dep_6.Project_6.Models;

namespace Users.user.Downloads.Dev_Dep_6.Project_6.Controllers
{
    public class SecretariateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SecretariateController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Role != "secretariate")
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Home()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Role != "secretariate")
            {
                return RedirectToAction("Login");
            }

            var students = _context.UserInfos
                .Where(info => info.User.Role == "student")
                .Select(info => new StudentViewModel
                {
                    Id = info.Id,
                    FirstName = info.FirstName,
                    LastName = info.LastName,
                    Username = info.User.Username,
                    Email = info.Email,
                    Role = info.User.Role
                })
                .ToList();

            return View(students);
        }
        public IActionResult SecretariateZgloszenia()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || user.Role != "secretariate")
            {
                return RedirectToAction("Login", "Auth");
            }

            var SecretariateZgloszenia = _context.Zgloszenia
                .Include(z => z.User)
                .Include(z => z.Statusy)
                .Where(z => z.Category == "Dziekanat")
                .ToList();

            return View(SecretariateZgloszenia);
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
                return RedirectToAction("Home");
            }

            Console.WriteLine("❌ ModelState is invalid:");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($" - {error.ErrorMessage}");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ZmienStatus(int zgloszenieId)
        {
            var zgloszenie = _context.Zgloszenia
                .Include(z => z.Statusy)
                .FirstOrDefault(z => z.Id == zgloszenieId);

            if (zgloszenie == null)
            {
                return NotFound();
            }

            var nowyStatus = new ZgloszenieStatus
            {
                Status = "Rozwiązane",
                DataZmiany = DateTime.Now,
                ZgloszenieId = zgloszenie.Id,
                Zgloszenie = zgloszenie
            };

            zgloszenie.Statusy ??= new List<ZgloszenieStatus>();
            zgloszenie.Statusy.Add(nowyStatus);
            _context.ZgloszenieStatuses.Add(nowyStatus);
            _context.SaveChanges();

            return RedirectToAction("SecretariateZgloszenia");
        }

        public IActionResult StudentList()
        {
            var username = HttpContext.Session.GetString("Username");
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            var students = _context.UserInfos
                .Where(info => info.User.Role == "student")
                .Select(info => new StudentViewModel
                {
                    Id = info.Id,
                    FirstName = info.FirstName,
                    LastName = info.LastName,
                    Username = info.User.Username,
                    Email = info.Email,
                    Role = info.User.Role
                })
                .ToList();

            return View(students);
        }

        public IActionResult CreateOgloszenia(OgloszenieViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            Console.WriteLine("UserId: " + userId);
            if (ModelState.IsValid)
            {
                var ogloszenie = new Ogloszenie
                {
                    Title = model.Title,
                    Message = model.Message,
                    CreateByRole = "Secretariat",
                    UserId = userId.Value,
                    CreatedAt = DateTime.Now
                };

                _context.Ogloszenia.Add(ogloszenie);
                _context.SaveChanges();

                return RedirectToAction("Home", "Secretariate");
            }

            return View(model);
        }

    }
}
