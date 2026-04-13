using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;
using MediaLibraryWebApp.Strategies;
using System;
using System.Linq;

namespace MediaLibraryWebApp.Facades
{
    public class WeeklyPlaylistFacade
    {
        private readonly AppDbContext _context;
        private readonly IWeeklyPlaylistStrategy _strategy;

        public WeeklyPlaylistFacade(AppDbContext context, IWeeklyPlaylistStrategy strategy)
        {
            _context = context;
            _strategy = strategy;
        }

        public Playlist GenerateWeeklyPlaylist(int userId)
        {
            var history = _context.PlaybackHistories
                .Where(h => h.UserId == userId)
                .ToList();

            var topTrackIds = _strategy.GetTopTrackIds(history);

            var playlist = new Playlist
            {
                Name = $"Weekly Top {DateTime.Now:yyyy-MM-dd}",
                UserId = userId,
                DateCreated = DateTime.Now
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            int position = 1;

            foreach (var trackId in topTrackIds)
            {
                _context.PlaylistTracks.Add(new PlaylistTrack
                {
                    PlaylistId = playlist.Id,
                    MediaItemId = trackId,
                    Position = position++
                });
            }

            _context.SaveChanges();

            return playlist;
        }
    }
}