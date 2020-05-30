using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Web.Models
{
    public class ViewModel
    {
        public int Id { get; set; }
        public PrivateUserInfoModel PrivateUserInfoModel { get; set; }
        public UserInfoModel UserInfoModel { get; set; }
    }



}
