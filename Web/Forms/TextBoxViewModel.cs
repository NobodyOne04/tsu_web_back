using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Forms
{
    public class TextBoxViewModel
    {
        [Required]
        public String Text { get; set; }
    }
}
