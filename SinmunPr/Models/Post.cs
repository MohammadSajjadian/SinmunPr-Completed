using SinmunPr.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Models
{
    public class Post
    {
        public int id { get; set; }

        #region Images
        public byte[] mainImg { get; set; }
        public byte[] bigSlideBar { get; set; }
        public byte[] dedicated { get; set; }
        public byte[] footerImg { get; set; }
        #endregion

        #region PostInfo
        public string topic { get; set; }
        public string shortDes { get; set; }
        public string content { get; set; }
        public string pPostCreation { get; set; }
        #endregion

        public DateTime ePostCreation { get; set; }

        #region boolians
        public bool IsInImmediate { get; set; }
        public bool IsInTopSlideBar { get; set; }
        public bool IsInEditorsChoice { get; set; }
        public bool IsInDedicated { get; set; }
        public bool IsInImportant { get; set; }
        #endregion

        #region ForeignKeys
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public Category category { get; set; }

        public string userId { get; set; }
        [ForeignKey("userId")]
        public ApplicationUser applicationUser { get; set; }

        public ICollection<MainComment> mainComments { get; set; }
        public ICollection<Source> sources { get; set; }
        public ICollection<PostTagId> postTagIds { get; set; }
        #endregion

        public int views { get; set; }
    }
}
