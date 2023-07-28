using Microsoft.AspNetCore.Identity;
using MovieAppDomain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppPresentation.ViewModel
{
    public class RatingVM
    {
        public int RatingId { get; set; }
        public int Ratings { get; set; }

        [ForeignKey("Movies")]
        public int MovieId { get; set; }
        public Movies Movies { get; set; }

        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
