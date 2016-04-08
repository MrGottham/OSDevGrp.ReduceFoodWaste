using System;
using System.Reflection;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions
{
    /// <summary>
    /// Repository exception used by the Reduce Food Waste Web Application.
    /// </summary>
    [Serializable]
    public class ReduceFoodWasteRepositoryException : ReduceFoodWasteExceptionBase
    {
        #region Private variables

        private readonly MethodBase _repositoryMethod;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a repository exception used by the Reduce Food Waste Web Application.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="repositoryMethod">Repository method where the exception occurs.</param>
        public ReduceFoodWasteRepositoryException(string message, MethodBase repositoryMethod) 
            : base(message)
        {
            if (repositoryMethod == null)
            {
                throw new ArgumentNullException("repositoryMethod");
            }
            _repositoryMethod = repositoryMethod;
        }

        /// <summary>
        /// Creates a repository exception used by the Reduce Food Waste Web Application.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="repositoryMethod">Repository method where the exception occurs.</param>
        /// <param name="innerException">Inner exception.</param>
        public ReduceFoodWasteRepositoryException(string message, MethodBase repositoryMethod, Exception innerException) 
            : base(message, innerException)
        {
            if (repositoryMethod == null)
            {
                throw new ArgumentNullException("repositoryMethod");
            }
            _repositoryMethod = repositoryMethod;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name for the method where the exception occurred.
        /// </summary>
        public string MethodName
        {
            get { return _repositoryMethod.Name; }
        }

        /// <summary>
        /// Gets the name for the repository where the exception occurred.
        /// </summary>
        public string RepositoryName
        {
            get { return _repositoryMethod.ReflectedType.Name; }
        }

        #endregion
    }
}