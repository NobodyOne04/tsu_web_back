using System;
using System.ComponentModel.DataAnnotations;


namespace Web.Models
{
    public class SignUp
    {
        [Required]
        public String FName { get; set; }
        [Required]
        public String SName { get; set; }
        [Required]
        public String born { get; set; }
        [Required]
        public String gender { get; set; }
    }
}
