using System.Security.Claims;
using MediaLibraryWebApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaLibraryWebApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoritesController(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public IActionResult Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var favorites = _favoriteRepository.GetFavorites(userId);
            return View(favorites);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var favorites = _favoriteRepository.GetFavorites(userId).Select(m => m.Id.ToString()).ToList();
            return Json(new { ids = favorites });
        }

        [HttpPost]
        public IActionResult Toggle(int mediaItemId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var isNowFavorite = _favoriteRepository.ToggleFavorite(userId, mediaItemId);
            return Json(new { isFavorite = isNowFavorite });
        }
    }
}
