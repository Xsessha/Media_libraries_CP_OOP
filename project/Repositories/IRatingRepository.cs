using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Repositories
{
    public interface IRatingRepository
    {
        IEnumerable<Rating> GetAll();
        Rating? GetById(int id);
        void Add(Rating rating);
        void Delete(int id);
    }
}