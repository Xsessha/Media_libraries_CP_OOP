using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;
using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public IActionResult Index()
        {
            var ratings = _ratingRepository.GetAll();
            return View(ratings);
        }
    }
}