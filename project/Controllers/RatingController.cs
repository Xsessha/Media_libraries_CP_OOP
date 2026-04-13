using System;
using System.Linq;
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

        // Конструктор, який підключає бази даних (репозиторії)
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
            // 1. ТИМЧАСОВЕ ВИРІШЕННЯ ДЛЯ КОРИСТУВАЧА
            int userId = 1; 

            if (rating < 1 || rating > 5)
            {
                return BadRequest("Некоректна оцінка");
            }

            var media = _mediaRepository.GetById(trackId);
            if (media == null)
            {
                return NotFound("Трек не знайдено");
            }

            // Зберігаємо або оновлюємо голос
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

            // 2. ПРАВИЛЬНЕ ПОВЕРНЕННЯ ДЛЯ JS
            var allTrackRatings = _ratingRepository.GetAll().Where(r => r.MediaItemId == trackId).ToList();
            double newAverage = allTrackRatings.Any() ? allTrackRatings.Average(r => r.RatingValue) : 0;
            
            // Заокруглюємо до 1 знака після коми, щоб JS отримав гарне число (напр. 4.5)
            newAverage = Math.Round(newAverage, 1);

            return Ok(new { average = newAverage });
        }
    }
}