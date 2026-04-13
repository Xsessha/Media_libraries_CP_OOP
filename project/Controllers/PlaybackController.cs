using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;
using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Controllers
{
    public class PlaybackController : Controller
    {
        private readonly IPlaybackRepository _playbackRepository;
        private readonly IMediaRepository _mediaRepository;

        public PlaybackController(IPlaybackRepository playbackRepository, IMediaRepository mediaRepository)
        {
            _playbackRepository = playbackRepository;
            _mediaRepository = mediaRepository;
        }

        public IActionResult History()
        {
            // 1. Перевіряємо авторизацію користувача
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // 2. Отримуємо дані з бази
            var history = _playbackRepository.GetByUser(userId);

            // 3. ВИПРАВЛЕНО: Правильне і безпечне фільтрування
            // Створюємо порожній список за замовчуванням
            var safeHistory = new List<PlaybackHistory>();
            
            if (history != null)
            {
                safeHistory = history
                    .ToList() // Спочатку завантажуємо всі записи в пам'ять (щоб БД не впала)
                    .Where(h => h != null && h.MediaItem != null) // Безпечно фільтруємо "биті" записи
                    .OrderByDescending(h => h.DatePlayed) // Додав сортування, щоб нові треки були зверху
                    .ToList();
            }

            return View(safeHistory);
        }

        [HttpPost]
        public IActionResult Record(int mediaItemId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var media = _mediaRepository.GetAll()
                .FirstOrDefault(m => m.Id == mediaItemId);

            if (media == null)
            {
                return BadRequest("Трек не знайдено.");
            }

            // Оновлюємо лічильник прослуховувань
            media.PlayCount += 1;
            _mediaRepository.Update(media);

            // Створюємо запис для історії
            var entry = new PlaybackHistory
            {
                UserId = userId,
                MediaItemId = mediaItemId,
                DatePlayed = DateTime.Now
            };

            _playbackRepository.Add(entry);

            return Ok();
        }
    }
}