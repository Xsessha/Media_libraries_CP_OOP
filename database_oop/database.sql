-- Створюємо базу даних
DROP DATABASE IF EXISTS MediaLibrary;
CREATE DATABASE MediaLibrary;
USE MediaLibrary;

-- Створюємо користувача для підключення з C#
ALTER USER 'mediauser'@'localhost' IDENTIFIED WITH mysql_native_password BY 'Xsenon24';
FLUSH PRIVILEGES;

-- Дамо повні права на базу MediaLibrary
GRANT ALL PRIVILEGES ON MediaLibrary.* TO 'mediauser'@'localhost';
FLUSH PRIVILEGES;

-- =============================
-- USERS
-- =============================
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(100) NOT NULL,
    Email VARCHAR(255),
    DateRegistered DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- =============================
-- MEDIA ITEMS
-- =============================
CREATE TABLE MediaItems (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Artist VARCHAR(255),
    Genre VARCHAR(100),
    Duration TIME,
    PlayCount INT DEFAULT 0,
    Filename VARCHAR(255) NOT NULL,
    DateAdded DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- =============================
-- PLAYLIST
-- =============================
CREATE TABLE Playlists (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT,
    Name VARCHAR(255) NOT NULL,
    DateCreated DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE
);

-- =============================
-- PLAYLIST TRACKS (many-to-many)
-- =============================
CREATE TABLE PlaylistTracks (
    PlaylistId INT,
    MediaItemId INT,
    Position INT,

    PRIMARY KEY (PlaylistId, Position),

    FOREIGN KEY (PlaylistId)
        REFERENCES Playlists(Id)
        ON DELETE CASCADE,

    FOREIGN KEY (MediaItemId)
        REFERENCES MediaItems(Id)
        ON DELETE CASCADE
);

-- =============================
-- RATINGS
-- =============================
CREATE TABLE Ratings (
    UserId INT,
    MediaItemId INT,
    RatingValue INT CHECK (RatingValue >= 1 AND RatingValue <= 5),

    PRIMARY KEY (UserId, MediaItemId),

    FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE,

    FOREIGN KEY (MediaItemId)
        REFERENCES MediaItems(Id)
        ON DELETE CASCADE
);

-- =============================
-- FAVORITES
-- =============================
CREATE TABLE FavoriteMedia (
    UserId INT,
    MediaItemId INT,

    PRIMARY KEY (UserId, MediaItemId),

    FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE,

    FOREIGN KEY (MediaItemId)
        REFERENCES MediaItems(Id)
        ON DELETE CASCADE
);

-- =============================
-- PLAYBACK HISTORY
-- =============================
CREATE TABLE PlaybackHistory (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT,
    MediaItemId INT NULL,
    PlaylistId INT NULL,
    DatePlayed DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (UserId)
        REFERENCES Users(Id)
        ON DELETE CASCADE,

    FOREIGN KEY (MediaItemId)
        REFERENCES MediaItems(Id)
        ON DELETE SET NULL,

    FOREIGN KEY (PlaylistId)
        REFERENCES Playlists(Id)
        ON DELETE SET NULL
);

-- =============================
-- TEST MEDIA
-- =============================
INSERT INTO MediaItems (Title, Artist, Genre, Duration, PlayCount, Filename)
VALUES
('Bassline Fever', 'Davico', 'EDM', '00:01:12', 0, 'song1.mp3'),
('Chillout Dreams', 'Massaro', 'Chillout', '00:03:04', 0, 'song2.mp3'),
('Fire & Ice', 'Ozzano', 'Rock', '00:03:24', 0, 'song3.mp3'),
('Heartbeat', 'Argonix', 'Pop', '00:02:27', 0, 'song4.mp3'),
('Jazz in the Rain', 'Mileson', 'Jazz', '00:02:49', 0, 'song5.mp3'),
('Moonlight Sonata', 'Luminari', 'Classical', '00:02:49', 0, 'song6.mp3'),
('Night Rider', 'Offnix', 'Punk Rock', '00:02:07', 0, 'song7.mp3'),
('Rock the Night', 'Zeron', 'Rock', '00:04:28', 0, 'song8.mp3'),
('Solar Winds', 'Anyra', 'Electronic', '00:03:48', 0, 'song9.mp3'),
('Urban Vibes', 'Arbyte', 'Electronic', '00:02:36', 0, 'song10.mp3');