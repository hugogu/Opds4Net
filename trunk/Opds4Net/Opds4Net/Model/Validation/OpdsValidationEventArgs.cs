using System;

namespace Opds4Net.Model.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsValidationEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public OpdsValidationErrorLevel ErrorLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="source"></param>
        public OpdsValidationEventArgs(OpdsValidationErrorLevel level, string message, string source = null)
        {
            ErrorLevel = level;
            Message = message;
            Source = source;
        }
    }
}
