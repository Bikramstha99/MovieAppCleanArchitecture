
using System.ComponentModel.DataAnnotations;

namespace MovieAppPresentation.ViewModel
{
    public class MovieVM
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Genre { get; set; }

        [Required]
        public string Director { get; set; }
        [Required]
        public string Description { get; set; }


        [Display(Name = "Insert an Image")]
        public double AverageRating { get; set; }
        public string MoviePhoto { get; set; }
        public List<MovieVM> Movies { get; set; }
        public CommentVM commentVM { get; set; }
        public List<CommentVM> comments { get; set; }
        public RatingVM ratingVM { get; set; }
        public List<RatingVM> ratings { get; set; }
    }

}
