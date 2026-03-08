using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Repositories
{
    public interface IPlaybackRepository
    {
        IEnumerable<PlaybackHistory> GetAll();
        void Add(PlaybackHistory playback);
    }
}