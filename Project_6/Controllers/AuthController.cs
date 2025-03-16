
using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
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
            // HttpContext.Session.SetString("Username", user.Username);
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
    public IActionResult Signup(string username, string email, string password, string token)
    {
        var regToken = _context.RegistrationTokens
        .FirstOrDefault(t => t.Token == token && !t.IsUsed);

        if (regToken == null)
        {
            ViewBag.Error = "Invalid or already used registration token.";
            return View();
        }

        if (_context.Users.Any(u => u.Email == email))
        {
            ViewBag.Error = "Email is already registerd.";
            return View();
        }

        var newUser = new User
        {
            Username = username,
            Email = email,
            Password = HashPassword(password)
        };

        _context.Users.Add(newUser);
        regToken.IsUsed = true;
        _context.SaveChanges();

        ViewBag.Message = $"User {username} has been successfully registered!";

        return RedirectToAction("Login");
    }

}

