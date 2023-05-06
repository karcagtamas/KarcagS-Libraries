using Microsoft.AspNetCore.Http;

namespace KarcagS.API.Mail.Models;

/// <summary>
/// Mail object
/// </summary>
public class Mail
{
    public List<MailRecipient> ToList { get; set; }
    public List<MailRecipient> CcList { get; set; }
    public List<MailRecipient> BccList { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public List<IFormFile> Attachments { get; set; }

    /// <summary>
    /// Mail init.
    /// Init lists
    /// </summary>
    public Mail()
    {
        ToList = new List<MailRecipient>();
        CcList = new List<MailRecipient>();
        BccList = new List<MailRecipient>();
        Attachments = new List<IFormFile>();
    }

    /// <summary>
    /// Add To element
    /// </summary>
    /// <param name="address">E-mail address</param>
    /// <param name="displayName">Display name</param>
    public void AddTo(string address, string displayName) => ToList.Add(new MailRecipient(address, displayName));


    /// <summary>
    /// Add To element
    /// </summary>
    /// <param name="person">Person object</param>
    /// <param name="emailGetter">Email getter</param>
    /// <param name="nameGetter">Name getter</param>
    public void AddTo<T>(T person, Func<T, string> emailGetter, Func<T, string> nameGetter) => ToList.Add(new MailRecipient(emailGetter(person), nameGetter(person)));


    /// <summary>
    /// Add CC element
    /// </summary>
    /// <param name="address">E-mail address</param>
    /// <param name="displayName">Display name</param>
    public void AddCc(string address, string displayName) => CcList.Add(new MailRecipient(address, displayName));


    /// <summary>
    /// Add CC element
    /// </summary>
    /// <param name="person">Person object</param>
    /// <param name="emailGetter">Email getter</param>
    /// <param name="nameGetter">Name getter</param>
    public void AddCc<T>(T person, Func<T, string> emailGetter, Func<T, string> nameGetter) => CcList.Add(new MailRecipient(emailGetter(person), nameGetter(person)));


    /// <summary>
    /// Add BCC element
    /// </summary>
    /// <param name="address">E-mail address</param>
    /// <param name="displayName">Display name</param>
    public void AddBcc(string address, string displayName) => BccList.Add(new MailRecipient(address, displayName));


    /// <summary>
    /// Add BCC element
    /// </summary>
    /// <param name="person">Person object</param>
    /// <param name="emailGetter">Email getter</param>
    /// <param name="nameGetter">Name getter</param>
    public void AddBcc<T>(T person, Func<T, string> emailGetter, Func<T, string> nameGetter) => BccList.Add(new MailRecipient(emailGetter(person), nameGetter(person)));

}
