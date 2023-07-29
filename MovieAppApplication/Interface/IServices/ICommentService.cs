using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppApplication.Interface.IServices
{
    public interface ICommentService
    {
        bool AddComment(Comment comment);
        Comment GetByCommentId(int CommentId);
        List<Comment> GetMovieComments(int movieId);
        bool UpdateComment(Comment comment);
        bool DeleteComment(Comment comment);
    }
}
