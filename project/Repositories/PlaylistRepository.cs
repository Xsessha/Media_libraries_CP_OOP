using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaLibraryWebApp.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _context;

        public PlaylistRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Playlist> GetAll()
        {
            return _context.Playlists
                .Include(p => p.Tracks)
                .ToList();
        }

        public Playlist? GetById(int id)
        {
            return _context.Playlists
                .Include(p => p.Tracks)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Add(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            _context.SaveChanges();
        }

        public void Update(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var playlist = _context.Playlists.Find(id);

            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                _context.SaveChanges();
            }
        }
    }
}