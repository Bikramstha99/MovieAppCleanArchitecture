using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppDomain.Entities
{
    public class TwoFactorOTPPhone
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string OTP { get; set; }

        [Required]
        public DateTime GeneratedDateTime { get; set; }
        [Required]
        public DateTime ExpiredDateTime { get; set; }
    }
}
