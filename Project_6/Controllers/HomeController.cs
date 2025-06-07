using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project_6.Models;

namespace Project_6.Controllers;

public class HomeController : Controller
{

    private readonly ApplicationDbContext _context;

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var username = HttpContext.Session.GetString("Username");

        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Auth");
        }
        return View();
    }

    [HttpGet]
    public IActionResult UserInfoInput()
    {
        var username = HttpContext.Session.GetString("Username");

        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Auth");
        }

        ViewBag.Username = username;
        return View();
    }

    [HttpPost]
    public IActionResult SubmitField(string userInput)
    {
        var username = HttpContext.Session.GetString("Username");

        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Login", "Auth");
        }

        ViewBag.SubmittedValue = userInput;
        return View("SubmissionResult");
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public JsonResult GetLatestOgloszenie()
    {
        var latest = _context.Ogloszenia
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefault();

        if (latest != null)
        {
            return Json(new
            {
                title = latest.Title,
                message = latest.Message,
                createdAt = latest.CreatedAt.ToString("dd.MM.yyyy HH:mm"),
                createdBy = latest.CreateByRole
            });
        }

        return Json(null);
    }

}
