using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MovieAppApplication.Interface.IServices;

namespace MovieAppAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public MailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendMail(string body)
        {
            // Use _emailService to send the email
            _emailService.SendMail("Notification from ASP.NET Core Web API", body);
            return Ok();
        }
    }
}
