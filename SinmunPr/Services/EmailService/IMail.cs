using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinmunPr.Services
{
    public interface IMail
    {
        public void SendMail(string subject, string body, string receiver);
    }
}
