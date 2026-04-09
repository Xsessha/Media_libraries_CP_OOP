using Xunit;
using System;
using System.Collections.Generic;
using MediaLibraryWebApp.Models;

namespace MediaLibrary.Tests
{
    public class MediaLibraryTests
    {
        // MEDIA ITEM TESTS

        [Fact]
        public void MediaItem_Title_ShouldStoreCorrectly()
        {
            var media = new MediaItem { Title = "Song" };
            Assert.Equal("Song", media.Title);
        }

        [Fact]
        public void MediaItem_Artist_ShouldStoreCorrectly()
        {
            var media = new MediaItem { Artist = "ArtistName" };
            Assert.Equal("ArtistName", media.Artist);
        }

        [Fact]
        public void MediaItem_Genre_ShouldStoreCorrectly()
        {
            var media = new MediaItem { Genre = "Rock" };
            Assert.Equal("Rock", media.Genre);
        }

        [Fact]
        public void MediaItem_Duration_ShouldStoreCorrectly()
        {
            var media = new MediaItem { Duration = TimeSpan.FromSeconds(180) };
            Assert.Equal(TimeSpan.FromSeconds(180), media.Duration);
        }

        [Fact]
        public void MediaItem_Filename_ShouldStoreCorrectly()
        {
            var media = new MediaItem { Filename = "song.mp3" };
            Assert.Equal("song.mp3", media.Filename);
        }

        [Fact]
        public void MediaItem_PlayCount_ShouldIncrease()
        {
            var media = new MediaItem { PlayCount = 0 };
            media.PlayCount++;
            Assert.Equal(1, media.PlayCount);
        }

        [Fact]
        public void MediaItem_AverageRating_ShouldCalculateCorrectly()
        {
            var media = new MediaItem
            {
                Ratings = new List<Rating>
                {
                    new Rating { RatingValue = 5 },
                    new Rating { RatingValue = 3 }
                }
            };

            Assert.Equal(4, media.AverageRating);
        }

        [Fact]
        public void MediaItem_RatingsCollection_ShouldContainItems()
        {
            var media = new MediaItem();
            media.Ratings.Add(new Rating { RatingValue = 4 });

            Assert.Single(media.Ratings);
        }

        [Fact]
        public void MediaItem_AddedDate_ShouldBeSet()
        {
            var media = new MediaItem();
            Assert.True(media.DateAdded <= DateTime.Now);
        }

        [Fact]
        public void MediaItem_DefaultPlayCount_ShouldBeZero()
        {
            var media = new MediaItem();
            Assert.Equal(0, media.PlayCount);
        }

        // USER TESTS

        [Fact]
        public void User_Username_ShouldStoreCorrectly()
        {
            var user = new User { Username = "TestUser" };
            Assert.Equal("TestUser", user.Username);
        }

        [Fact]
        public void User_Email_ShouldStoreCorrectly()
        {
            var user = new User { Email = "test@mail.com" };
            Assert.Equal("test@mail.com", user.Email);
        }

        [Fact]
        public void User_DateRegistered_ShouldBeSet()
        {
            var user = new User();
            Assert.True(user.DateRegistered <= DateTime.Now);
        }

        [Fact]
        public void User_PlaylistsCollection_ShouldAllowAdding()
        {
            var user = new User();
            user.Playlists.Add(new Playlist { Name = "Playlist1" });

            Assert.Single(user.Playlists);
        }

        [Fact]
        public void User_FavoriteMediaCollection_ShouldAllowAdding()
        {
            var user = new User();
            user.FavoriteMedia.Add(new FavoriteMedia());

            Assert.Single(user.FavoriteMedia);
        }

        // PLAYLIST TESTS

        [Fact]
        public void Playlist_Name_ShouldStoreCorrectly()
        {
            var playlist = new Playlist { Name = "MyPlaylist" };
            Assert.Equal("MyPlaylist", playlist.Name);
        }

        [Fact]
        public void Playlist_DateCreated_ShouldBeSet()
        {
            var playlist = new Playlist();
            Assert.True(playlist.DateCreated <= DateTime.Now);
        }

        [Fact]
        public void Playlist_TracksCollection_ShouldAllowAdding()
        {
            var playlist = new Playlist();
            playlist.Tracks.Add(new PlaylistTrack());

            Assert.Single(playlist.Tracks);
        }

        [Fact]
        public void PlaylistTrack_ShouldReferenceMediaItem()
        {
            var media = new MediaItem { Title = "Song" };
            var track = new PlaylistTrack { MediaItem = media };

            Assert.Equal("Song", track.MediaItem.Title);
        }

        [Fact]
        public void PlaylistTrack_Position_ShouldStoreCorrectly()
        {
            var track = new PlaylistTrack { Position = 1 };
            Assert.Equal(1, track.Position);
        }

        // RATING TESTS

        [Fact]
        public void Rating_Value_ShouldStoreCorrectly()
        {
            var rating = new Rating { RatingValue = 5 };
            Assert.Equal(5, rating.RatingValue);
        }

        [Fact]
        public void Rating_ShouldReferenceUser()
        {
            var user = new User { Username = "User1" };
            var rating = new Rating { User = user };

            Assert.Equal("User1", rating.User.Username);
        }

        [Fact]
        public void Rating_ShouldReferenceMediaItem()
        {
            var media = new MediaItem { Title = "Song" };
            var rating = new Rating { MediaItem = media };

            Assert.Equal("Song", rating.MediaItem.Title);
        }

        [Fact]
        public void Rating_Value_ShouldBeWithinRange()
        {
            var rating = new Rating { RatingValue = 4 };

            Assert.InRange(rating.RatingValue, 1, 5);
        }

        [Fact]
        public void Rating_List_ShouldAllowMultipleRatings()
        {
            var media = new MediaItem();
            media.Ratings.Add(new Rating { RatingValue = 5 });
            media.Ratings.Add(new Rating { RatingValue = 4 });

            Assert.Equal(2, media.Ratings.Count);
        }

        // FAVORITES TESTS

        [Fact]
        public void FavoriteMedia_ShouldReferenceUser()
        {
            var user = new User { Username = "User1" };
            var fav = new FavoriteMedia { User = user };

            Assert.Equal("User1", fav.User.Username);
        }

        [Fact]
        public void FavoriteMedia_ShouldReferenceMediaItem()
        {
            var media = new MediaItem { Title = "Song" };
            var fav = new FavoriteMedia { MediaItem = media };

            Assert.Equal("Song", fav.MediaItem.Title);
        }

        [Fact]
        public void FavoriteMedia_List_ShouldAllowAdding()
        {
            var user = new User();
            user.FavoriteMedia.Add(new FavoriteMedia());

            Assert.Single(user.FavoriteMedia);
        }

        [Fact]
        public void FavoriteMedia_ShouldCreateObject()
       {
           var fav = new FavoriteMedia();
           Assert.NotNull(fav);
      }

        [Fact]
        public void FavoriteMedia_MultipleFavorites_ShouldWork()
        {
            var user = new User();
            user.FavoriteMedia.Add(new FavoriteMedia());
            user.FavoriteMedia.Add(new FavoriteMedia());

            Assert.Equal(2, user.FavoriteMedia.Count);
        }

        // PLAYBACK HISTORY TESTS

        [Fact]
        public void PlaybackHistory_ShouldReferenceUser()
        {
            var user = new User { Username = "User1" };
            var history = new PlaybackHistory { User = user };

            Assert.Equal("User1", history.User.Username);
        }

        [Fact]
        public void PlaybackHistory_ShouldReferenceMediaItem()
        {
            var media = new MediaItem { Title = "Song" };
            var history = new PlaybackHistory { MediaItem = media };

            Assert.Equal("Song", history.MediaItem.Title);
        }

        [Fact]
        public void PlaybackHistory_DatePlayed_ShouldBeSet()
        {
            var history = new PlaybackHistory();
            Assert.True(history.DatePlayed <= DateTime.Now);
        }

        [Fact]
        public void PlaybackHistory_List_ShouldAllowAdding()
        {
            var user = new User();
            user.PlaybackHistory.Add(new PlaybackHistory());

            Assert.Single(user.PlaybackHistory);
        }

        [Fact]
        public void PlaybackHistory_MultipleEntries_ShouldWork()
        {
            var user = new User();
            user.PlaybackHistory.Add(new PlaybackHistory());
            user.PlaybackHistory.Add(new PlaybackHistory());

            Assert.Equal(2, user.PlaybackHistory.Count);
        }

        // INTEGRATION TEST

        [Fact]
        public void SystemIntegration_BasicFlow()
        {
            var user = new User { Username = "User1" };
            var media = new MediaItem { Title = "Song" };

            var playlist = new Playlist { Name = "MyPlaylist" };
            playlist.Tracks.Add(new PlaylistTrack { MediaItem = media });

            user.Playlists.Add(playlist);

            Assert.Single(user.Playlists);
            Assert.Single(playlist.Tracks);
        }
    }
}