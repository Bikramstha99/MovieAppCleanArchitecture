using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.ViewModel;
using MovieAppApplication.Interface.IServices;

namespace MovieAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieService _iMovieService;

        public MovieController(IMovieService iMovieService )
        {
            _iMovieService = iMovieService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var movies= _iMovieService.GetAllMovies();
            var movieViewModels = new List<MovieVM>();
            foreach (var movie in movies)
            {
                movieViewModels.Add(new MovieVM
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Description = movie.Description,
                    Director = movie.Director,
                    MoviePhoto = movie.MoviePhoto,
                    Genre = movie.Genre,
                });
            }
            return Ok(movieViewModels);
        }
        [HttpGet("{id}")]
        public IActionResult Detail(int id)
        {
            var movie = _iMovieService.GetByID(id);
            MovieVM movies = new MovieVM()
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Description = movie.Description,
                MoviePhoto = movie.MoviePhoto,
                Genre = movie.Genre,
            };
            return Ok(movies);
        }
    }
}
