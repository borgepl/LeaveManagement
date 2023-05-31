using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace LeaveManagement.Web.Services
{
    public class EmailDummySender : IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            return Task.CompletedTask;
        }
    }
}