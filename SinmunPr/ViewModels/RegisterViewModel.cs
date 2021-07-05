using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="لطفا نام کاربری خود را وارد کنید.")]
        public string username { get; set; }

        [Required(ErrorMessage ="لطفا رمزعبور خود را وارد کنید.")]
        [MinLength(6, ErrorMessage ="رمز عبور باید حداقل 6 حرف باشد.")]
        public string password { get; set; }

        [Required(ErrorMessage ="لطفا تکرار رمزعبور خود را وارد کنید.")]
        [Compare("password", ErrorMessage ="رمزعبور با تکرار رمز عبور برابر نیست.")]
        public string repassword { get; set; }

        [Required(ErrorMessage ="لطفا ایمیل خود را وارد کنید.")]
        public string email { get; set; }
    }
}
