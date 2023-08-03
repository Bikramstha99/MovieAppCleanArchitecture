using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.ViewModel;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using Nest;
using System.Data;

namespace MovieAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        private readonly IMovieService _iMovieService;
     

        public MovieController(IMovieService iMovieService)
        {
            _iMovieService = iMovieService;
            
        }


        [Authorize(Roles="Admin")]
        [HttpGet]

        public IActionResult Index()
        {

            var movies = _iMovieService.GetAllMovies();
            var movieViewModels = new List<MovieUpdateVM>();
            foreach (var movie in movies)
            {
                movieViewModels.Add(new MovieUpdateVM
                {                   
                    Name = movie.Name,
                    Description = movie.Description,
                    Director = movie.Director,
                    MoviePhoto = movie.MoviePhoto,
                    Genre = movie.Genre,
                });
            }
            return Ok(movieViewModels);
        }

        [HttpPost]
        public IActionResult Create([FromForm]MovieCreateVM addmovie)
        {
            var path = "C:\\Users\\Acer\\OneDrive\\Desktop\\C#consoleapp\\MovieAppCleanArchitecture\\MovieAppAPI";
            var filePath = "Images/" + addmovie.MoviePhoto.FileName;
            var fullPath = Path.Combine(path, filePath);
            using (var fileStreams = new FileStream(fullPath, FileMode.Create))
            {
                 addmovie.MoviePhoto.CopyTo(fileStreams);
            }
            
            Movies movie = new Movies()
            {
                Name = addmovie.Name,
                Director = addmovie.Director,
                Description = addmovie.Description,
                MoviePhoto = fullPath,
                Genre = addmovie.Genre,
            };
            _iMovieService.AddMovies(movie);
            return Ok();
        }

        [HttpGet("Delete/{id}")]
        [HttpPost("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            if (HttpContext.Request.Method == "GET")
            {
                var movie = _iMovieService.GetByID(id);
                MovieVM movievm = new MovieVM()
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    Director = movie.Director,
                    Description = movie.Description,
                    MoviePhoto = movie.MoviePhoto,
                    Genre = movie.Genre,
                };
                return Ok(movievm);
            }
            else if (HttpContext.Request.Method == "POST")
            {
                _iMovieService.DeleteMovies(id);
                return Ok();
            }

            // Return an error response if the HTTP method is not supported
            return BadRequest();
        }

        [HttpGet("Edit/{id}")]

        public IActionResult Edit(int id)
        {
            var movie = _iMovieService.GetByID(id);
            MovieVM movievm = new MovieVM()
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Description = movie.Description,
                MoviePhoto = movie.MoviePhoto,
                Genre = movie.Genre,
            };
            return Ok(movievm); // Display the edit view
        }
        [HttpPost("Edits")]
        public IActionResult Edits(MovieVM editmovie)
        {
            Movies movie = new Movies()
            {
                Id = editmovie.Id,
                Name = editmovie.Name,
                Director = editmovie.Director,
                Description = editmovie.Description,
                MoviePhoto = editmovie.MoviePhoto,
                Genre = editmovie.Genre,
            };
            _iMovieService.UpdateMovies(movie);

            return Ok();
        }

    }
}
