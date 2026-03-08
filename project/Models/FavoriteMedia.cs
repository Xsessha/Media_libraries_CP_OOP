namespace MediaLibraryWebApp.Models
{
    public class FavoriteMedia
    {
        public int UserId { get; set; }
        public int MediaItemId { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public MediaItem MediaItem { get; set; } = null!;
    }
}