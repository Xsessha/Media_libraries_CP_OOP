using Microsoft.EntityFrameworkCore;
using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<MediaItem> MediaItems { get; set; } = null!;
        public DbSet<Playlist> Playlists { get; set; } = null!;
        public DbSet<PlaylistTrack> PlaylistTracks { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<PlaybackHistory> PlaybackHistory { get; set; } = null!;
        public DbSet<FavoriteMedia> FavoriteMedia { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlaylistTrack>()
                .HasKey(pt => new { pt.PlaylistId, pt.Position });

            modelBuilder.Entity<Rating>()
                .HasKey(r => new { r.UserId, r.MediaItemId });

            modelBuilder.Entity<FavoriteMedia>()
                .HasKey(f => new { f.UserId, f.MediaItemId });

            modelBuilder.Entity<PlaylistTrack>()
                .HasOne(pt => pt.Playlist)
                .WithMany(p => p.Tracks)
                .HasForeignKey(pt => pt.PlaylistId);

            modelBuilder.Entity<PlaylistTrack>()
                .HasOne(pt => pt.MediaItem)
                .WithMany(m => m.PlaylistTracks)
                .HasForeignKey(pt => pt.MediaItemId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.MediaItem)
                .WithMany(m => m.Ratings)
                .HasForeignKey(r => r.MediaItemId);

            modelBuilder.Entity<FavoriteMedia>()
                .HasOne(f => f.User)
                .WithMany(u => u.FavoriteMedia)
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<FavoriteMedia>()
                .HasOne(f => f.MediaItem)
                .WithMany(m => m.FavoriteMedia)
                .HasForeignKey(f => f.MediaItemId);

            modelBuilder.Entity<PlaybackHistory>()
                .HasOne(h => h.User)
                .WithMany(u => u.PlaybackHistory)
                .HasForeignKey(h => h.UserId);
        }
    }
}