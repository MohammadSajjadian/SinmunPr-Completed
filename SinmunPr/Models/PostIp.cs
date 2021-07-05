using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SinmunPr.Models
{
    public class PostIp
    {
        public int id { get; set; }
        public int postId { get; set; }

        public string IP { get; set; }
    }
}
