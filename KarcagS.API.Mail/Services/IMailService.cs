namespace KarcagS.API.Mail.Services;

public interface IMailService
{
    Task SendEmailAsync(Models.Mail mail);
}
