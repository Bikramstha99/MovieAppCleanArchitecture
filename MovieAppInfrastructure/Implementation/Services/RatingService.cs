using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppInfrastructure.Implementation.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWOrk _iUnitOfWork;

        public RatingService(IUnitOfWOrk iUnitOfWork)
        {
            _iUnitOfWork = iUnitOfWork;
        }

        public bool AddRating(Rating rating)
        {
            _iUnitOfWork.RatingRepo.AddRating(rating);
            return true;
        }

        public double GetAverageRating(int MovieId)
        {
            var rate= _iUnitOfWork.RatingRepo.GetAverageRating(MovieId);
            return rate;
        }

        public int GetRatingByUserIdAndMovieId(string UserId, int MovieId)
        {
            var rating=_iUnitOfWork.RatingRepo.GetRatingByUserIdAndMovieId(UserId, MovieId);
            return rating;
        }

        public List<Rating> GetRatings(int MovieId)
        {
            var ratings=_iUnitOfWork.RatingRepo.GetRatings(MovieId);
            return ratings;
        }

        public bool UpdateRating(Rating rating)
        {
           _iUnitOfWork.RatingRepo.UpdateRating(rating);
            return true;
        }
    }
}
