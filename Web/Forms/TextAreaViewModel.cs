using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Forms
{
    public class TextAreaViewModel
    {
        [Required]
        public String Text { get; set; }
    }
}