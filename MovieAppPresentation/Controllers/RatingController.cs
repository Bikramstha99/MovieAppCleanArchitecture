using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using MovieAppPresentation.ViewModel;

namespace MovieAppPresentation.Controllers
{
    public class RatingController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRatingService _iRatingService;
        private readonly IMovieService _iMovieService;

        public RatingController
            (
                UserManager<IdentityUser> userManager,
                IRatingService iRatingService,
                IMovieService iMovieService
            )
        {
            _userManager = userManager;
            _iRatingService = iRatingService;
            _iMovieService = iMovieService;
        }
        [HttpPost]
        public IActionResult SubmitRating([Bind("MovieId,Ratings")] RatingVM ratingvm)
        {
            ratingvm.UserId = _userManager.GetUserId(User);
            int rate = _iRatingService.GetRatingByUserIdAndMovieId(ratingvm.UserId, ratingvm.MovieId);
            Rating ratings = new Rating()
            {
                RatingId = ratingvm.RatingId,
                Ratings = ratingvm.Ratings,
                MovieId = ratingvm.MovieId,
                UserId = ratingvm.UserId

            };
            if (rate == 0)
            {
                _iRatingService.AddRating(ratings);
            }
            else
            {
                _iRatingService.UpdateRating(ratings);
            }
            double averageRating = _iRatingService.GetAverageRating(ratings.MovieId);
            var movie = _iMovieService.GetByID(ratings.MovieId);
            if (movie != null)
            {
                movie.AverageRating = averageRating;
                _iMovieService.UpdateMovies(movie);
            }


            return RedirectToAction("Detail", "Movie", new { id = ratingvm.MovieId });

        }
    }
}

