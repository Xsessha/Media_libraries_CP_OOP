DROP DATABASE IF EXISTS MediaLibrary;
CREATE DATABASE MediaLibrary;
USE MediaLibrary;

CREATE TABLE Users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(100) NOT NULL,
    email VARCHAR(255),
    date_registered DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE MediaItem (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    artist VARCHAR(255),
    genre VARCHAR(100),
    duration TIME,
    play_count INT DEFAULT 0,
    filename VARCHAR(255),
    date_added DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Playlist (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    name VARCHAR(255) NOT NULL,
    date_created DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
);

CREATE TABLE PlaylistItems (
    playlist_id INT,
    mediaitem_id INT,
    position INT,
    PRIMARY KEY (playlist_id, position),
    FOREIGN KEY (playlist_id) REFERENCES Playlist(id) ON DELETE CASCADE,
    FOREIGN KEY (mediaitem_id) REFERENCES MediaItem(id) ON DELETE CASCADE
);

CREATE TABLE History (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    mediaitem_id INT NULL,
    playlist_id INT NULL,
    date_played DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE,
    FOREIGN KEY (mediaitem_id) REFERENCES MediaItem(id) ON DELETE SET NULL,
    FOREIGN KEY (playlist_id) REFERENCES Playlist(id) ON DELETE SET NULL
);

CREATE TABLE FavoriteMedia (
    user_id INT,
    mediaitem_id INT,
    PRIMARY KEY (user_id, mediaitem_id),
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE,
    FOREIGN KEY (mediaitem_id) REFERENCES MediaItem(id) ON DELETE CASCADE
);

CREATE TABLE MediaRating (
    user_id INT,
    mediaitem_id INT,
    rating INT CHECK (rating >= 1 AND rating <= 5),
    PRIMARY KEY (user_id, mediaitem_id),
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE,
    FOREIGN KEY (mediaitem_id) REFERENCES MediaItem(id) ON DELETE CASCADE
);


INSERT INTO MediaItem (title, artist, genre, duration, play_count, filename)
VALUES
('Bassline Fever', 'Davico', 'EDM', '00:01:12', 0, '1.mp3'),
('Chillout Dreams', 'Massaro', 'Chillout', '00:03:04', 0, '2.mp3'),
('Fire & Ice', 'Ozzano', 'Rock', '00:03:24', 0, '3.mp3'),
('Heartbeat', 'Argonix', 'Pop', '00:02:27', 0, '4.mp3'),
('Jazz in the Rain', 'Mileson', 'Jazz', '00:02:49', 0, '5.mp3'),
('Moonlight Sonata', 'Luminari', 'Classical', '00:02:49', 0, '6.mp3'),
('Night Rider', 'Offnix', 'Punk Rock', '00:02:07', 0, '7.mp3'),
('Rock the Night', 'Zeron', 'Rock', '00:04:28', 0, '8.mp3'),
('Solar Winds', 'Anyra', 'Electronic', '00:03:48', 0, '9.mp3'),
('Urban Vibes', 'Arbyte', 'Electronic', '00:02:36', 0, '10.mp3');