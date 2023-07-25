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

        public Movies GetByID(int Id)
        {
            var movie = _movieDbContext.Movies.Find(Id);   
            return movie;
        }

        public bool UpdateMovies(Movies movie)
        {
            var movies = _movieDbContext.Movies.Find(movie.Id);
            movie.Id = movie.Id;
            movie.Name = movie.Name;
            movie.Genre = movie.Genre;
            movie.MoviePhoto = movie.MoviePhoto;
            movie.Director = movie.Director;
            movie.Description = movie.Description;
            movie.AverageRating = movie.AverageRating;
            _movieDbContext.SaveChanges();
            return true;
        }

        public bool DeleteMovies(int Id)
        {
            var movie = _movieDbContext.Movies.Find(Id);
            _movieDbContext.Movies.Remove(movie);
            _movieDbContext.SaveChanges();
            return true;
        }

        public List<Movies> GetAllMovies()
        {
            var data = _movieDbContext.Movies.ToList();
            return data;

        }
    }
}
