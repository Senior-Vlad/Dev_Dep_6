
using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        if (username == "admin" && password == "123")
        {
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Error = "Invalid credentials";
        return View();
    }


 public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Signup(string username, string email, string password)
    {
        // database logic to be implemented
        ViewBag.Message = $"User {username} has been successfully registered!";
    
        return RedirectToAction("Index", "Home");
    }

}

