using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeManagement.Api.Core.Email
{
    public class EmailService : IEmailService
    {
        private readonly string apiKey;
        private readonly SendGridClient sendGridClient;

        public EmailService(IConfiguration configuration)
        {
            apiKey = configuration.GetSection("SendGrid").Value;
            sendGridClient = new SendGridClient(apiKey);
        }

        public async Task Send(string from, List<string> to, string subject, string plainContent, string htmlContent)
        {
            var msg = CreateMessage(from, to, subject, plainContent, htmlContent);

            if (to.Any()) throw new Exception($"No receiver was provided.");

            if (to.Count > 1) msg.AddTos(to.Select(t => new EmailAddress(t)).ToList());
            else msg.AddTo(to.First());

            var response = await sendGridClient.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"There was an error while trying to send an email");
        }

        public async Task Send(string from, List<string> to, string subject, string plainContent, string htmlContent, List<Attachment> attachments)
        {
            var msg = CreateMessage(from, to, subject, plainContent, htmlContent, attachments);

            if (to.Any()) throw new Exception($"No receiver was provided.");

            if (to.Count > 1) msg.AddTos(to.Select(t => new EmailAddress(t)).ToList());
            else msg.AddTo(to.First());

            var response = await sendGridClient.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.OK) throw new Exception($"There was an error while trying to send an email");
        }

        public async Task Send(string from, List<string> to, string subject, string plainContent, string htmlContent, string filename, string content)
        {
            var msg = CreateMessage(from, to, subject, plainContent, htmlContent);

            msg.AddAttachment(filename, content);

            if (!to.Any()) throw new Exception($"No receiver was provided.");

            if (to.Count > 1) msg.AddTos(to.Select(t => new EmailAddress(t)).ToList());
            else msg.AddTo(to.First());

            var response = await sendGridClient.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.Accepted) throw new Exception($"There was an error while trying to send an email");
        }

        private SendGridMessage CreateMessage(string from, List<string> to, string subject, string plainContent, string htmlContent)
        {
            return new SendGridMessage
            {
                From = new EmailAddress(from),
                Subject = subject,
                PlainTextContent = plainContent,
                HtmlContent = htmlContent
            };
        }

        private SendGridMessage CreateMessage(string from, List<string> to, string subject, string plainContent, string htmlContent, List<Attachment> attachments)
        {
            return new SendGridMessage
            {
                From = new EmailAddress(from),
                Subject = subject,
                PlainTextContent = plainContent,
                HtmlContent = htmlContent,
                Attachments = attachments
            };
        }
    }
}
