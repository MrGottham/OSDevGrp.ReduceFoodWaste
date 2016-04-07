using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions
{
    /// <summary>
    /// System exception used by the Reduce Food Waste Web Application.
    /// </summary>
    [Serializable]
    public class ReduceFoodWasteSystemException : ReduceFoodWasteExceptionBase
    {
        #region Methods

        /// <summary>
        /// Creates a system exception used by the Reduce Food Waste Web Application.
        /// </summary>
        /// <param name="message">Message.</param>
        public ReduceFoodWasteSystemException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Creates a system exception used by the Reduce Food Waste Web Application.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ReduceFoodWasteSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion
    }
}