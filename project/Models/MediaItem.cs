using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MediaLibraryWebApp.Models
{
    public class MediaItem
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Artist { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Genre { get; set; } = string.Empty;

        public TimeSpan Duration { get; set; } = TimeSpan.Zero;

        public int PlayCount { get; set; } = 0;

        [Required, MaxLength(255)]
        public string Filename { get; set; } = string.Empty;

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<PlaybackHistory> PlaybackHistory { get; set; } = new List<PlaybackHistory>();
        public ICollection<FavoriteMedia> FavoriteMedia { get; set; } = new List<FavoriteMedia>();

        public double AverageRating =>
            Ratings.Count == 0 ? 0 : Ratings.Average(r => r.RatingValue);
    }
}