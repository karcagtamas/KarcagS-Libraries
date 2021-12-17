using System.Threading.Tasks;

namespace Karcags.Common.Tools.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(Mail mail);
    }
}