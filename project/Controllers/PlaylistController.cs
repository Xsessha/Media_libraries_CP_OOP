using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Repositories;

namespace MediaLibraryWebApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistController(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public IActionResult Index()
        {
            var playlists = _playlistRepository.GetAll();
            return View(playlists);
        }
    }
}