using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class ResetModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string email { get; set; }
    }
}
