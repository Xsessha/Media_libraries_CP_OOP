using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;
using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Controllers
{
    public class PlaybackController : Controller
    {
        private readonly IPlaybackRepository _playbackRepository;

        public PlaybackController(IPlaybackRepository playbackRepository)
        {
            _playbackRepository = playbackRepository;
        }

        public IActionResult History()
        {
            var history = _playbackRepository.GetAll();
            return View(history);
        }
    }
}