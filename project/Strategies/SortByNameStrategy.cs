using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Strategies
{
    public class SortByNameStrategy : ISortStrategy
    {
        public IEnumerable<MediaItem> Sort(IEnumerable<MediaItem> items)
        {
            return items.OrderBy(i => i.Title);
        }
    }
}