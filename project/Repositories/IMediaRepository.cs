using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Repositories
{
    public interface IMediaRepository
    {
        IEnumerable<MediaItem> GetAll();
        MediaItem? GetById(int id);
        void Add(MediaItem media);
        void Update(MediaItem media);
        void Delete(int id);
    }
}