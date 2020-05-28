using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Forms
{
    public class RadioViewModel
    {
        [Required]
        public Int32? Month { get; set; }
    }
}