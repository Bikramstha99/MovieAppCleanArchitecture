using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Persistance;
using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppInfrastructure.Implementation.NewFolder
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public MovieRepository(MovieDbContext movieDbContext)
        {

            _movieDbContext = movieDbContext;
        }
        public bool AddMovies(Movies movie)
        {
            _movieDbContext.Movies.Add(movie);
            _movieDbContext.SaveChanges();
            return true;
        }

        //public UpdateMovie GetByID(int Id)
        //{
        //    var movie = _movieDbContext.Movies.Find(Id);
        //    var viewmodel = new UpdateMovie()
        //    {
        //        Id = movie.Id,
        //        Name = movie.Name,
        //        Genre = movie.Genre,
        //        MoviePhoto = movie.MoviePhoto,
        //        Director = movie.Director,
        //        AverageRating = movie.AverageRating,
        //        Description = movie.Description,
        //    };
        //    return viewmodel;
        //}

        //public bool UpdateMovies(UpdateMovie updatemovie)
        //{
        //    var movie = _movieDbContext.Movies.Find(updatemovie.Id);
        //    movie.Id = updatemovie.Id;
        //    movie.Name = updatemovie.Name;
        //    movie.Genre = updatemovie.Genre;
        //    movie.MoviePhoto = updatemovie.MoviePhoto;
        //    movie.Director = updatemovie.Director;
        //    movie.Description = updatemovie.Description;
        //    movie.AverageRating = updatemovie.AverageRating;
        //    _movieDbContext.SaveChanges();
        //    return true;
        //}

        //public bool DeleteMovies(int Id)
        //{
        //    var movie = _movieDbContext.Movies.Find(Id);
        //    _movieDbContext.Movies.Remove(movie);
        //    _movieDbContext.SaveChanges();
        //    return true;
        //}

        public List<Movies> GetAllMovies()
        {
            var data = _movieDbContext.Movies.ToList();
            return data;

        }
    }
}
