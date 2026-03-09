using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;

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
                .ToList();
        }

        public void Add(PlaybackHistory playback)
        {
            _context.PlaybackHistory.Add(playback);
            _context.SaveChanges();
        }
    }
}