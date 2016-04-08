using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions
{
    /// <summary>
    /// Business exception used by the Reduce Food Waste Web Application.
    /// </summary>
    public class ReduceFoodWasteBusinessException : ReduceFoodWasteExceptionBase
    {
        #region Constructors

        /// <summary>
        /// Creates a business exception used by the Reduce Food Waste Web Application.
        /// </summary>
        /// <param name="message">Message.</param>
        public ReduceFoodWasteBusinessException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Creates a business exception used by the Reduce Food Waste Web Application.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        public ReduceFoodWasteBusinessException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        #endregion
    }
}