using System;
using System.Configuration;
using System.Reflection;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Configuration for payments.
    /// </summary>
    public class PaymentConfiguration : ConfigurationSection, IPaymentConfiguration
    {
        #region Private variables

        private static IPaymentConfiguration _paymentConfiguration;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Methods

        /// <summary>
        /// Creates and initialize the membership configuration.
        /// </summary>
        /// <returns>Created and intialized membership configuration.</returns>
        public static IPaymentConfiguration Create()
        {
            try
            {
                lock (SyncRoot)
                {
                    if (_paymentConfiguration != null)
                    {
                        return _paymentConfiguration;
                    }
                    _paymentConfiguration = (IPaymentConfiguration) ConfigurationManager.GetSection("paymentConfiguration");
                    return _paymentConfiguration;
                }
            }
            catch (Exception ex)
            {
                throw new ReduceFoodWasteRepositoryException(ex.Message, MethodBase.GetCurrentMethod(), ex);
            }
        }

        #endregion
    }
}