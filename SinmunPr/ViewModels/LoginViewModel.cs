using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="لطفا نام کاربری خود را وارد کنید.")]
        public string username { get; set; }

        [Required(ErrorMessage ="لطفا رمزعبور خود را وارد کنید.")]
        public string password { get; set; }

        public bool rememmberme { get; set; }
    }
}
