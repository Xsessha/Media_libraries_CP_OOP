using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Repositories
{
    public interface IPlaylistRepository
    {
        IEnumerable<Playlist> GetAll();
        Playlist? GetById(int id);
        void Add(Playlist playlist);
        void Update(Playlist playlist);
        void Delete(int id);
        void RemoveTrackFromPlaylist(int playlistId, int mediaItemId);
    }
}