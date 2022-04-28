using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Services.Implementations
{
    public class MailService
    {
        private static readonly MailAddress _from = new MailAddress("onlymovieslnu@gmail.com", "OnlyMovies2 Team");
        private static readonly string SmtpGmail = "smtp.gmail.com";
        private static readonly int SmtpGmailPort = 587;
        private static readonly string SmtpGmailLogin = "onlymovieslnu@gmail.com";
        private static readonly string SmtpGmailPassword = "OnlyMovies2022";
        private static readonly bool SslEnable = true;
        private static bool mailSent = false;

        public bool SendMail(string to, string message, string sub)
        {
            try
            {
                var toEmail = new MailAddress(to);
                var smtp = new SmtpClient(SmtpGmail, SmtpGmailPort);
                smtp.Credentials = new NetworkCredential(SmtpGmailLogin, SmtpGmailPassword);
                smtp.EnableSsl = SslEnable;
                smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                string userState = "Orc massage";
                var mailMessage = new MailMessage(_from, toEmail);
                mailMessage.Body = message;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Subject = sub;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                smtp.SendAsync(mailMessage, userState);
                mailMessage.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            string token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }

            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }

            mailSent = true;
        }
    }
}
