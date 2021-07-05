using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.ViewModels
{
    public class CommentViewModel
    {
        public int postId { get; set; }

        [Required(ErrorMessage = "لطفا نظر خود را وارد کنید.")]
        [Range(1, 1500, ErrorMessage = "حداکثر حروف مجاز 1500 کاراکتر میباشد.")]
        public string content { get; set; }
    }
}
