using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;

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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Guard against history entries that do not have a related media item loaded.
            // This can happen if the media item was deleted after being played.
            var history = _playbackRepository.GetByUser(userId)
                .Where(h => h.MediaItem != null)
                .ToList();

            return View(history);
        }

        [HttpPost]
        public IActionResult Record(int mediaItemId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            // Ensure the media item exists
            var media = _mediaRepository.GetAll().FirstOrDefault(m => m.Id == mediaItemId);
            if (media == null)
            {
                return BadRequest();
            }

            var entry = new MediaLibraryWebApp.Models.PlaybackHistory
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