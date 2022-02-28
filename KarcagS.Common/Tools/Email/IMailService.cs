namespace KarcagS.Common.Tools.Email;

public interface IMailService
{
    Task SendEmailAsync(Mail mail);
}
