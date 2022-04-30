using System;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace Services.Implementations
{
    public class MailService : IMailService
    {
        private const string SmtpGmail = Constants.SmtpGmail;
        private const int SmtpGmailPort = Constants.SmtpGmailPort;
        private const string SmtpGmailLogin = Constants.SmtpGmailLogin;
        private const string SmtpGmailPassword = Constants.SmtpGmailPassword;
        private const bool SslEnable = Constants.SslEnable;
        private static readonly MailAddress _from = new (Constants.SmtpGmailLogin, Constants.MailAuthor);

        private readonly ILogger<MailService> _logger;

        public MailService(ILogger<MailService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendMailAsync(string to, string subject, string message)
        {
            try
            {
                var smtp = new SmtpClient(SmtpGmail, SmtpGmailPort)
                {
                    Credentials = new NetworkCredential(SmtpGmailLogin, SmtpGmailPassword),
                    EnableSsl = SslEnable
                };

                var toEmail = new MailAddress(to);
                var mailMessage = new MailMessage(_from, toEmail)
                {
                    Body = message,
                    BodyEncoding = Encoding.UTF8,
                    Subject = subject,
                    SubjectEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(mailMessage);
                mailMessage.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception while trying to send email");
                return false;
            }
        }

        public async Task<bool> SendHtmlEmailAsync(string to, string subject, string templateName, params string[] parameters)
        {
            try
            {
                var template = await File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(), "..") + templateName);

                var content = string.Format(template, parameters);

                return await SendMailAsync(to, subject, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught exception while trying to send email with template");
                return false;
            }
        }
    }
}
