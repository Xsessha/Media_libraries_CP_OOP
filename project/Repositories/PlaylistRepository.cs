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

       // НОВИЙ МЕТОД: Видалення треку без порушення ключів БД
       public void RemoveTrackFromPlaylist(int playlistId, int mediaItemId)
       {
         var playlist = _context.Playlists
        .Include(p => p.Tracks)
        .FirstOrDefault(p => p.Id == playlistId);

    if (playlist != null)
    {
        var track = playlist.Tracks.FirstOrDefault(t => t.MediaItemId == mediaItemId);
        if (track != null)
        {
            // Прямо кажемо базі даних видалити цей зв'язок
            _context.Remove(track); 

            // Відразу зберігаємо зміни. 
            // Ми більше НЕ намагаємося змінити Position в інших треків, 
            // щоб не сердити Entity Framework :)
            _context.SaveChanges();
        }
    }
}
    }
}