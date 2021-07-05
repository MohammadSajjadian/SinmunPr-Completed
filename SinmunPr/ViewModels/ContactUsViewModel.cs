using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewModels
{
    public class ContactUsViewModel
    {
        [Required(ErrorMessage = "نام خود را وارد کنید")]
        public string name { get; set; }

        [Required(ErrorMessage = "ایمیل خود را وارد کنید")]
        public string email { get; set; }

        [Required(ErrorMessage = "موضوع خود را بنویسید")]
        public string subject { get; set; }

        [Required(ErrorMessage = "پیام خود را بنویسید")]
        public string content { get; set; }
    }
}
