using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Models
{
    public class ContactUs
    {
        public int id { get; set; }

        #region Attributes

        public string name { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public string content { get; set; }

        #endregion

        public DateTime createTime { get; set; }
    }
}
