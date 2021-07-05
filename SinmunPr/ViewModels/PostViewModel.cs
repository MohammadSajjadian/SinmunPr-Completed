using Microsoft.AspNetCore.Http;
using SinmunPr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewModels
{
    public class PostViewModel
    {
        [Required(ErrorMessage = "لطفا نام دسته بندی را وارد کنید.")]
        public int categoryId { get; set; }

        [Required(ErrorMessage = "لطفا عکس خبر را وارد کنید.")]
        public IFormFile mainImg { get; set; }

        [Required(ErrorMessage = "لطفا عنوان خبر را وارد کنید.")]
        public string topic { get; set; }
        [Required(ErrorMessage = "لطفا توضیح مختصر خبر را وارد کنید.")]
        public string shortDes { get; set; }
        [Required(ErrorMessage = "لطفا محتوای خبر را وارد کنید.")]
        public string content { get; set; }

        public bool IsInImmediate { get; set; }
        public bool IsInTopSlideBar { get; set; }
        public bool IsInEditorsChoice { get; set; }
        public bool IsInDedicated { get; set; }
        public bool IsInImportant { get; set; }

        public List<string> sourceName { get; set; }
        public List<string> sourceLink { get; set; }
        public List<int> tags { get; set; }
    }
}
