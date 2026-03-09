using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Strategies
{
    public class SortByRatingStrategy : ISortStrategy
    {
        public IEnumerable<MediaItem> Sort(IEnumerable<MediaItem> items)
        {
            return items.OrderByDescending(i => i.AverageRating);
        }
    }
}