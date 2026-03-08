namespace MediaLibraryWebApp.Models
{
    public class PlaylistTrack
    {
        public int PlaylistId { get; set; }
        public int MediaItemId { get; set; }
        public int Position { get; set; }

        // Navigation
        public Playlist Playlist { get; set; } = null!;
        public MediaItem MediaItem { get; set; } = null!;
    }
}