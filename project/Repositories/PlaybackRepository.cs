using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaLibraryWebApp.Repositories
{
    public class PlaybackRepository : IPlaybackRepository
    {
        private readonly AppDbContext _context;

        public PlaybackRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PlaybackHistory> GetAll()
        {
            return _context.PlaybackHistory
                .OrderByDescending(p => p.DatePlayed)
                .Include(p => p.MediaItem)
                .ToList();
        }

        public IEnumerable<PlaybackHistory> GetByUser(int userId)
        {
            return _context.PlaybackHistory
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.DatePlayed)
                .Include(p => p.MediaItem)
                .ToList();
        }

        public void Add(PlaybackHistory playback)
        {
            _context.PlaybackHistory.Add(playback);
            _context.SaveChanges();
        }
    }
}