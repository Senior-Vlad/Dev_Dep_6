using Microsoft.AspNetCore.Mvc;
using Project_6.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Project_6.Models;

public class ComplaintController : Controller
{
    private readonly ApplicationDbContext _context;

    public ComplaintController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Submit(string title, string content)
    {
        var username = HttpContext.Session.GetString("Username");
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null) return RedirectToAction("Login", "Auth");

        var complaint = new Complaint
        {
            Title = title,
            Content = content,
            UserId = user.Id
        };

        _context.Complaints.Add(complaint);
        _context.SaveChanges();

        return RedirectToAction("MyInfo", "Student");
    }

    [HttpGet]
    public IActionResult List()
    {
        var complaints = _context.Complaints
            .Select(c => new ComplaintViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                SubmittedAt = c.SubmittedAt,
                IsResolved = c.IsResolved,
                Username = c.User.Username,
                Email = c.User.Email
            }).ToList();

        return View(complaints);
    }

    [HttpPost]
    public IActionResult ToggleStatus(int id)
    {
        var complaint = _context.Complaints.FirstOrDefault(c => c.Id == id);
        if (complaint != null)
        {
            complaint.IsResolved = !complaint.IsResolved;
            _context.SaveChanges();
        }
        return RedirectToAction("List");
    }
}
