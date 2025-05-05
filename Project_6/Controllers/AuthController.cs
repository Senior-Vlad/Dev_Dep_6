using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
    // private void SendEmail(string to, string subject, string body)
    // {
    //     var smtpClient = new SmtpClient("smtp.gmail.com")
    //     {
    //         Port = 587,
    //         Credentials = new NetworkCredential("your_email@gmail.com", "your_app_password"),
    //         EnableSsl = true,
    //     };

    //     smtpClient.Send("your_email@gmail.com", to, subject, body);
    // }

    [HttpPost]
    public IActionResult ResetPassword(string token, string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        var user_token = _context.RegistrationTokens.FirstOrDefault(u => u.Token == token);

        if (user == null || user_token == null)
        {
            ViewBag.Error = "Invalid token or email.";
            return View("Recover");
        }

        // Генеруємо новий випадковий пароль
        string newPassword = GenerateRandomPassword();
        user.Password = HashPassword(newPassword); // Хешуємо новий пароль
        _context.SaveChanges();

        // Відправляємо email користувачу
        //SendEmail(email, "Password Reset", $"Your new password is: {newPassword}");

        ViewBag.Message = "New password has been sent to your email.";
        return View("Recover");
    }

    [HttpPost]
    public IActionResult RecoverUsername(string token, string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        var user_token = _context.RegistrationTokens.FirstOrDefault(u => u.Token == token);
        if (user == null || user_token == null)
        {
            ViewBag.Error = "Invalid token or email.";
            return View("Recover");
        }

        // Відправляємо email користувачу з username
        //SendEmail(email, "Username Recovery", $"Your username is: {user.Username}");

        ViewBag.Message = "Your username has been sent to your email.";
        return View("Recover");
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
            if (userIdExist == null && user.Role == "Student" || user.Role == "Secretariate")
            {
                return RedirectToAction("UserInfoInput", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }
        ViewBag.Error = "Invalid username or password";
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
            Password = HashPassword(password),
            Role = regToken.Role
        };

        _context.Users.Add(newUser);
        regToken.IsUsed = true;
        _context.SaveChanges();

        ViewBag.Message = $"User {username} has been successfully registered!";

        return RedirectToAction("Login", "Auth");
    }

}

