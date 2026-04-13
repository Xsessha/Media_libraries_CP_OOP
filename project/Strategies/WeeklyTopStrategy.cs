using MediaLibraryWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaLibraryWebApp.Strategies
{
    public class WeeklyTopStrategy : IWeeklyPlaylistStrategy
    {
        public List<int> GetTopTrackIds(List<PlaybackHistory> history)
        {
            var weekAgo = DateTime.Now.AddDays(-7);

            return history
                .Where(h => h.DatePlayed >= weekAgo && h.MediaItemId != null)
                .GroupBy(h => h.MediaItemId!.Value)
                .OrderByDescending(g => g.Count())
                .Take(20)
                .Select(g => g.Key)
                .ToList();
        }
    }
}