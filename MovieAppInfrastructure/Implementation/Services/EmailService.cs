using Microsoft.Extensions.Configuration;
using MimeKit;
using MovieAppApplication.Interface.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;

namespace MovieAppInfrastructure.Implementation.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(_configuration["EmailSettings:ToEmail"]));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:SmtpPort"]),
                MailKit.Security.SecureSocketOptions.StartTls
            );

            smtp.Authenticate(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:AppPassword"]);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }

}
