using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "لطفا نام دسته بندی را وارد کنید.")]
        public string Pname { get; set; }
        [Required(ErrorMessage = "لطفا نام انگلیسی دسته بندی را انتخاب کنید.")]
        public string Ename { get; set; }
    }
}
