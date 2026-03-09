using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Strategies
{
    public interface ISortStrategy
    {
        IEnumerable<MediaItem> Sort(IEnumerable<MediaItem> items);
    }
}