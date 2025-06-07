using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Users.user.Downloads.Dev_Dep_6.Project_6.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Auth");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
