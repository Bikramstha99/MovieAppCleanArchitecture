using Microsoft.AspNetCore.Identity;
using MovieAppDomain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.ViewModel
{
    public class CommentVM
    {
       
        public string? CommentDesc { get; set; }

        [ForeignKey("Movies")]
        public int MovieId { get; set; }
    }
}
