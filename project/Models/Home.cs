namespace MediaLibraryWebApp.Models
{
    public class HomeViewModel
    {

        public List<MediaItem> TopRatedTracks { get; set; }


        public List<MediaItem> RecentlyAdded { get; set; }

  
        public List<MediaItem> MostPlayedTracks { get; set; }

        public List<string> Genres { get; set; } 
        public int TotalPlaysCount { get; set; }  
        public int TotalTracksCount { get; set; } 
    }
}