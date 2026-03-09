using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;
using Microsoft.EntityFrameworkCore;

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

        public Rating? Get(int userId, int mediaItemId)
        {
            return _context.Ratings
                .FirstOrDefault(r => r.UserId == userId && r.MediaItemId == mediaItemId);
        }

        public void Add(Rating rating)
        {
            _context.Ratings.Add(rating);
            _context.SaveChanges();
        }

        public void Delete(int userId, int mediaItemId)
        {
            var rating = _context.Ratings
                .FirstOrDefault(r => r.UserId == userId && r.MediaItemId == mediaItemId);

            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                _context.SaveChanges();
            }
        }
    }
}