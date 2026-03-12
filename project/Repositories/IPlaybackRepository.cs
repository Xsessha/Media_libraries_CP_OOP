using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Repositories
{
    public interface IPlaybackRepository
    {
        IEnumerable<PlaybackHistory> GetAll();
        IEnumerable<PlaybackHistory> GetByUser(int userId);
        void Add(PlaybackHistory playback);
    }
}