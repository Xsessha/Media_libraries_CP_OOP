using System.Security.Claims;
using MediaLibraryWebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace MediaLibraryWebApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _db;

        public ProfileController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _db.Users.Find(userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
    }
}
