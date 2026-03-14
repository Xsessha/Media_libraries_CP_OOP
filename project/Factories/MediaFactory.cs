using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Factories
{
    public static class MediaFactory //створити фекторі для створення плейлистів та в ідеалі створити стрателді + фектор для автоматичних плейлистів з найпрослуховуванішими треками за тиждень
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