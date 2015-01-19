using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using Elasticsearch.Net;
using Microsoft.AspNet.Identity;
using S22.Imap;

namespace Microbrewit.Api.Util
{
    public class EmailService : IIdentityMessageService
    {
        private static readonly string MailPassword = ConfigurationManager.AppSettings["mailPassword"];
        public Task SendAsync(IdentityMessage message)
        {
            return sendMail(message);
        }

        public async Task sendMail(IdentityMessage message)
        {
            #region formatter
           string text = string.Format("Please click on this link to {0}: {1}", message.Subject, message.Body);
          string html = message.Body + "<br/>";

           html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser:" + message.Body);
            #endregion

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("microbrewit@asphaug.io","Microbrew.it");
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            SmtpClient smtpClient = new SmtpClient("mail.gandi.net", Convert.ToInt32(587));

            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("microbrewit@asphaug.io", MailPassword);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
            using (ImapClient Client = new ImapClient("access.mail.gandi.net", 993,
             "microbrewit@asphaug.io", MailPassword, AuthMethod.Login, true))
            {
                Client.StoreMessage(msg, true, "Sent");
            }

        }
    }
}