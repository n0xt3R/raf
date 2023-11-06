using bfws.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace bfws.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private static SmtpConfig _config;

        public EmailSender() { }
        private EmailSender(SmtpConfig config)
        {
            _config = config;
        }

        public static EmailSender NewInstance(IConfiguration config)
        {
            SmtpConfig smtpConfig = new SmtpConfig
            {
                Name = config.GetValue<string>("Name"),
                User = config.GetValue<string>("User"),
                Pass = config.GetValue<string>("Pass"),
                Server = config.GetValue<string>("Server"),
                Port = config.GetValue<int>("Port")
            };
            return new EmailSender(smtpConfig);

        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient client = new SmtpClient(_config.Server, _config.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config.User, _config.Pass),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,

            };
            MailAddress from = new MailAddress(_config.User, _config.Name, System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(email);
            MailMessage msg = new MailMessage(from, to)
            {
                Subject = subject,
                SubjectEncoding = System.Text.Encoding.UTF8,
                Body = message,
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true
            };
            try
            {
                client.Send(msg);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // Clean up.
                msg.Dispose();
            }
            return Task.CompletedTask;
        }
    }
}
