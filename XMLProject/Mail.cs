using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace XMLProject
{
    internal class Mail
    {

        private static readonly string smtpServer = "smtp.gmail.com";
        private static readonly int smtpPort = 587; 
        private static readonly string smtpUser = "********@gmail.com"; 
        private static readonly string smtpPass = "sxbo szzn dxxv dmmn"; 

        public static void SendEmail(string subject, string body)
        {
            try
            {
                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtpClient.EnableSsl = true; 

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpUser),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false 
                    };
                    mailMessage.To.Add("**********@gmail.com"); 

                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public static string CreateEmailBody(string processName, int processId, string computerName, string userName, string ipAddress, string status)
        {
            return $"Application Name: {processName}\n" +
                   $"Process ID: {processId}\n" +
                   $"Computer Name: {computerName}\n" +
                   $"User Name: {userName}\n" +
                   $"IP Address: {ipAddress}\n" +
                   $"Status: {status}";
        }
    }
}
