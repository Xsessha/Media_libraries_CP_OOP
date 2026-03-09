using System.Text.Json;
using MediaLibraryWebApp.Models;
using MediaLibraryWebApp.Iterators;

namespace MediaLibraryWebApp.Facade
{
    public class MediaLibraryFacade
    {
        public string ExportPlaylistToJson(Playlist playlist)
        {
            var data = playlist.Tracks
                .OrderBy(t => t.Position)
                .Select(t => new
                {
                    Title = t.MediaItem.Title,
                    Artist = t.MediaItem.Artist,
                    Genre = t.MediaItem.Genre
                });

            return JsonSerializer.Serialize(data,
                new JsonSerializerOptions { WriteIndented = true });
        }

        public void PlayPlaylist(Playlist playlist)
        {
            var iterator = new PlaylistIterator(playlist);

            while (iterator.HasNext())
            {
                var track = iterator.Next();
                track.PlayCount++;
            }
        }
    }
}