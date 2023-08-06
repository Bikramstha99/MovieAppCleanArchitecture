using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MovieAppDomain.Entities
{
    public class Movies
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
    
        public DateTime ReleaseDate { get; set; }
        
        public DateTime AddedDate { get; set; }

        [Display(Name = "Insert an Image")]
        public double AverageRating { get; set; }
        public string MoviePhoto { get; set; }


    }
}
