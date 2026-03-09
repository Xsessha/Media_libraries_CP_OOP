using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Strategies
{
    public class SortByDateStrategy : ISortStrategy
    {
        public IEnumerable<MediaItem> Sort(IEnumerable<MediaItem> items)
        {
            return items.OrderByDescending(i => i.DateAdded);
        }
    }
}