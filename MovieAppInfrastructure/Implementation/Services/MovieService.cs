using MovieAppApplication.Interface.IRepository;
using MovieAppApplication.Interface.IServices;
using MovieAppDomain.Entities;
using System;
using System.Collections.Generic;
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

        public MovieService(IUnitOfWOrk iUnitOfWork, IEmailService iEmailService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iEmailService = iEmailService;
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

        public async Task<string> SendEmail(int Id, string Email)
        {
            var movie = _iUnitOfWork.MovieRepo.GetByID(Id);
            {
                string filepath = Path.Combine("C:\\Users\\Acer\\OneDrive\\Desktop\\C#consoleapp\\MovieAppCleanArchitecture\\MovieAppAPI\\EmailHtml", "SendEmailFormat.html");

                string imagepath = Path.Combine("C:\\Users\\Acer\\OneDrive\\Desktop\\C#consoleapp\\MovieAppCleanArchitecture\\MovieAppAPI\\Images", movie.MoviePhoto);

                byte[] imageData = File.ReadAllBytes(imagepath);
                string imageBase64 = Convert.ToBase64String(imageData);

                string imageExtension = Path.GetExtension(imagepath).TrimStart('.');
                string imageMimeType = $"image/{imageExtension}";
                string imageSrc = $"data:{imageMimeType};base64,{imageBase64}";

                var layout = await File.ReadAllTextAsync(filepath);

                layout = layout.Replace("##Title##", "MovieApp");
                layout = layout.Replace("##imagesLink##", imageSrc);
                layout = layout.Replace("##ImageName##", movie.Name);
                layout = layout.Replace("##movieTitle##", movie.Name);
                layout = layout.Replace("##MovieDirector##", movie.Director);
                layout = layout.Replace("##MovieDescription##", movie.Description);
                layout = layout.Replace("##Genre##", movie.Genre);

                layout = layout.Replace("##AverageRating##", movie.AverageRating.ToString());
                EmailServiceVM emailservicevm = new()
                {
                    Subject = "A movie has been Recommended",
                    ReceiverEmail = Email,
                    UserName = Email,
                    Message = "",
                    HtmlContent = layout
                };
                return await _iEmailService.SendSMTPEmail(emailservicevm);
            }
        }

        public bool UpdateMovies(Movies movie)
        {
            _iUnitOfWork.MovieRepo.UpdateMovies(movie);
            _iUnitOfWork.Save();
            return true;
        }
        
    }
}
