using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Models
{
    public class PostTagId
    {
        public int id { get; set; }

        #region ForeignKeys
        public int postId { get; set; }
        [ForeignKey("postId")]
        public Post Post { get; set; }
        
        public int tagId { get; set; }
        [ForeignKey("tagId")]
        public Tag Tag { get; set; }
        #endregion
    }
}
