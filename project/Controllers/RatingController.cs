using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;
using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Controllers
{
    public class RatingController : Controller
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMediaRepository _mediaRepository;

        public RatingController(IRatingRepository ratingRepository, IMediaRepository mediaRepository)
        {
            _ratingRepository = ratingRepository;
            _mediaRepository = mediaRepository;
        }

        public IActionResult Index()
        {
            var ratings = _ratingRepository.GetAll();
            return View(ratings);
        }

        [HttpPost]
        public IActionResult SetRating(int trackId, int rating)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            if (rating < 1 || rating > 5)
            {
                return BadRequest();
            }

            var media = _mediaRepository.GetById(trackId);
            if (media == null)
            {
                return BadRequest();
            }

            var existing = _ratingRepository.Get(userId, trackId);
            if (existing != null)
            {
                existing.RatingValue = rating;
                _ratingRepository.Update(existing);
            }
            else
            {
                var newRating = new Models.Rating
                {
                    UserId = userId,
                    MediaItemId = trackId,
                    RatingValue = rating
                };
                _ratingRepository.Add(newRating);
            }

            // Answer with latest average to allow future UI refresh if needed
            return Ok(new { average = media.AverageRating });
        }
    }
}