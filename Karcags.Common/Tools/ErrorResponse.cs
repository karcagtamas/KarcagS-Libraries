using System;

namespace Karcags.Common.Tools
{
    /// <summary>
    /// Error response model
    /// </summary>
    public class ErrorResponse
    {
        private string Message { get; set; }
        private ErrorResponse Inner { get; set; }
        private string StackTrace { get; set; }

        /// <summary>
        /// Error response from Exception
        /// </summary>
        /// <param name="e">Exception</param>
        public ErrorResponse(Exception e)
        {
            this.Message = e.Message;
            if (e.InnerException != null)
            {
                this.Inner = new ErrorResponse(e.InnerException);
            }

            if (e.StackTrace != null)
            {
                this.StackTrace = e.StackTrace;
            }
        }
    }
}