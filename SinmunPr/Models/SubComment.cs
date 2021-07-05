using SinmunPr.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Models
{
    public class SubComment
    {
        public int id { get; set; }

        public string content { get; set; }

        public bool IsConfirm { get; set; }

        public DateTime creationTime { get; set; }

        #region ForeignKeys
        public int mainCommentId { get; set; }
        [ForeignKey("mainCommentId")]
        public MainComment MainComment { get; set; }

        public string userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser applicationUser { get; set; }
        #endregion
    }
}
