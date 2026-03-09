using Microsoft.AspNetCore.Mvc;

namespace MediaLibraryWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Media");
        }
    }
}