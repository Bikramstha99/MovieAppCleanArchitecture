using Microsoft.AspNetCore.Mvc;

namespace MovieAppAPI.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
