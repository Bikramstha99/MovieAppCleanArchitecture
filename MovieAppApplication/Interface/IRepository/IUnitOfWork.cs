using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppApplication.Interface.IRepository
{
    public interface IUnitOfWOrk
    {
        IMovieRepository MovieRepo { get; set; }
        ICommentRepository CommentRepo { get; set; }       
        IRatingRepository RatingRepo { get; set; }
        void Save();

    }
}
