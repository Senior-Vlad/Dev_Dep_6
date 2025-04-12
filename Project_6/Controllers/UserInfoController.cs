using Microsoft.AspNetCore.Mvc;
using Project_6.Models;

namespace Project_6.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display the form
        public IActionResult Index()
        {
            var model = new UserInfoModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitField(UserInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId.HasValue)
                {
                    var userInfo = new UserInfo
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        YearOfStudy = model.YearOfStudy,
                        Faculty = model.Faculty,
                        Major = model.Major,
                        UserId = userId.Value
                    };

                    // Save to database
                    _context.UserInfos.Add(userInfo);
                    _context.SaveChanges();

                    ViewBag.Message = "Information saved successfully.";
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    ViewBag.Error = "User is not logged in or session has expired.";
                    return View("Index", model);
                }
            }

            ViewBag.Error = "Please fill out the form correctly.";
            return View("Index", model);
        }

    }
}
