using Microsoft.AspNetCore.Mvc;

namespace SocialHighload.Controllers
{
    public class ProfileController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Friends()
        {
            return View();
        }
    }
}