﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using MovieAppInfrastructure.Implementation.Repository;
using MovieAppPresentation.ViewModel;

namespace MovieAppPresentation.Controllers
{
    public class CommentController : Controller
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

        public IActionResult AddComment()
        {
            return PartialView("_AddComment");
        }
        [HttpPost]
        public IActionResult AddComment(CommentVM commentVM)
        {
            try
            {
                string Username = User.Identity.Name;
                string UserId = _userManager.GetUserId(User);
                Comment comment = new Comment();
                comment.CommentId = commentVM.CommentId;
                comment.UserId = UserId;
                comment.CommentDesc = commentVM.CommentDesc;
                comment.UserName = Username;
                comment.MovieId = commentVM.MovieId;
                _iCommentService.AddComment(comment);
                return RedirectToAction("Detail", "Movie", new { id = commentVM.MovieId });
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }

        }
         [HttpGet]
        public IActionResult Edit(int CommentId)
        {
            var comment = _iCommentService.GetByCommentId(CommentId);
            CommentVM commentvm = new CommentVM()
            {
                MovieId = comment.MovieId,
                CommentId = comment.CommentId,
                CommentDesc = comment.CommentDesc,
            };
            return View(commentvm);

        }
        [HttpPost]
        public IActionResult Edit(CommentVM updatecomment)
        {
            var comment= _iCommentService.GetByCommentId(updatecomment.CommentId);
            comment.UserId = _userManager.GetUserId(User);
            comment.UserName = _userManager.GetUserName(User);
            comment.TimeStamp = DateTime.Now;
            comment.CommentDesc = updatecomment.CommentDesc;
            _iCommentService.UpdateComment(comment);
            return RedirectToAction("Detail","Movie", new { id = updatecomment.MovieId });
        }

        [HttpGet]
        public IActionResult Delete(int CommentId)
        { 
            var comment = _iCommentService.GetByCommentId(CommentId);
            CommentVM commentvm = new CommentVM()
            {
                MovieId = comment.MovieId,
                CommentId = comment.CommentId,
                CommentDesc = comment.CommentDesc,
            };
            return View(commentvm);
        }

        [HttpPost]
        public IActionResult Delete(CommentVM deletecomment)
        {
            var comment = _iCommentService.GetByCommentId(deletecomment.CommentId);
            comment.UserId = _userManager.GetUserId(User);
            comment.UserName = _userManager.GetUserName(User);
            comment.TimeStamp = DateTime.Now;
            comment.CommentDesc = deletecomment.CommentDesc;
            _iCommentService.DeleteComment(comment);  
            return RedirectToAction("Detail","Movie",new {id=deletecomment.MovieId});
        }


    }
}

