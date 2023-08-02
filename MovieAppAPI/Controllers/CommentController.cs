using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.ViewModel;
using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;

namespace MovieAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICommentService _iCommentService;
        private readonly IMovieService _iMovieService;
        private readonly IUnitOfWOrk _iUnitOfWork;
        private readonly ICommentRepository _iComment;

        public CommentController(UserManager<IdentityUser> userManager, ICommentService iCommentService, IMovieService iMovieService)
        {
            _userManager = userManager;
            _iCommentService = iCommentService;
            _iMovieService = iMovieService;
        }

        [HttpPost]
        public IActionResult AddComment(CommentVM commentVM)
        {
            try
            {
                string Username = User.Identity.Name;
                string UserId = _userManager.GetUserId(User);
                Comment comment = new Comment();

                comment.UserId = UserId;
                comment.CommentDesc = commentVM.CommentDesc;
                comment.UserName = Username;
                comment.MovieId = commentVM.MovieId;
                _iCommentService.AddComment(comment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
