using Microsoft.AspNetCore.Mvc;

namespace Project_6.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                return Json(new { success = true, response = message });
            }
            return Json(new { success = false, response = "Message cannot be empty!" });
        }
    }
}