using Microsoft.AspNetCore.Identity;
using MovieAppDomain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppPresentation.ViewModel
{
    public class CommentVM
    {
        [Key]
        public int CommentId { get; set; }
        public string? CommentDesc { get; set; }

        [ForeignKey("Movies")]
        public int MovieId { get; set; }
        public Movies Movies { get; set; }

        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public IdentityUser IdentityUser { get; set; }

        public string UserName { get; set; }


        public DateTime TimeStamp { get; set; }
    }
}