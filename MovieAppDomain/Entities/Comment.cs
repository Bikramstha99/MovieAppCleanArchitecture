using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAppDomain.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string? CommentDesc { get; set; }

        [ForeignKey("Movies")]
        public int MovieId { get; set; }
        public Movies Movies { get; set; }

        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public Microsoft.AspNetCore.Identity.IdentityUser IdentityUser { get; set; }

        public string UserName { get; set; }


        public DateTime TimeStamp { get; set; }
    }
}
