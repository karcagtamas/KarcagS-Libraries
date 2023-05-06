using System.Net;
using System.Net.Mail;
using KarcagS.API.Mail.Configurations;
using Microsoft.Extensions.Options;

namespace KarcagS.API.Mail.Services;

/// <summary>
/// Mail Service
/// </summary>
public class MailService : IMailService
{
    private readonly EmailSettings _settings;

    /// <summary>
    /// Mail Service init
    /// </summary>
    /// <param name="emailOptions">E-mail options</param>
    public MailService(IOptions<EmailSettings> emailOptions)
    {
        _settings = emailOptions.Value;
    }

    /// <summary>
    /// Send email async
    /// </summary>
    /// <param name="mail">Mail object</param>
    /// <returns>Task</returns>
    public async Task SendEmailAsync(Models.Mail mail)
    {
        var message = new MailMessage();

        var smtp = new SmtpClient();

        // From
        message.From = new MailAddress(_settings.Mail, _settings.DisplayName);

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
        smtp = ConfigSMTP(smtp);

        // Send
        await smtp.SendMailAsync(message);
    }

    /// <summary>
    /// Config SMTP client
    /// </summary>
    /// <param name="smtp">Smtp</param>
    /// <returns>Smtp</returns>
    private SmtpClient ConfigSMTP(SmtpClient smtp)
    {
        smtp.Port = _settings.Port;
        smtp.Host = _settings.Host;
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_settings.Mail, _settings.Password);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        return smtp;
    }
}