using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SinmunPr.Models;

namespace SinmunPr.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        #region Images
        
        public byte[] smallAvatar { get; set; }
        public byte[] bigAvatar { get; set; }

        #endregion

        #region Attributes

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string bio { get; set; }

        #endregion

        #region DateTimes

        public DateTime accountCrationTime { get; set; } = DateTime.Now;
        public DateTime registerTokenCreationTime { get; set; }
        public DateTime ressPassTokenCreationTime { get; set; }

        #endregion

        #region Relations

        ICollection<Post> posts { get; set; }
        ICollection<MainComment> mainComments { get; set; }
        ICollection<SubComment> subComments { get; set; }
        
        #endregion
    }
}
