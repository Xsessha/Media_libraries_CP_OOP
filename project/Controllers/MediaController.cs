using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;

namespace MediaLibraryWebApp.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaRepository _mediaRepository;

        public MediaController(IMediaRepository mediaRepository)
        {
            _mediaRepository = mediaRepository;
        }

        public IActionResult Index(string? q, string? sort)
        {
            ViewData["SearchQuery"] = q ?? string.Empty;
            ViewData["Sort"] = sort ?? "title";

            var media = _mediaRepository.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var search = q.Trim().ToLower();
                media = media.Where(m => m.Title.ToLower().Contains(search)
                                         || m.Artist.ToLower().Contains(search)
                                         || m.Genre.ToLower().Contains(search));
            }

            media = sort?.ToLower() switch
            {
                "title" => media.OrderBy(m => m.Title),
                "rating" => media.OrderByDescending(m => m.AverageRating),
                "plays" => media.OrderByDescending(m => m.PlayCount),
                "date" => media.OrderByDescending(m => m.DateAdded),
                _ => media.OrderBy(m => m.Title)
            };

            return View(media.ToList());
        }
    }
}