using MailKit.Net.Smtp;
using MC.Email.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MC.Email
{
    public class EmailProvider : IEmailProvider
    {
        private readonly EmailClientConfig configuration;
        public EmailProvider(IOptions<EmailClientConfig> config)
        {
            configuration = config.Value;
        }

        public async Task<bool> SendEmailAsPlain(string To, string Content) {

            try
            {

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(configuration.FromAddress));
                mimeMessage.To.Add(new MailboxAddress(To));
                mimeMessage.Subject = configuration.Subject;
                mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
                {
                    Text = Content
                };
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(configuration.Host, configuration.Port, true);
                    client.Authenticate(configuration.Username, configuration.Password);

                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch {
                return false;
            }

        }

        public async Task<bool> SendEmailAsHtml(string To, string Content)
        {

            try
            {

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(configuration.FromAddress));
                mimeMessage.To.Add(new MailboxAddress(To));
                mimeMessage.Subject = configuration.Subject;
                mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = Content
                };
                using (SmtpClient client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(configuration.Host, configuration.Port, false);
                    client.Authenticate(configuration.Username, configuration.Password);

                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

   

    }
}
