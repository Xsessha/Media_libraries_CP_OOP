using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;
using System.Linq;
using System.Collections.Generic;

namespace MediaLibraryWebApp.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IRatingRepository _ratingRepository;

        public MediaController(IMediaRepository mediaRepository, IRatingRepository ratingRepository)
        {
            _mediaRepository = mediaRepository;
            _ratingRepository = ratingRepository;
        }

        public IActionResult Index(string? q, string? sort)
        {
            ViewData["SearchQuery"] = q ?? string.Empty;
            ViewData["Sort"] = sort ?? "title";

            var mediaList = _mediaRepository.GetAll().ToList();

            // ✅ КОРИСТУВАЧ
            int currentUserId = 1;

            var userRatings = _ratingRepository.GetAll()
                .Where(r => r.UserId == currentUserId)
                .ToDictionary(r => r.MediaItemId, r => r.RatingValue);

            // 🔍 ПОШУК
            if (!string.IsNullOrWhiteSpace(q))
            {
                var search = q.Trim().ToLower();
                mediaList = mediaList.Where(m =>
                    m.Title.ToLower().Contains(search) ||
                    m.Artist.ToLower().Contains(search) ||
                    m.Genre.ToLower().Contains(search)
                ).ToList();
            }

            // ✅ СОРТУВАННЯ (ВИПРАВЛЕНО)
            mediaList = sort?.ToLower() switch
            {
                "title" => mediaList.OrderBy(m => m.Title).ToList(),

                "rating" => mediaList
                    .OrderByDescending(m => userRatings.ContainsKey(m.Id) ? userRatings[m.Id] : 0)
                    .ToList(),

                "plays" => mediaList.OrderByDescending(m => m.PlayCount).ToList(),

                _ => mediaList.OrderBy(m => m.Title).ToList()
            };

            ViewBag.UserRatings = userRatings;

            return View(mediaList);
        }
    }
}