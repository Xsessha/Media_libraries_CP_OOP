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
            return _context.PlaybackHistories
                .Include(p => p.MediaItem)
                .Include(p => p.User)
                .OrderByDescending(p => p.DatePlayed)
                .ToList();
        }

        public IEnumerable<PlaybackHistory> GetByUser(int userId)
        {
            return _context.PlaybackHistories
                .Include(p => p.MediaItem)
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.DatePlayed)
                .ToList();
        }

        public void Add(PlaybackHistory playback)
        {
            _context.PlaybackHistories.Add(playback);
            _context.SaveChanges();
        }
    }
}