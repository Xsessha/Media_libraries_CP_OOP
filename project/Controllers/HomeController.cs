using Microsoft.AspNetCore.Mvc;
using MediaLibraryWebApp.Models;
using System.Linq;
using MediaLibraryWebApp.Repositories; 

namespace MediaLibraryWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediaRepository _repository;

        public HomeController(IMediaRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            // Отримуємо всі медіа-файли
            var allMedia = _repository.GetAll(); 

            var viewModel = new HomeViewModel
            {
                // 1. Топ рейтингів: використовуємо AverageRating замість Rating
                TopRatedTracks = allMedia.OrderByDescending(m => m.AverageRating).Take(5).ToList(),

                // 2. Новинки: використовуємо DateAdded для сортування за датою замість Id
                RecentlyAdded = allMedia.OrderByDescending(m => m.DateAdded).Take(5).ToList(),

                // 3. Популярне: використовуємо PlayCount замість Plays
                MostPlayedTracks = allMedia.OrderByDescending(m => m.PlayCount).Take(5).ToList(),

                // 4. Жанри
                Genres = allMedia.Select(m => m.Genre).Distinct().ToList(),

                // 5. Статистика: також використовуємо PlayCount
                TotalTracksCount = allMedia.Count(),
                TotalPlaysCount = allMedia.Sum(m => m.PlayCount)
            };

            return View(viewModel);
        }
    }
}