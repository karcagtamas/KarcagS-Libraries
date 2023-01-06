using System.Net.Mail;

namespace KarcagS.Common.Tools.Email;

/// <summary>
/// E-mail recipient
/// </summary>
public class MailRecipient
{
    private string EmailAddress { get; set; }
    private string DisplayName { get; set; }

    /// <summary>
    /// E-mail recipient init
    /// </summary>
    /// <param name="address">E-mail address</param>
    /// <param name="displayName">Display name</param>
    public MailRecipient(string address, string displayName)
    {
        EmailAddress = address;
        DisplayName = displayName;
    }

    /// <summary>
    /// Get mail address object from mail recipient
    /// </summary>
    /// <returns></returns>
    public MailAddress GetMailAddress() => new MailAddress(EmailAddress, DisplayName);

}
