using Microsoft.AspNetCore.Identity;
using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace MovieAppInfrastructure.Implementation.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWOrk _iUnitOfWork;
        private readonly IEmailService _iEmailService;
        private readonly UserManager<IdentityUser> _userManager;

        public MovieService(IUnitOfWOrk iUnitOfWork, IEmailService iEmailService, UserManager<IdentityUser> userManager)
        {
            _iUnitOfWork = iUnitOfWork;
            _iEmailService = iEmailService;
            _userManager = userManager;
        }

        public bool AddMovies(Movies movie)
        {
            _iUnitOfWork.MovieRepo.AddMovies(movie);
            _iUnitOfWork.Save();
            return true;
        }

        public bool DeleteMovies(int Id)
        {
            _iUnitOfWork.MovieRepo.DeleteMovies(Id);
            _iUnitOfWork.Save();
            return true;
        }

        public List<Movies> GetAllMovies()
        {
            var movies=_iUnitOfWork.MovieRepo.GetAllMovies();
            return movies;
        }

        public Movies GetByID(int Id)
        {
            var movie=_iUnitOfWork.MovieRepo.GetByID(Id);
            return movie;
        }

        //public async Task<string> SendEmail(int Id, string Email)
        //{
        //    DateTime current_date = DateTime.Now;
        //    var movies = _iUnitOfWork.MovieRepo.GetMovieOnDate(current_date);
        //    if (movies.Count == 0)
        //    {
        //        return "No movies found for the current date.";
        //    }
        //    string filepath = Path.Combine("C:\\Users\\Acer\\OneDrive\\Desktop\\C#consoleapp\\MovieAppCleanArchitecture\\MovieAppAPI\\EmailHtml", "SendEmailFormat.html");

        //    var layout = await File.ReadAllTextAsync(filepath);
        //    layout = layout.Replace("##Title##", "MovieApp");

        //    List<EmailServiceVM> emailServices = new List<EmailServiceVM>();
        //    foreach (var movie in movies)
        //    {
        //        string imagepath = Path.Combine("C:\\Users\\Acer\\OneDrive\\Desktop\\C#consoleapp\\MovieAppCleanArchitecture\\MovieAppAPI\\Images", movie.MoviePhoto);
        //        byte[] imageData = File.ReadAllBytes(imagepath);
        //        string imageBase64 = Convert.ToBase64String(imageData);

        //        string imageExtension = Path.GetExtension(imagepath).TrimStart('.');
        //        string imageMimeType = $"image/{imageExtension}";
        //        string imageSrc = $"data:{imageMimeType};base64,{imageBase64}";

        //        string emailContent = layout
        //            //.Replace("##imagesLink##", imageSrc)
        //            .Replace("##ImageName##", movie.Name)
        //            .Replace("##movieTitle##", movie.Name)
        //            .Replace("##MovieDirector##", movie.Director)
        //            .Replace("##MovieDescription##", movie.Description)
        //            .Replace("##Genre##", movie.Genre)
        //            .Replace("##AverageRating##", movie.AverageRating.ToString());

        //        EmailServiceVM emailservicevm = new EmailServiceVM
        //        {
        //            Subject = "A movie has been Recommended",
        //            ReceiverEmail = Email,
        //            UserName = Email,
        //            Message = "",
        //            HtmlContent = emailContent
        //        };
        //        emailServices.Add(emailservicevm);

        //    }
        //    return await _iEmailService.SendSMTPEmail(emailServices);

        //}
        public async Task<string> SendEmail(int Id, string Email)
        {
            DateTime current_date = DateTime.Now;
            var movies = _iUnitOfWork.MovieRepo.GetMovieOnDate(current_date);
            if (movies.Count == 0)
            {
                return "No movies found for the current date.";
            }
            string filepath = Path.Combine("C:\\Users\\Acer\\OneDrive\\Desktop\\C#consoleapp\\MovieAppCleanArchitecture\\MovieAppAPI\\EmailHtml", "SendEmailFormat.html");

            var layout = await File.ReadAllTextAsync(filepath);
            layout = layout.Replace("##Title##", "MovieApp");

            string combinedEmailContent = ""; // Initialize an empty string to combine email content for both movies

            foreach (var movie in movies)
            {
                // ... construct the email content for each movie ...
                string imagepath = Path.Combine("C:\\Users\\Acer\\OneDrive\\Desktop\\C#consoleapp\\MovieAppCleanArchitecture\\MovieAppAPI\\Images", movie.MoviePhoto);
                byte[] imageData = File.ReadAllBytes(imagepath);
                string imageBase64 = Convert.ToBase64String(imageData);

                string imageExtension = Path.GetExtension(imagepath).TrimStart('.');
                string imageMimeType = $"image/{imageExtension}";
                string imageSrc = $"data:{imageMimeType};base64,{imageBase64}";

                string emailContent = layout
                    //.Replace("##imagesLink##", imageSrc)
                    .Replace("##ImageName##", movie.Name)
                    .Replace("##movieTitle##", movie.Name)
                    .Replace("##MovieDirector##", movie.Director)
                    .Replace("##MovieDescription##", movie.Description)
                    .Replace("##Genre##", movie.Genre)
                    .Replace("##AverageRating##", movie.AverageRating.ToString());

                combinedEmailContent += emailContent;

                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    EmailServiceVM emailService = new EmailServiceVM
                    {
                        Subject = "Movies Recommended",
                        ReceiverEmail = user.Email, // Get the email address of the user from the database
                        UserName = user.UserName, // Get the username of the user from the database
                        Message = "",
                        HtmlContent = combinedEmailContent // Combined email content for the current movie
                    };

                    // Call SendSMTPEmail for each user to send the email individually
                    string result = await _iEmailService.SendSMTPEmail(emailService);

                    // You can handle the result here (e.g., log success/failure)
                }
            }

            return "Movie recommendations email sent successfully to all users.";
        }


        public bool UpdateMovies(Movies movie)
        {
            _iUnitOfWork.MovieRepo.UpdateMovies(movie);
            _iUnitOfWork.Save();
            return true;
        }
        
    }
}
