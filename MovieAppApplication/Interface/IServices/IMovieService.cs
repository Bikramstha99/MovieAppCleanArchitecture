using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppApplication.Interface.IServices
{
    public interface IMovieService
    {
        bool AddMovies(Movies movie);
        bool UpdateMovies(Movies movie);
        bool DeleteMovies(int Id);
        Movies GetByID(int Id);
        List<Movies> GetAllMovies();
        Task<string> SendEmail(int Id, string Email);


    }
}
