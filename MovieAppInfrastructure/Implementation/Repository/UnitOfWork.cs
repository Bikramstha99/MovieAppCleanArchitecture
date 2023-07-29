using MovieAppApplication.Interface.IRepository;
using MovieAppInfrastructure.Implementation.NewFolder;
using MovieAppInfrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppInfrastructure.Implementation.Repository
{
    public class UnitOfWork : IUnitOfWOrk
    {
        private readonly MovieDbContext _movieDbContext;

        public UnitOfWork(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
            CommentRepo= new CommentRepository(movieDbContext);
            MovieRepo = new MovieRepository(movieDbContext);
            RatingRepo = new RatingRepository(movieDbContext);
        }

        public ICommentRepository CommentRepo { get;set; }
        
        public IMovieRepository MovieRepo { get;set; }
        
        public IRatingRepository RatingRepo { get;set; }
       

        public void Save()
        {
            _movieDbContext.SaveChanges();
        }
    }
}
