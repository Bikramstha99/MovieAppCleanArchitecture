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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWOrk _iUnitOfWork;

        public CommentService(IUnitOfWOrk iUnitOfWork)
        {
            _iUnitOfWork = iUnitOfWork;
        }

        public bool AddComment(Comment comment)
        {
            _iUnitOfWork.CommentRepo.AddComment(comment);
            return true;
        }

        public bool DeleteComment(Comment comment)
        {
            _iUnitOfWork.CommentRepo?.DeleteComment(comment);
            return true;
        }

        public Comment GetByCommentId(int CommentId)
        {
            var comment= _iUnitOfWork.CommentRepo.GetByCommentId(CommentId);
            return comment;
        }

        public List<Comment> GetMovieComments(int movieId)
        {
            var comments= _iUnitOfWork.CommentRepo.GetMovieComments(movieId);
            return comments;
        }

        public bool UpdateComment(Comment comment)
        {
            _iUnitOfWork.CommentRepo.UpdateComment(comment);
            return true;
        }
    }
}
