using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppApplication.Interface.IRepository;
using MovieAppDomain.Entities;
using MovieAppPresentation.ViewModel;
using System.Data;

namespace MovieAppPresentation.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _iMovie;
        private readonly IWebHostEnvironment _iwebhostenvironment;
        //private readonly ICommentRepository _iComment;
        //private readonly IRatingRepository _iRating;

        public MovieController(
            IMovieRepository imovie,
            IWebHostEnvironment iwebhostenvironment)
        //ICommentRepository iComment,
        //IRatingRepository iRating)
        {
            _iMovie = imovie;
            _iwebhostenvironment = iwebhostenvironment;
            //_iComment = iComment;
            //_iRating = iRating;
        }
        [HttpGet]
        public IActionResult Index(int page)
        {
            int pageNumber = page;
            int pageSize = 3;
            var movies = _iMovie.GetAllMovies();
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
            int totalMovies = movies.Count;
            int totalPages = (int)Math.Ceiling(totalMovies / (double)pageSize);

            //starting index of each page
            int startIndex = (pageNumber - 1) * pageSize;

            //skip skips first specified number of data and take takes the specified number of data
            //var pagedMovies = movies.Skip(startIndex).Take(pageSize)
            //    {

            //}
            //var pager = new MovieAppPresentation.ViewModel.PagerVM
            //{
            //    Movies = pagedMovies,
            //    PageNumber = pageNumber,
            //    PageSize = pageSize,
            //    TotalMovies = totalMovies,
            //    TotalPages = totalPages
            //};

            return View(movieViewModels);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MovieVM addmovie)
        {
            var image = Request.Form.Files.FirstOrDefault();
            var fileName = Guid.NewGuid().ToString();
            var path = $@"images\";
            var wwwRootPath = _iwebhostenvironment.WebRootPath;
            var uploads = Path.Combine(wwwRootPath, path);
            var extension = Path.GetExtension(image.FileName);
            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                image.CopyTo(fileStreams);
            }
            addmovie.MoviePhoto = $"\\images\\{fileName}" + extension;
            Movies movie = new Movies()
            {
                Id = addmovie.Id,
                Name = addmovie.Name,
                Director = addmovie.Director,
                Description = addmovie.Description,
                MoviePhoto = addmovie.MoviePhoto,
                Genre = addmovie.Genre,
            };
            _iMovie.AddMovies(movie);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var movie = _iMovie.GetByID(Id);
            MovieVM movievm = new MovieVM()
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Description = movie.Description,
                MoviePhoto = movie.MoviePhoto,
                Genre = movie.Genre,
            };
            return View(movievm);
            

            
        }

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        public IActionResult Edit(int Id)
        {
            var movie = _iMovie.GetByID(Id);
            MovieVM movies = new MovieVM()
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Description = movie.Description,
                MoviePhoto = movie.MoviePhoto,
                Genre = movie.Genre,
            };

           

            return View(movies);
        }
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        public IActionResult Edit(MovieVM editmovie)
        {
            var images = Request.Form.Files.FirstOrDefault();
            var fileName = Guid.NewGuid().ToString();
            var path = $@"updateimages\";
            var wwwRootPath = _iwebhostenvironment.WebRootPath;
            var uploads = Path.Combine(wwwRootPath, path);
            var extension = Path.GetExtension(images.FileName);
            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                images.CopyTo(fileStreams);
            }
            editmovie.MoviePhoto = $"\\updateimages\\{fileName}" + extension;
            Movies movie = new Movies()
            {
                Id = editmovie.Id,
                Name = editmovie.Name,
                Director = editmovie.Director,
                Description = editmovie.Description,
                MoviePhoto = editmovie.MoviePhoto,
                Genre = editmovie.Genre,
            };
            _iMovie.UpdateMovies(movie);
            return RedirectToAction("Index");

        }

        [HttpPost]
        public IActionResult DeleteId(int Id)
        {
            _iMovie.DeleteMovies(Id);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var movie = _iMovie.GetByID(id);
            MovieVM movies = new MovieVM()
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Description = movie.Description,
                MoviePhoto = movie.MoviePhoto,
                Genre = movie.Genre,
            };

            //ViewBag.Comments = _iComment.GetComments(id);
            //ViewBag.Ratings = _iRating.GetRatings(id);

            return View(movies);
        }
    }
}




