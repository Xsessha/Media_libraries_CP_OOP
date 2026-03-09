using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Factories
{
    public static class MediaFactory
    {
        public static MediaItem Create(
            string title,
            string artist,
            string genre,
            string filename,
            TimeSpan duration)
        {
            return new MediaItem
            {
                Title = title,
                Artist = artist,
                Genre = genre,
                Filename = filename,
                Duration = duration,
                DateAdded = DateTime.Now
            };
        }
    }
}