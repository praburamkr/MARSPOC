using Common.Notification;
using NotificationWebApi.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace NotificationWebApi.Email
{
    public class EmailNotificationProxy
    {
        private NotificationHubConfiguration _configuration;

        string smtpAddress, emailPortNumber, sendFromEmail, sendFromPassword, emailAttachmetnPath;

        public EmailNotificationProxy(NotificationHubConfiguration configuration)
        {
            _configuration = configuration;

            smtpAddress = _configuration.SMTPAddress.ToString();
            emailPortNumber = _configuration.EmailPortNumber.ToString();
            sendFromEmail = _configuration.SendFromEmail.ToString();
            sendFromPassword = _configuration.SendFromPassword.ToString();
            emailAttachmetnPath = _configuration.EmailAttachmetnPath.ToString();
        }


        public EmailNotificationResponsecs SendEmail(Common.Notification.SendMailRequest emailModel)
        {
            EmailNotificationResponsecs outcome = new EmailNotificationResponsecs();

            try
            {
                var client = new SmtpClient(smtpAddress, 587)
                {
                    Credentials = new NetworkCredential(sendFromEmail, sendFromPassword),
                    EnableSsl = true
                };

                MailMessage mail = new MailMessage(sendFromEmail, emailModel.replyto, emailModel.subject, emailModel.body);
                mail.IsBodyHtml = true;
                if (!string.IsNullOrWhiteSpace(emailModel.cc))
                    mail.CC.Add(emailModel.cc);

                client.Send(mail);

                outcome.Message = "Email has been successfully sent!!";
                outcome.CompletedWithSuccess = true;
                return outcome;
            }
            catch (Exception ex)
            {
                outcome.Message = "There is an issue in sending the Email, Please contact administrator!! : " + ex.Message;
                outcome.CompletedWithSuccess = false;
                return outcome;
            }
        }
    }
}
