﻿using Nest;
using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.ViewModel
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
    }
}
