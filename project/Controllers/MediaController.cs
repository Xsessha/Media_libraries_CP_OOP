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

        public IActionResult Index()
        {
            var media = _mediaRepository.GetAll();
            return View(media);
        }
    }
}