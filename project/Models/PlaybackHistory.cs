using System;

namespace MediaLibraryWebApp.Models
{
    public class PlaybackHistory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? MediaItemId { get; set; }

        public int? PlaylistId { get; set; }

        public DateTime DatePlayed { get; set; } = DateTime.Now;

        public User User { get; set; } = null!;

        public MediaItem? MediaItem { get; set; }

        public Playlist? Playlist { get; set; }
    }
}