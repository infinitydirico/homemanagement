using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeManagement.Api.Core.Email
{
    public interface IEmailService
    {
        Task Send(string from, List<string> to, string subject, string plainContent, string htmlContent);

        Task Send(string from, List<string> to, string subject, string plainContent, string htmlContent, string filename, string content);

        Task Send(string from, List<string> to, string subject, string plainContent, string htmlContent, List<Attachment> attachments);
    }
}
