using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Project_6.Models;

public class AuthController : Controller
{
    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
    public IActionResult Recover()
    {
        return View();
    }
    private string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private readonly ApplicationDbContext _context;
    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Login()
    {
        var username = HttpContext.Session.GetString("Username");
        if (!string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        string hashedPassword = HashPassword(password);// hash password
        var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);
        if (user != null)
        {
            var userInfoId = _context.UserInfos.FirstOrDefault(u => u.UserId == user.Id);
            if (userInfoId != null)
            {
                HttpContext.Session.SetString("FirstName", userInfoId.FirstName);
                HttpContext.Session.SetString("LastName", userInfoId.LastName);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);
            // Creating a session with the user's role
            HttpContext.Session.SetString("Role", user.Role ?? "user");//?
            var sessionIdUser = HttpContext.Session.GetInt32("UserId");
            var userIdExist = _context.UserInfos.FirstOrDefault(u => u.UserId == sessionIdUser);
            Console.WriteLine($"userIdExist: {userIdExist}");
            if (user.Role == "admin")
            {
                return RedirectToAction("Home", "Admin");
            }
            if (userIdExist == null && user.Role == "student")
            {
                return RedirectToAction("UserInfoInput", "Home");
            }
            if (userIdExist == null && user.Role == "secretariate")
            {
                return RedirectToAction("Home", "Secretariate");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        ViewBag.Error = "Niepoprawna nazwa użytkownika lub hasło";
        return View();
    }

    public IActionResult Logout()
    {
        // clear session
        HttpContext.Session.Clear();

        // translates to login page
        return RedirectToAction("Login", "Auth");
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
            ViewBag.Error = "Niepoprawny lub wykorzystany token.";
            return View();
        }

        if (_context.Users.Any(u => u.Email == email))
        {
            ViewBag.Error = "Użytkownik z tym adresem email już istnieje.";
            return View();
        }

        var newUser = new User
        {
            Username = username,
            Email = email,
            Password = HashPassword(password),
            Role = regToken.Role
        };

        _context.Users.Add(newUser);
        regToken.IsUsed = true;
        _context.SaveChanges();

        ViewBag.Message = $"Uzytkownik {username} został zarejestrowany!";

        return RedirectToAction("Login", "Auth");
    }

    // Recover username
    [HttpPost]
    public IActionResult RecoverUsername(RecoverViewModel model)
    {
        var token = _context.RegistrationTokens
            .FirstOrDefault(t => t.Token == model.Token && !t.IsUsed);

        if (token == null)
        {
            ModelState.AddModelError("", "Nieprawidłowy lub wykorzystany token.");
            return View("Recover", model);
        }

        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user != null)
        {
            // Optionally mark token as used
            token.IsUsed = true;
            _context.SaveChanges();
            Console.WriteLine("Username: " + user.Username);

            ViewBag.Username = user.Username;
            return View("ShowUsername");
        }

        ModelState.AddModelError("", "Nie znaleziono użytkownika.");
        return View("Recover", model);
    }

    // Begin password reset
    [HttpPost]
    public IActionResult ResetPassword(RecoverViewModel model)
    {
        var token = _context.RegistrationTokens
            .FirstOrDefault(t => t.Token == model.Token && !t.IsUsed);

        if (token == null)
        {
            ModelState.AddModelError("", "Nieprawidłowy lub wykorzystany token.");
            return View("Recover", model);
        }

        var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        if (user != null)
        {
            Console.WriteLine("I am here: " + user.Username);
            Console.WriteLine("I am token: " + model.Token);
            Console.WriteLine("I am email: " + model.Email);

            return RedirectToAction("SetNewPassword", model);
        }

        ModelState.AddModelError("", "Nie znaleziono użytkownika.");
        return View("Recover");
    }

    // Set new password form
    public IActionResult SetNewPassword(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Recover");
        }

        var model = new RecoverViewModel
        {
            Email = email,
            Token = token
        };

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> SetNewPassword(string email, string token, string newPassword)
    {

        var registrationToken = await _context.RegistrationTokens
            .FirstOrDefaultAsync(t => t.Token == token && !t.IsUsed);

        if (registrationToken == null)
        {
            ModelState.AddModelError("", "Token jest nieprawidłowy lub został już użyty.");
            return View();
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            ModelState.AddModelError("", "Nie znaleziono użytkownika z podanym adresem email.");
            return View();
        }

        user.Password = HashPassword(newPassword);

        _context.Entry(user).Property(u => u.Password).IsModified = true;

        registrationToken.IsUsed = true;

        _context.Update(registrationToken);
        _context.Update(user);


        await _context.SaveChangesAsync();

        TempData["Message"] = "Hasło zostało zaktualizowane.";
        return RedirectToAction("Login", "Auth");
    }


    [HttpGet]
    public IActionResult ShowUsername()
    {
        var username = TempData["RecoveredUsername"] as string;
        if (username == null)
        {
            return RedirectToAction("Recover");
        }

        ViewBag.Username = username;
        return View();
    }



}

