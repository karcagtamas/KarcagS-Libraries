using System;

namespace Karcags.Common.Tools
{
    /// <summary>
    /// Own Generated Message Exception
    /// </summary>
    public class MessageException : Exception
    {
        /// <summary>
        /// Empty init
        /// </summary>
        public MessageException()
        {
        }

        /// <summary>
        /// Exception with message
        /// </summary>
        /// <param name="msg">Exception message</param>
        public MessageException(string msg) : base(msg)
        {
        }

        public MessageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}