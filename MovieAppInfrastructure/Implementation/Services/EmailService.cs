using Microsoft.Extensions.Configuration;
using MimeKit;
using MovieAppApplication.Interface.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net;
using MovieAppDomain.Entities;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace MovieAppInfrastructure.Implementation.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //public void SendMail(string subject, string body)
        //{
        //    {
        //        var email = new MimeMessage();
        //        email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
        //        email.To.Add(MailboxAddress.Parse(_configuration["EmailSettings:ToEmail"]));
        //        email.Subject = subject;
        //        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

        //        using var smtp = new SmtpClient();
        //        smtp.Connect(
        //            _configuration["EmailSettings:SmtpServer"],
        //            int.Parse(_configuration["EmailSettings:SmtpPort"]),
        //            MailKit.Security.SecureSocketOptions.StartTls
        //        );

        //        smtp.Authenticate(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:AppPassword"]);

        //        smtp.Send(email);
        //        smtp.Disconnect(true);
        //    }
        //}
        public async Task<string> SendSMTPEmail(EmailServiceVM emailservices)
        {
            string senderEmail = _configuration["EmailSettings:FromEmail"];

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_configuration["EmailSettings:FromEmail"], _configuration["EmailSettings:Password"]);

            try
            {
                
                    string receipentEmail = emailservices.ReceiverEmail;
                    string subject = emailservices.Subject;
                    string body = emailservices.HtmlContent;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(senderEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(receipentEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                    mailMessage.Dispose();
                

                return "Mail Sent Successfully";
            }
            catch (Exception ex)
            {
                return $"Error Sending Mail: {ex.Message}";
            }
            finally
            {
                smtpClient.Dispose();
            }
        }

    }
}
