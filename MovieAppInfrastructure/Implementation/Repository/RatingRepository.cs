using Microsoft.EntityFrameworkCore;
using MovieAppApplication.Interface.IRepository;
using MovieAppDomain.Entities;
using MovieAppInfrastructure.Persistance;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppInfrastructure.Implementation.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public RatingRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public bool AddRating(Rating rating)
        {
            _movieDbContext.Ratings.Add(rating);
            _movieDbContext.SaveChanges();
            return true;
        }

        public double GetAverageRating(int MovieId)
        {
            double averageRating = _movieDbContext.Ratings.Where(r => r.MovieId == MovieId).Average(r => r.Ratings);

            return averageRating;
        }

        public int GetRatingByUserIdAndMovieId(string UserId, int MovieId)
        {
            Rating rating = new Rating();
            rating = _movieDbContext.Ratings.FirstOrDefault(r => r.UserId == UserId && r.MovieId == MovieId);
            if (rating != null)
            {
                return rating.Ratings;
            }
            else
            {
                return 0;
            }
        }

        public List<Rating> GetRatings(int MovieId)
        {
            var data = _movieDbContext.Ratings.Include(e => e.IdentityUser).Where(c => c.MovieId == MovieId).ToList();
            return data;
        }

        public bool UpdateRating(Rating rating)
        {
            var ratings = _movieDbContext.Ratings.FirstOrDefault(x => x.UserId == rating.UserId && x.MovieId == rating.MovieId);
            ratings.Ratings = rating.Ratings;
            _movieDbContext.Ratings.Update(rating);
            _movieDbContext.SaveChanges();
            return true;
        }
    }
}
