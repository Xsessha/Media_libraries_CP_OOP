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
        public DbSet<PlaybackHistory> PlaybackHistories { get; set; } = null!;
        public DbSet<FavoriteMedia> FavoriteMedia { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // ВИПРАВЛЕННЯ: Вказуємо правильну назву таблиці
            // ==========================================
            modelBuilder.Entity<PlaybackHistory>().ToTable("PlaybackHistory");

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

            modelBuilder.Entity<PlaybackHistory>()
                .HasOne(h => h.User)
                .WithMany(u => u.PlaybackHistory)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaybackHistory>()
                .HasOne(h => h.MediaItem)
                .WithMany(m => m.PlaybackHistory)
                .HasForeignKey(h => h.MediaItemId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PlaybackHistory>()
                .HasOne(h => h.Playlist)
                .WithMany()
                .HasForeignKey(h => h.PlaylistId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}