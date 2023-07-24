

using MovieAppDomain.Entities;

namespace MovieAppApplication.Interface.IRepository
{
    public interface IMovieRepository 
    {
        bool AddMovies(Movies movie);
        //bool UpdateMovies(UpdateMovie updatemovie);
        //bool DeleteMovies(int Id);
        //UpdateMovie GetByID(int Id);
        List<Movies> GetAllMovies();
    }
}
