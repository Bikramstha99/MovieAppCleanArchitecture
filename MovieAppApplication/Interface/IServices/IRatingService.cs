using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppApplication.Interface.IServices
{
    public interface IRatingService
    {
        bool AddRating(Rating rating);
        bool UpdateRating(Rating rating);
        int GetRatingByUserIdAndMovieId(string UserId, int MovieId);
        double GetAverageRating(int MovieId);
        List<Rating> GetRatings(int MovieId);
    }
}
