using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Microbrewit.Api.Util
{
    public static class MailHelper
    {
        private static string  mailPassword = ConfigurationManager.AppSettings["mailPassword"];
        public static void SendMail(string subject, string content, string receiver)
        {
            SmtpClient smtpClient = new SmtpClient("mail.gandi.net", 587);

            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("microbrewit@asphaug.io", mailPassword);

            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("microbrewit@asphaug.io", "Microbrew.it");
            mail.To.Add(new MailAddress(receiver));
            //mail.CC.Add(new MailAddress("MyEmailID@gmail.com"));
            mail.Subject = subject;
            mail.Body = content;

            smtpClient.Send(mail);
        }
    }
}