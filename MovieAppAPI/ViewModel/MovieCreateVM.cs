using Nest;
using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.ViewModel
{
    public class MovieCreateVM
    {
        

        [Required]
        public string Name { get; set; }
        [Required]
        public string Genre { get; set; }

        [Required]
        public string Director { get; set; }
        [Required]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime AddedDate { get; set; }


        [Display(Name = "Insert an Image")]
        public double AverageRating { get; set; }
        public IFormFile? MoviePhoto { get; set; }
    }
}
