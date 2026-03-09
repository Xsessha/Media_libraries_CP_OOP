using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Iterators
{
    public interface IPlaylistIterator
    {
        bool HasNext();
        MediaItem Next();
    }
}