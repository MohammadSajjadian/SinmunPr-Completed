using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Models
{
    public class Category
    {
        public int id { get; set; }

        #region CategoriesInfo
        public string Pname { get; set; }
        public string Ename { get; set; }
        #endregion

        public bool IsInMainPage { get; set; }

        public ICollection<Post> posts { get; set; }
    }
}
