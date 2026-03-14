using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Repositories
{
    public interface IRatingRepository
    {
        IEnumerable<Rating> GetAll();
        Rating? Get(int userId, int mediaItemId);
        void Add(Rating rating);
        void Update(Rating rating);
        void Delete(int userId, int mediaItemId);
    }
}