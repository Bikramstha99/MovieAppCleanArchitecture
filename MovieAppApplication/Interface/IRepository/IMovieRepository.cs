using MovieAppDomain.Entities;

namespace MovieAppApplication.Interface.IRepository
{
    public interface IMovieRepository 
    {
        bool AddMovies(Movies movie);
        bool UpdateMovies(Movies movie);
        bool DeleteMovies(int Id);
        Movies GetByID(int Id);
        List<Movies> GetAllMovies();
    }
}
