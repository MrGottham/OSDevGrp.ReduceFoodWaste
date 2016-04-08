using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions
{
    /// <summary>
    /// Base exception used by the Reduce Food Waste Web Application.
    /// </summary>
    [Serializable]
    public abstract class ReduceFoodWasteExceptionBase : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates an instance of a the base exception used by the Reduce Food Waste Web Application.
        /// </summary>
        /// <param name="message">Message.</param>
        protected ReduceFoodWasteExceptionBase(string message)
            : base(message)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
        }

        /// <summary>
        /// Creates an instance of a the base exception used by the Reduce Food Waste Web Application.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="innerException">Inner exception.</param>
        protected ReduceFoodWasteExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }
            if (innerException == null)
            {
                throw new ArgumentNullException("innerException");
            }
        }

        #endregion
    }
}