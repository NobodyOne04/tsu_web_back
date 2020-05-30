using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Web.Models
{

    public class UserInfoModel
    {

        [Required(ErrorMessage ="Поле обязательно для заполнения")]
        [StringLength(50, MinimumLength = 1, ErrorMessage ="Одну букву нельзя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Одну букву нельзя")]
        public string SecondName { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(1,31,ErrorMessage ="Несуществующая дата")]
        public int Day { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Month { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(1900, 2020, ErrorMessage ="Несуществующий год")]
        public string Year { get; set; }
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Gender { get; set; }

        public List<SelectListItem> Mounts { get; } = new List<SelectListItem>
        {
                    new SelectListItem { Value ="January", Text ="January" },
                    new SelectListItem { Value ="February", Text ="February" },
                    new SelectListItem { Value ="March", Text ="March" },
                    new SelectListItem { Value ="April", Text ="April" },
                    new SelectListItem { Value ="May", Text ="May" },
                    new SelectListItem { Value ="June", Text ="June" },
                    new SelectListItem { Value ="July", Text ="July" },
                    new SelectListItem { Value ="August", Text ="August" },
                    new SelectListItem { Value ="September", Text ="September" },
                    new SelectListItem { Value ="October", Text ="October" },
                    new SelectListItem { Value ="November", Text ="November" },
                    new SelectListItem { Value ="December", Text ="December" }
        };
        public List<SelectListItem> Days { get; } = new List<SelectListItem>();
        public List<SelectListItem> Years { get; } = new List<SelectListItem>();
        public List<string> Genres { get; } = new List<string>();
        public UserInfoModel() {
            for(int i  = 1; i <=31; i++)
            {
                this.Days.Add(new SelectListItem {Value= i.ToString(), Text = i.ToString() });
            }
            for (int i = 2020; i >= 1900; i--)
            {
                this.Years.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }
            Genres.Add("Male");
            Genres.Add("Female");
        }
    }
}
