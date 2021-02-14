using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace ge_repository.services
{
    public class EmailSender : IEmailSender
    {
       public EmailSender(IOptions<smpt_config> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public smpt_config Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options, subject, message, email);
        }

        public Task Execute(smpt_config options, string subject, string message, string email)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(options.from, options.from));
            mimeMessage.To.Add(new MailboxAddress(email, email));
            mimeMessage.Subject = subject;

            mimeMessage.Body = new TextPart("html")
            {
                Text = message
            };


            SmtpClient client = new SmtpClient();
            
            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
         //   client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            client.Connect(options.server, options.port,  false);

            // Note: only needed if the SMTP server requires authentication
            client.Authenticate(options.smtpUsername, options.smtpPassword);

            client.Send(mimeMessage);
            client.Disconnect(true);
            client.Dispose();

            return Task.FromResult(0);
        }
    }

public class smpt_config
    {
        public int port { get; set; }
        public string server { get; set; }
        public string smtpUsername { get; set; }
        public string smtpPassword { get; set; }
        public string from { get; set; }
    }

 /*    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
    */
}




