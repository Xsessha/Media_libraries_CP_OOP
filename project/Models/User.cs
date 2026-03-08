using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaLibraryWebApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        public DateTime DateRegistered { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<PlaybackHistory> PlaybackHistory { get; set; } = new List<PlaybackHistory>();
        public ICollection<FavoriteMedia> FavoriteMedia { get; set; } = new List<FavoriteMedia>();
    }
}