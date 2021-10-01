using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Praktyki2021.Models
{
    public static class MailService
    {
        public static void SendMail(string to, string subject, string body)
        {
            var message = new MailMessage(ConfigurationManager.AppSettings["sender"], to)
            {
                Subject = subject,
                Body = body
            };
            var smtpClient = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["smtpHost"],
                Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["sender"],
                    ConfigurationManager.AppSettings["passwd"]
                    ),
                EnableSsl = true
            };
            smtpClient.Send(message);
        }
    }
}