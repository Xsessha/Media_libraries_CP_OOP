using MediaLibraryWebApp.Models;
using System.Collections.Generic;

namespace MediaLibraryWebApp.Strategies
{
    public interface IWeeklyPlaylistStrategy
    {
        List<int> GetTopTrackIds(List<PlaybackHistory> history);
    }
}