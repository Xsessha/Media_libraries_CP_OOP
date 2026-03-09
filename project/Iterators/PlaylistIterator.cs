using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Iterators
{
    public class PlaylistIterator : IPlaylistIterator
    {
        private readonly List<MediaItem> _tracks;
        private int _position = 0;

        public PlaylistIterator(Playlist playlist)
        {
            _tracks = playlist.Tracks
                .OrderBy(t => t.Position)
                .Select(t => t.MediaItem)
                .ToList();
        }

        public bool HasNext()
        {
            return _position < _tracks.Count;
        }

        public MediaItem Next()
        {
            return _tracks[_position++];
        }
    }
}