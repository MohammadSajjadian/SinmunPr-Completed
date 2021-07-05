using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SinmunPr.Services
{
    public class EmailProcess : IMail
    {
        public void SendMail(string subject, string body, string receiver)
        {
            MailMessage mailMessage = new("rexongame1@gmail.com", receiver);
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("rexongame1@gmail.com", "RexonGame_1100");

            smtpClient.Send(mailMessage);
        }
    }
}
