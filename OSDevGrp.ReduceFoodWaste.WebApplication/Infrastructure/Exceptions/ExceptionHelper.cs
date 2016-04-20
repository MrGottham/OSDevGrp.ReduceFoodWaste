using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions
{
    /// <summary>
    /// Helper functionality for exceptions.
    /// </summary>
    public static class ExceptionHelper
    {
        #region Methods

        /// <summary>
        /// Creates and returns an exception used by the Reduce Food Waste Web Application from a given aggregate exception.
        /// </summary>
        /// <param name="aggregateException">Aggregate exception from which a new exception used by the Reduce Food Waste Web Application should be rethrown.</param>
        /// <returns>Exception used by the Reduce Food Waste Web Application from a given aggregate exception.</returns>
        public static ReduceFoodWasteExceptionBase ToReduceFoodWasteException(this AggregateException aggregateException)
        {
            if (aggregateException == null)
            {
                throw new ArgumentNullException("aggregateException");
            }

            ReduceFoodWasteExceptionBase exceptionToThrow = null;
            aggregateException.Handle(exception =>
            {
                exceptionToThrow = exception as ReduceFoodWasteExceptionBase;
                if (exceptionToThrow != null)
                {
                    return true;
                }
                exceptionToThrow = new ReduceFoodWasteSystemException(exception.Message, exception);
                return true;
            });
            throw exceptionToThrow;
        }

        #endregion
    }
}