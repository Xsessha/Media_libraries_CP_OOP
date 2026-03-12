using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaLibraryWebApp.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MediaItem> GetFavorites(int userId)
        {
            return _context.FavoriteMedia
                .Where(f => f.UserId == userId)
                .Include(f => f.MediaItem)
                .Select(f => f.MediaItem)
                .ToList();
        }

        public bool IsFavorite(int userId, int mediaItemId)
        {
            return _context.FavoriteMedia.Any(f => f.UserId == userId && f.MediaItemId == mediaItemId);
        }

        public bool ToggleFavorite(int userId, int mediaItemId)
        {
            var existing = _context.FavoriteMedia.FirstOrDefault(f => f.UserId == userId && f.MediaItemId == mediaItemId);
            if (existing != null)
            {
                _context.FavoriteMedia.Remove(existing);
                _context.SaveChanges();
                return false;
            }

            _context.FavoriteMedia.Add(new FavoriteMedia
            {
                UserId = userId,
                MediaItemId = mediaItemId
            });
            _context.SaveChanges();
            return true;
        }
    }
}
