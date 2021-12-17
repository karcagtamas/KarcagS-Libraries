using System.Net.Mail;

namespace Karcags.Common.Tools.Email
{
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
            this.EmailAddress = address;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Get mail address object from mail recipient
        /// </summary>
        /// <returns></returns>
        public MailAddress GetMailAddress()
        {
            return new MailAddress(this.EmailAddress, this.DisplayName);
        }
    }
}