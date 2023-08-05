using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;

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

        //[HttpPost]
        //public async Task<IActionResult> SendSMTPMail(EmailServiceVM emailServiceVM)
        //{
        //    // Use _emailService to send the email
        //    //_emailService.SendMail("Bikram.stha4@gmail.com","Notification from ASP.NET Core Web API", body);
        //    _emailService.SendSMTPMail("Notification from ASP.NET Core Web API", body);
        //    return Ok();
        //}
    }
}
