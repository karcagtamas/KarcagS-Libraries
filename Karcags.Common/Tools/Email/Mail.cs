using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Karcags.Common.Tools.Email
{
    /// <summary>
    /// Mail object
    /// </summary>
    public class Mail
    {
        public List<MailRecipient> ToList { get; set; }
        public List<MailRecipient> CcList { get; set; }
        public List<MailRecipient> BccList { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }

        /// <summary>
        /// Mail init.
        /// Init lists
        /// </summary>
        public Mail()
        {
            this.ToList = new List<MailRecipient>();
            this.CcList = new List<MailRecipient>();
            this.BccList = new List<MailRecipient>();
            this.Attachments = new List<IFormFile>();
        }

        /// <summary>
        /// Add To element
        /// </summary>
        /// <param name="address">E-mail address</param>
        /// <param name="displayName">Display name</param>
        public void AddTo(string address, string displayName)
        {
            this.ToList.Add(new MailRecipient(address, displayName));
        }

        /// <summary>
        /// Add To element
        /// </summary>
        /// <param name="person">Person object</param>
        /// <param name="emailGetter">Email getter</param>
        /// <param name="nameGetter">Name getter</param>
        public void AddTo<T>(T person, Func<T, string> emailGetter, Func<T, string> nameGetter)
        {
            this.ToList.Add(new MailRecipient(emailGetter(person), nameGetter(person)));
        }

        /// <summary>
        /// Add CC element
        /// </summary>
        /// <param name="address">E-mail address</param>
        /// <param name="displayName">Display name</param>
        public void AddCc(string address, string displayName)
        {
            this.CcList.Add(new MailRecipient(address, displayName));
        }

        /// <summary>
        /// Add CC element
        /// </summary>
        /// <param name="person">Person object</param>
        /// <param name="emailGetter">Email getter</param>
        /// <param name="nameGetter">Name getter</param>
        public void AddCc<T>(T person, Func<T, string> emailGetter, Func<T, string> nameGetter)
        {
            this.CcList.Add(new MailRecipient(emailGetter(person), nameGetter(person)));
        }

        /// <summary>
        /// Add BCC element
        /// </summary>
        /// <param name="address">E-mail address</param>
        /// <param name="displayName">Display name</param>
        public void AddBcc(string address, string displayName)
        {
            this.BccList.Add(new MailRecipient(address, displayName));
        }

        /// <summary>
        /// Add BCC element
        /// </summary>
        /// <param name="person">Person object</param>
        /// <param name="emailGetter">Email getter</param>
        /// <param name="nameGetter">Name getter</param>
        public void AddBcc<T>(T person, Func<T, string> emailGetter, Func<T, string> nameGetter)
        {
            this.BccList.Add(new MailRecipient(emailGetter(person), nameGetter(person)));
        }
    }
}