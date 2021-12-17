using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Karcags.Common.Tools.Email
{
    /// <summary>
    /// Mail Service
    /// </summary>
    public class MailService : IMailService
    {
        private readonly EmailSettings _settings;

        /// <summary>
        /// Mail Service init
        /// </summary>
        /// <param name="settings">Mail Settings</param>
        public MailService(EmailSettings settings)
        {
            this._settings = settings;
        }

        /// <summary>
        /// Send email async
        /// </summary>
        /// <param name="mail">Mail object</param>
        /// <returns>Task</returns>
        public async Task SendEmailAsync(Karcags.Common.Tools.Email.Mail mail)
        {
            var message = new MailMessage();

            var smtp = new SmtpClient();

            // From
            message.From = new MailAddress(this._settings.Mail, this._settings.DisplayName);

            // To
            foreach (var to in mail.ToList)
            {
                message.To.Add(to.GetMailAddress());
            }

            // CC
            foreach (var cc in mail.CcList)
            {
                message.CC.Add(cc.GetMailAddress());
            }

            // BCC
            foreach (var bcc in mail.BccList)
            {
                message.Bcc.Add(bcc.GetMailAddress());
            }

            // Subject
            message.Subject = mail.Subject;

            // Attachments
            foreach (var attachment in mail.Attachments)
            {
                if (attachment.Length > 0)
                {
                    await using var ms = new MemoryStream();
                    await attachment.CopyToAsync(ms);
                    byte[] fileBytes = ms.ToArray();
                    var att = new Attachment(new MemoryStream(fileBytes), attachment.FileName);
                    message.Attachments.Add(att);
                }
            }

            // Body
            message.IsBodyHtml = true;
            message.Body = mail.Body;

            // Config
            smtp = this.ConfigSmpt(smtp);

            // Send
            await smtp.SendMailAsync(message);
        }

        /// <summary>
        /// Config SMTP client
        /// </summary>
        /// <param name="smtp">Smtp</param>
        /// <returns>Smtp</returns>
        private SmtpClient ConfigSmpt(SmtpClient smtp)
        {
            smtp.Port = this._settings.Port;
            smtp.Host = this._settings.Host;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(this._settings.Mail, this._settings.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            return smtp;
        }
    }
}