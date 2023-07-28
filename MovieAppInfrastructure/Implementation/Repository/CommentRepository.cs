using MovieAppApplication.Interface.IRepository;
using MovieAppDomain.Entities;
using MovieAppInfrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppInfrastructure.Implementation.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public CommentRepository(MovieDbContext moviedbcontext)
        {
            _movieDbContext = moviedbcontext;
        }
        public bool AddComment(Comment comment)
        {
            _movieDbContext.Comments.Add(comment);
            _movieDbContext.SaveChanges();
            return true;
        }

        public bool DeleteComment(Comment comment)
        {
            _movieDbContext.Remove(comment);
            _movieDbContext.SaveChanges();
            return true;
        }

        public Comment GetByCommentId(int CommentId)
        {
            var comments = _movieDbContext.Comments.Find(CommentId);
            return comments;
        }

        public List<Comment> GetMovieComments(int movieId)
        {
            List<Comment> comments = new List<Comment>();
            comments = _movieDbContext.Comments
                .Where(c => c.MovieId == movieId)
                .ToList();
            comments = comments.OrderByDescending(c => c.TimeStamp).ToList();
            return comments;
        }

        public bool UpdateComment(Comment comment)
        {
            _movieDbContext.Update(comment);
            _movieDbContext.SaveChanges();
            return true;
        }
    }
}
