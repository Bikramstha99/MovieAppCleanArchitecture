using Nest;
using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.ViewModel
{
    public class MovieUpdateVM
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Genre { get; set; }

        [Required]
        public string Director { get; set; }
        [Required]
        public string Description { get; set; }


        [Display(Name = "Insert an Image")]
        public string MoviePhoto { get; set; }
        public DateTime ReleaseDate { get; set; }

        public DateTime AddedDate { get; set; }
        //public double AverageRating { get; set; }

    }
}
