classDiagram
direction TB
	namespace IteratorPattern {
        class IPlaylistIterator {
	        +HasNext() bool
	        +Next() MediaItem
	        +HasPrevious() bool
	        +Previous() MediaItem
        }

        class PlaylistIterator {
	        -int position
	        -Playlist playlist
	        +PlaylistIterator(playlist: Playlist)
	        +HasNext() bool
	        +Next() MediaItem
	        +HasPrevious() bool
	        +Previous() MediaItem
        }

	}
	namespace StrategyPattern {
        class ISortingStrategy {
	        +Sort(items: List~MediaItem~) List~MediaItem~
        }

        class SortByRating {
	        +Sort(items: List~MediaItem~) List~MediaItem~
        }

        class SortByDate {
	        +Sort(items: List~MediaItem~) List~MediaItem~
        }

        class SortByTitle {
	        +Sort(items: List~MediaItem~) List~MediaItem~
        }

	}
	namespace RepositoryPattern {
        class IMediaRepository {
	        +GetAll() List~MediaItem~
	        +GetById(id:int) MediaItem
	        +Add(item:MediaItem)
	        +Update(item:MediaItem)
	        +Delete(id:int)
        }

        class MediaRepository {
	        -Database db
	        +GetAll() List~MediaItem~
	        +GetById(id:int) MediaItem
	        +Add(item:MediaItem)
	        +Update(item:MediaItem)
	        +Delete(id:int)
        }

        class IPlaylistRepository {
	        +GetAll() List~Playlist~
	        +GetById(id:int) Playlist
	        +Add(playlist:Playlist)
	        +Delete(id:int)
	        +AddItem(playlistId:int, mediaId:int)
        }

        class PlaylistRepository {
	        -Database db
	        +GetAll() List~Playlist~
	        +GetById(id:int) Playlist
	        +Add(playlist:Playlist)
	        +Delete(id:int)
	        +AddItem(playlistId:int, mediaId:int)
        }

	}
	namespace FactoryPattern {
        class IExportService {
	        +Export(data:string, path:string)
        }

        class JsonExportService {
	        +Export(data:string, path:string)
        }

        class XmlExportService {
	        +Export(data:string, path:string)
        }

        class ExportFactory {
	        +CreateExport(type:string) IExportService
        }

	}
	namespace ServiceLayer {
        class HistoryManager {
	        +AddHistory(mediaId:int)
	        +GetHistory() List~MediaItem~
	        +ClearHistory()
        }

        class ReportManager {
	        +GenerateTopPlayed() List~MediaItem~
	        +GenerateTopRated() List~MediaItem~
	        +ExportReport(type:string)
        }

        class MediaLibrary {
	        -IMediaRepository mediaRepo
	        -IPlaylistRepository playlistRepo
	        -HistoryManager historyManager
	        -ReportManager reportManager
	        +PlayMedia(mediaId:int)
	        +CreatePlaylist(name:string)
	        +AddToPlaylist(playlistId:int, mediaId:int)
	        +SortPlaylist(playlistId:int, strategy:ISortingStrategy)
        }

	}
    class MediaItem {
	    -int id
	    -string title
	    -string genre
	    -Time duration
	    -float rating
	    -Date dateAdded
	    -int playCount
	    +GetInfo() string
    }

    class Playlist {
	    -int id
	    -string name
	    -Date createdDate
	    -List~MediaItem~ items
	    +AddItem(item:MediaItem)
	    +RemoveItem(item:MediaItem)
	    +GetItems() List~MediaItem~
	    +Sort(strategy:ISortingStrategy)
	    +CreateIterator() IPlaylistIterator
    }

    class Database {
	    +ConnectionString
	    +ExecuteQuery(query)
	    +ExecuteNonQuery(query)
    }

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

	class IPlaylistIterator:::Aqua
	class PlaylistIterator:::Aqua
	class ISortingStrategy:::Sky
	class SortByRating:::Sky
	class SortByDate:::Sky
	class SortByTitle:::Sky
	class IMediaRepository:::Rose
	class MediaRepository:::Rose
	class IPlaylistRepository:::Rose
	class PlaylistRepository:::Rose
	class IExportService:::Peach
	class JsonExportService:::Peach
	class XmlExportService:::Peach
	class ExportFactory:::Peach
	class HistoryManager:::Ash
	class ReportManager:::Ash
	class MediaLibrary:::Ash
	class MediaItem:::Pine
	class Playlist:::Pine
	class Database:::Pine

	classDef Aqua :,stroke-width:1px,stroke-dasharray:none,stroke:#46EDC8,fill:#DEFFF8,color:#378E7A,stroke-width:1px,stroke-dasharray:none,stroke:#46EDC8,fill:#DEFFF8,color:#378E7A
	classDef Sky :,stroke-width:1px,stroke-dasharray:none,stroke:#374D7C,fill:#E2EBFF,color:#374D7C,stroke-width:1px,stroke-dasharray:none,stroke:#374D7C,fill:#E2EBFF,color:#374D7C,stroke-width:1px,stroke-dasharray:none,stroke:#374D7C,fill:#E2EBFF,color:#374D7C,stroke-width:1px,stroke-dasharray:none,stroke:#374D7C,fill:#E2EBFF,color:#374D7C
	classDef Rose :,stroke-width:1px,stroke-dasharray:none,stroke:#FF5978,fill:#FFDFE5,color:#8E2236,stroke-width:1px,stroke-dasharray:none,stroke:#FF5978,fill:#FFDFE5,color:#8E2236,stroke-width:1px,stroke-dasharray:none,stroke:#FF5978,fill:#FFDFE5,color:#8E2236,stroke-width:1px,stroke-dasharray:none,stroke:#FF5978,fill:#FFDFE5,color:#8E2236
	classDef Peach :,stroke-width:1px,stroke-dasharray:none,stroke:#FBB35A,fill:#FFEFDB,color:#8F632D,stroke-width:1px,stroke-dasharray:none,stroke:#FBB35A,fill:#FFEFDB,color:#8F632D,stroke-width:1px,stroke-dasharray:none,stroke:#FBB35A,fill:#FFEFDB,color:#8F632D,stroke-width:1px,stroke-dasharray:none,stroke:#FBB35A,fill:#FFEFDB,color:#8F632D
	classDef Pine :,stroke-width:1px,stroke-dasharray:none,stroke:#254336,fill:#27654A,color:#FFFFFF,stroke-width:1px,stroke-dasharray:none,stroke:#254336,fill:#27654A,color:#FFFFFF,stroke-width:1px,stroke-dasharray:none,stroke:#254336,fill:#27654A,color:#FFFFFF,stroke-width:1px,stroke-dasharray:none,stroke:#254336,fill:#27654A,color:#FFFFFF
	classDef Ash :,stroke-width:1px,stroke-dasharray:none,stroke:#999999,fill:#EEEEEE,color:#000000,stroke-width:1px,stroke-dasharray:none,stroke:#999999,fill:#EEEEEE,color:#000000,stroke-width:1px,stroke-dasharray:none,stroke:#999999,fill:#EEEEEE,color:#000000
