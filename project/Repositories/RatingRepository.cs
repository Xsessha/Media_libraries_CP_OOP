using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;

namespace MediaLibraryWebApp.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Rating> GetAll()
        {
            return _context.Ratings.ToList();
        }

        public Rating? GetById(int id)
        {
            return _context.Ratings.Find(id);
        }

        public void Add(Rating rating)
        {
            _context.Ratings.Add(rating);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var rating = _context.Ratings.Find(id);

            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                _context.SaveChanges();
            }
        }
    }
}