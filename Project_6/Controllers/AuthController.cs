
using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;
    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null)
        {
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Home");
        }
        ViewBag.Error = "Invalid username or password";
        return View();
    }


    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Signup(string username, string email, string password)
    {
        if (_context.Users.Any(u => u.Email == email))
        {
            ViewBag.Error = "Email is already registerd.";
            return View();
        }

        var newUser = new User
        {
            Username = username,
            Email = email,
            Password = password
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();


        // database logic to be implemented
        ViewBag.Message = $"User {username} has been successfully registered!";

        return RedirectToAction("Login");
    }

}

