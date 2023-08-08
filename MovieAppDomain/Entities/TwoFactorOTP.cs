using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAppDomain.Entities
{
    public class TwoFactorOTP
    {
        [Key]
        public int Id { get; set; }

      
        [ForeignKey("IdentityUser")]
        public string Email { get; set; }

        [Required]
        public string OTP { get; set; }

        [Required]
        public DateTime GeneratedDateTime{ get; set; }
        [Required]
        public DateTime ExpiredDateTime { get; set; }
    }
}
