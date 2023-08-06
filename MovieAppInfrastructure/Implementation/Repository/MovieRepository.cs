using MovieAppApplication.Interface.IRepository;
using MovieAppDomain.Entities;
using MovieAppInfrastructure.Implementation.Repository;
using MovieAppInfrastructure.Persistance;
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
            //_movieDbContext.SaveChanges();
            //_iUnitOfWork.Save();
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
            movies.Id = movie.Id;
            movies.Name = movie.Name;
            movies.Genre = movie.Genre;
            movies.MoviePhoto = movie.MoviePhoto;
            movies.Director = movie.Director;
            movies.Description = movie.Description;
            movies.AverageRating = movie.AverageRating;
            //_movieDbContext.SaveChanges();
            //_iUnitOfWork.Save();
            return true;
        }

        public bool DeleteMovies(int Id)
        {
            var movie = _movieDbContext.Movies.Find(Id);
            _movieDbContext.Movies.Remove(movie);
            //_movieDbContext.SaveChanges();
            return true;
        }

        public List<Movies> GetAllMovies()
        {
            var data = _movieDbContext.Movies.ToList();
            return data;

        }

        public List<Movies> GetMovieOnDate(DateTime dateTime)
        {
            var moviesOnDate = _movieDbContext.Movies
            .Where(movie => movie.ReleaseDate.Date == dateTime.Date)
            .ToList();

            return moviesOnDate;
        }
    }
}
