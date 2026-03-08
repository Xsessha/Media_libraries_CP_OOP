using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaLibraryWebApp.Models
{
    public class Playlist
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Navigation
        public User User { get; set; } = null!;
        public ICollection<PlaylistTrack> Tracks { get; set; } = new List<PlaylistTrack>();
    }
}