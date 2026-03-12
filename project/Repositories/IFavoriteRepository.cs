using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Repositories
{
    public interface IFavoriteRepository
    {
        IEnumerable<MediaItem> GetFavorites(int userId);
        bool ToggleFavorite(int userId, int mediaItemId);
        bool IsFavorite(int userId, int mediaItemId);
    }
}
