using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class SignUpSec : SignUpFst
    {
        [Required]
        public String Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public String ConfPassword { get; set; }
    }
}
