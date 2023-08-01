using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppInfrastructure.Implementation.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWOrk _iUnitOfWork;

        public MovieService(IUnitOfWOrk iUnitOfWork)
        {
            _iUnitOfWork = iUnitOfWork;
        }

        public bool AddMovies(Movies movie)
        {
            _iUnitOfWork.MovieRepo.AddMovies(movie);
            _iUnitOfWork.Save();
            return true;
        }

        public bool DeleteMovies(int Id)
        {
            _iUnitOfWork.MovieRepo.DeleteMovies(Id);
            _iUnitOfWork.Save();
            return true;
        }

        public List<Movies> GetAllMovies()
        {
            var movies=_iUnitOfWork.MovieRepo.GetAllMovies();
            return movies;
        }

        public Movies GetByID(int Id)
        {
            var movie=_iUnitOfWork.MovieRepo.GetByID(Id);
            return movie;
        }

        public bool UpdateMovies(Movies movie)
        {
            _iUnitOfWork.MovieRepo.UpdateMovies(movie);
            _iUnitOfWork.Save();
            return true;
        }
    }
}
