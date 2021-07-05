using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewModels
{
    public class EditProfileViewModel
    {
        [MinLength(6, ErrorMessage ="نام کاربری باید حداقل 6 کاراکتر و حداکثر 18 کاراکتر باشد.")]
        [MaxLength(18, ErrorMessage ="نام کاربری باید حداقل 6 کاراکتر و حداکثر 18 کاراکتر باشد.")]
        public string username { get; set; }

        [Phone(ErrorMessage ="فرمت وارد شده صحیح نیست")]
        public string phonenumber { get; set; }

        public string bio { get; set; }

        [Required(ErrorMessage ="لطفا نام خود را وارد کنید. اگر قصد تغییر ندارید، نام قبلی خود را وارد کنید.")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید. اگر قصد تغییر ندارید، نام خانوادگی قبلی خود را وارد کنید.")]
        public string lastname { get; set; }

        public IFormFile avatar { get; set; }
    }
}