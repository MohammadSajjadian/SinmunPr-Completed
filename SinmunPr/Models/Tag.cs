using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Models
{
    public class Tag
    {
        public int id { get; set; }

        #region TagInfo
        public string Pname { get; set; }
        public string Ename { get; set; }
        #endregion

        public ICollection<PostTagId> postTagIds { get; set; }
    }
}
