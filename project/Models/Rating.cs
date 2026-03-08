using System.ComponentModel.DataAnnotations;

namespace MediaLibraryWebApp.Models
{
    public class Rating
    {
        public int UserId { get; set; }
        public int MediaItemId { get; set; }

        [Range(1,5)]
        public int RatingValue { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public MediaItem MediaItem { get; set; } = null!;
    }
}