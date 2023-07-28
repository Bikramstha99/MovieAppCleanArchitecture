using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieAppApplication.Interface.IRepository;
using MovieAppDomain.Entities;
using MovieAppPresentation.ViewModel;

namespace MovieAppPresentation.Controllers
{
    public class RatingController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IRatingRepository _iRating;
        private readonly IMovieRepository _iMovieRepo;

        public RatingController
            (
                UserManager<IdentityUser> userManager,
                IRatingRepository ratingService,
                IMovieRepository iMovieRepo
            )
        {
            _userManager = userManager;

            _iRating = ratingService;
            _iMovieRepo = iMovieRepo;
        }
        [HttpPost]
        public IActionResult SubmitRating([Bind("MovieId,Ratings")] RatingVM ratingvm)
        {
            ratingvm.UserId = _userManager.GetUserId(User);
            int rate = _iRating.GetRatingByUserIdAndMovieId(ratingvm.UserId, ratingvm.MovieId);
            Rating ratings = new Rating()
            {
                RatingId = ratingvm.RatingId,
                Ratings = ratingvm.Ratings,
                MovieId = ratingvm.MovieId,
                UserId = ratingvm.UserId

            };
            if (rate == 0)
            {
                _iRating.AddRating(ratings);
            }
            else
            {
                _iRating.UpdateRating(ratings);
            }
            double averageRating = _iRating.GetAverageRating(ratings.MovieId);
            var movie = _iMovieRepo.GetByID(ratings.MovieId);
            if (movie != null)
            {
                movie.AverageRating = averageRating;
                _iMovieRepo.UpdateMovies(movie);
            }


            return RedirectToAction("Detail", "Movie", new { id = ratingvm.MovieId });

        }
    }
}

