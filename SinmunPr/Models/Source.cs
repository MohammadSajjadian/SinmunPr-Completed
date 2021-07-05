using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Models
{
    public class Source
    {
        public int id { get; set; }

        #region SourceInfo
        public string name { get; set; }
        public string link { get; set; }
        #endregion

        public int postId { get; set; }
        [ForeignKey("postId")]
        public Post post { get; set; }
    }
}
