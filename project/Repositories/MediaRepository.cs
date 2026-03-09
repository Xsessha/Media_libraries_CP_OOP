using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaLibraryWebApp.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly AppDbContext _context;

        public MediaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MediaItem> GetAll()
        {
            return _context.MediaItems
                .Include(m => m.Ratings)
                .Include(m => m.PlaylistTracks)
                .ToList();
        }

        public MediaItem? GetById(int id)
        {
            return _context.MediaItems
                .Include(m => m.Ratings)
                .Include(m => m.PlaylistTracks)
                .FirstOrDefault(m => m.Id == id);
        }

        public void Add(MediaItem media)
        {
            _context.MediaItems.Add(media);
            _context.SaveChanges();
        }

        public void Update(MediaItem media)
        {
            _context.MediaItems.Update(media);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var media = _context.MediaItems.Find(id);

            if (media != null)
            {
                _context.MediaItems.Remove(media);
                _context.SaveChanges();
            }
        }
    }
}