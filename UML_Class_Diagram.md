---
config:
  theme: mc
  look: neo
---
classDiagram
direction TB
	namespace IteratorPattern {
        class IPlaylistIterator {
	        +HasNext()
	        +Next()
	        +HasPrevious()
	        +Previous()
        }

        class PlaylistIterator {
	        -int position
	        -Playlist playlist
        }

	}
	namespace StrategyPattern {
        class ISortingStrategy {
	        +Sort(items)
        }

        class SortByRating {
        }

        class SortByDate {
        }

        class SortByTitle {
        }

	}
	namespace RepositoryPattern {
        class IMediaRepository {
        }

        class MediaRepository {
        }

        class IPlaylistRepository {
        }

        class PlaylistRepository {
        }

	}
	namespace FactoryPattern {
        class IExportService {
        }

        class JsonExportService {
        }

        class XmlExportService {
        }

        class ExportFactory {
	        +CreateExport(type)
        }

	}
	namespace ServiceLayer {
        class HistoryManager {
        }

        class ReportManager {
        }

        class MediaLibrary {
        }

	}
    class MediaItem {
	    -int id
	    -string title
	    -string genre
	    -Time duration
	    -float rating
	    -Date dateAdded
	    +GetInfo() string
    }

    class Playlist {
	    -string name
	    -List~MediaItem~ items
	    +AddItem(item: MediaItem)
	    +RemoveItem(item: MediaItem)
	    +GetItems()
	    +Sort(strategy: ISortingStrategy)
	    +CreateIterator()
    }

    class Database {
    }

	<<interface>> IPlaylistIterator
	<<interface>> ISortingStrategy
	<<interface>> IMediaRepository
	<<interface>> IPlaylistRepository
	<<interface>> IExportService
	<<Facade>> MediaLibrary

    Playlist o-- MediaItem
    Playlist --> IPlaylistIterator
    PlaylistIterator ..|> IPlaylistIterator
    Playlist ..> ISortingStrategy
    SortByRating ..|> ISortingStrategy
    SortByDate ..|> ISortingStrategy
    SortByTitle ..|> ISortingStrategy
    MediaRepository ..|> IMediaRepository
    PlaylistRepository ..|> IPlaylistRepository
    MediaRepository --> Database
    PlaylistRepository --> Database
    MediaLibrary --> IMediaRepository
    MediaLibrary --> IPlaylistRepository
    MediaLibrary --> HistoryManager
    MediaLibrary --> ReportManager
    MediaLibrary --> ExportFactory
    ExportFactory --> IExportService
    ExportFactory --> JsonExportService
    ExportFactory --> XmlExportService
    JsonExportService ..|> IExportService
    XmlExportService ..|> IExportService

	class IPlaylistIterator:::iterator
	class PlaylistIterator:::iterator
	class ISortingStrategy:::strategy
	class SortByRating:::strategy
	class SortByDate:::strategy
	class SortByTitle:::strategy
	class IMediaRepository:::repository
	class MediaRepository:::repository
	class IPlaylistRepository:::repository
	class PlaylistRepository:::repository
	class IExportService:::factory
	class JsonExportService:::factory
	class XmlExportService:::factory
	class ExportFactory:::factory
	class HistoryManager:::service
	class ReportManager:::service
	class MediaLibrary:::service

	classDef iterator :,fill:#D6EAF8,stroke:#2E86C1,fill:#D6EAF8,stroke:#2E86C1
	classDef strategy :,fill:#D5F5E3,stroke:#239B56
	classDef repository :,fill:#FAD7A0,stroke:#CA6F1E
	classDef factory :,fill:#E8DAEF,stroke:#7D3C98
	classDef service :,fill:#E5E7E9,stroke:#566573
