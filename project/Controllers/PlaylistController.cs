using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;
using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMediaRepository _mediaRepository;

        public PlaylistController(IPlaylistRepository playlistRepository, IMediaRepository mediaRepository)
        {
            _playlistRepository = playlistRepository;
            _mediaRepository = mediaRepository;
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId)) return userId;
            return null;
        }

        public IActionResult Index()
        {
            var userId = GetUserId();
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            var playlists = _playlistRepository.GetAll().Where(p => p.UserId == userId.Value).ToList();
            return View(playlists);
        }

        [HttpPost]
        public IActionResult Create(string name)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["PlaylistError"] = "Playlist name is required.";
                return RedirectToAction("Index");
            }

            var playlist = new Playlist
            {
                Name = name.Trim(),
                UserId = userId.Value,
                DateCreated = DateTime.Now
            };

            _playlistRepository.Add(playlist);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return RedirectToAction("Login", "Account");

            var playlist = _playlistRepository.GetById(id);
            if (playlist == null || playlist.UserId != userId.Value) return NotFound();

            var allMedia = _mediaRepository.GetAll().ToList();
            var availableMedia = allMedia.Where(m => !playlist.Tracks.Any(t => t.MediaItemId == m.Id)).ToList();

            ViewData["AvailableMedia"] = availableMedia;
            return View(playlist);
        }

        [HttpPost]
        public IActionResult AddTrack(int playlistId, int mediaItemId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var playlist = _playlistRepository.GetById(playlistId);
            if (playlist == null || playlist.UserId != userId.Value) return NotFound();

            if (playlist.Tracks.Any(t => t.MediaItemId == mediaItemId))
            {
                return BadRequest("Track already in playlist.");
            }

            var position = playlist.Tracks.Any() ? playlist.Tracks.Max(t => t.Position) + 1 : 1;
            playlist.Tracks.Add(new PlaylistTrack { PlaylistId = playlistId, MediaItemId = mediaItemId, Position = position });
            _playlistRepository.Update(playlist);

            return RedirectToAction("Details", new { id = playlistId });
        }

        [HttpPost]
        public IActionResult RemoveTrack(int playlistId, int mediaItemId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var playlist = _playlistRepository.GetById(playlistId);
            if (playlist == null || playlist.UserId != userId.Value) return NotFound();

            var track = playlist.Tracks.FirstOrDefault(t => t.MediaItemId == mediaItemId);
            if (track != null)
            {
                playlist.Tracks.Remove(track);
                // Reorder positions
                var ordered = playlist.Tracks.OrderBy(t => t.Position).ToList();
                for (int i = 0; i < ordered.Count; i++) ordered[i].Position = i + 1;
                _playlistRepository.Update(playlist);
            }

            return RedirectToAction("Details", new { id = playlistId });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var playlist = _playlistRepository.GetById(id);
            if (playlist == null || playlist.UserId != userId.Value) return NotFound();

            _playlistRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}