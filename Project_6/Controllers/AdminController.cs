using Microsoft.AspNetCore.Mvc;
using Project_6.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Veryfing if the user is an admin
        var username = HttpContext.Session.GetString("Username");
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null || (user.Role != "admin" && user.Role != "superadmin"))
        {
            return RedirectToAction("Login");
        }

        return View();
    }
    public IActionResult Home()
    {
        // Veryfing if the user is an admin
        var username = HttpContext.Session.GetString("Username");
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null || (user.Role != "admin" && user.Role != "superadmin"))
        {
            return RedirectToAction("Login");
        }

        return View();
    }


    [HttpPost]
    public IActionResult CreateToken(string token, string role)
    {
        var username = HttpContext.Session.GetString("Username");
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        // Veryfing the user's rightsПеревірка прав користувача:
        if (user == null || (user.Role != "admin" && user.Role != "superadmin"))
        {
            return RedirectToAction("Login", "Auth");
        }

        // admin can create tokens only for user & secretariate
        if (user.Role == "admin" && (role == "admin" || role == "superadmin"))
        {
            ViewBag.Error = "Insufficient rights to assign admin roles.";
            return View("Index");
        }
        if (!_context.RegistrationTokens.Any(t => t.Token == token))
        {
            var newToken = new RegistrationToken
            {
                Token = token,
                IsUsed = false,
                CreatedAt = DateTime.UtcNow,
                Role = role
            };

            _context.RegistrationTokens.Add(newToken);
            _context.SaveChanges();

            ViewBag.Message = $"Token '{token}' created successfully!";
        }
        else
        {
            ViewBag.Error = "Token already exists!";
        }


        return View("Index");
    }

    public IActionResult CreateToken()
    {
        return View("Index");
    }

    public IActionResult StudentList()
    {
        var username = HttpContext.Session.GetString("Username");
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        var checkResult = RoleCheck(username);

        if (checkResult != null)
        {
            return checkResult;
        }
        var students = _context.UserInfos.Where(info => info.User.Role == "student")
        .Select(info => new
        {
            Id = info.Id,
            FirstName = info.FirstName,
            LastName = info.LastName,
            Username = info.User.Username,
            Email = info.Email,
            Role = info.User.Role
        }).ToList();
        ViewBag.Students = students;
        return View(students);
    }

    private IActionResult RoleCheck(string username)
    {
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null || (user.Role != "admin" && user.Role != "superadmin"))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (user.Role != "admin")
        {
            ViewBag.Error = "Insufficient rights to assign admin roles.";
            return View("Index");
        }
        return null;
    }

    public IActionResult DeleteStudent(int Id)
    {
        var userExist = _context.UserInfos.FirstOrDefault(s => s.Id == Id);
        if (userExist != null)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userExist.Id);
            _context.UserInfos.Remove(userExist);
            _context.SaveChanges();
        }
        return RedirectToAction("StudentList");
    }
}
