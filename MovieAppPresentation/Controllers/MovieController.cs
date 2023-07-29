
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using MovieAppPresentation.ViewModel;
using System.Data;

namespace MovieAppPresentation.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _iMovieService;
        private readonly IRatingService _iRatingService;
        private readonly ICommentService _iCommentService;
        private readonly IWebHostEnvironment _iwebhostenvironment;
        

        public MovieController(
            IMovieService iMovieService,
            IRatingService iRatingService,
            ICommentService iCommentService,
            IWebHostEnvironment iwebhostenvironment
           )
        {
            
            _iMovieService = iMovieService;
            _iRatingService = iRatingService;
            _iCommentService = iCommentService;
            _iwebhostenvironment = iwebhostenvironment;
           
        }
        //To show the list of movie 
        [HttpGet]
        public IActionResult Index(int page)
        {
            int pageNumber = page;
            int pageSize = 3;
            var movies = _iMovieService.GetAllMovies();
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
            int totalMovies = movieViewModels.Count;
            int totalPages = (int)Math.Ceiling(totalMovies / (double)pageSize);

            //starting index of each page
            int startIndex = (pageNumber - 1) * pageSize;

            //skip skips first specified number of data and take takes the specified number of data
            var pagedMovies = movieViewModels.Skip(startIndex).Take(pageSize).ToList();
            var pager = new PagerVM
            {
                Movies = pagedMovies,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalMovies = totalMovies,
                TotalPages = totalPages
            };

            return View(pager);
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
            _iMovieService.AddMovies(movie);
            return RedirectToAction("Index");

        }
        //To Delete the data

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var movie = _iMovieService.GetByID(Id);
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

        [HttpPost]
        public IActionResult DeleteId(int Id)
        {
            _iMovieService.DeleteMovies(Id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var movie = _iMovieService.GetByID(Id);
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
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
            _iMovieService.UpdateMovies(movie);
           
            return RedirectToAction("Index");

        }       
        [HttpGet]
        public IActionResult Detail(int id)
        {
            var comments= _iCommentService.GetMovieComments(id);
            List<CommentVM> commentsVM = new List<CommentVM>();           
            commentsVM = comments
                       .Where(c => c.MovieId == id)
                       .Select(s => new CommentVM()
                       {
                           MovieId = s.MovieId,
                           CommentId = s.CommentId,
                           CommentDesc = s.CommentDesc,
                           UserName = s.UserName,
                           UserId = s.UserId
                       })
                       .ToList();
            var ratings = _iRatingService.GetRatings(id);
            List<RatingVM> ratingsVM= new List<RatingVM>();
            ratingsVM=ratings
                      .Where(r => r.MovieId == id)
                      .Select(s=> new RatingVM()
                      {
                          MovieId= s.MovieId,
                          RatingId=s.RatingId,
                          Ratings=s.Ratings,
                          UserId=s.UserId
                      })
                      .ToList();

            var movie = _iMovieService.GetByID(id);
            MovieVM movies = new MovieVM()
            {
                Id = movie.Id,
                Name = movie.Name,
                Director = movie.Director,
                Description = movie.Description,
                MoviePhoto = movie.MoviePhoto,
                AverageRating=movie.AverageRating,
                Genre = movie.Genre,
                comments = commentsVM,
                ratings = ratingsVM,
            };
            return View(movies);
        }
    }
}




