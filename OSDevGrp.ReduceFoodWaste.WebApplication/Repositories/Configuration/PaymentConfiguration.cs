using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

        #region Properties

        /// <summary>
        /// Gets the collection of configuration for one or more data providers who can handle payments.
        /// </summary>
        [ConfigurationProperty("paymentHandlers", IsDefaultCollection = false, IsRequired = true)]
        [ConfigurationCollection(typeof(PaymentHandlerElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public PaymentHandlerElementCollection PaymentHandlerElements => (PaymentHandlerElementCollection) base["paymentHandlers"];

        /// <summary>
        /// Gets the configuration for all the data providers who can handle payments.
        /// </summary>
        public IEnumerable<IPaymentHandlerElement> PaymentHandlers => PaymentHandlerElements.Cast<IPaymentHandlerElement>().ToList();

        #endregion

        #region Methods

        /// <summary>
        /// Gets the configuration for a given data provider who can handle payments.
        /// </summary>
        /// <param name="dataProviderIdentifier">Identifier for the data provider who can handle payments.</param>
        /// <returns>Configuration for a given data provider who can handle payments.</returns>
        public IPaymentHandlerElement GetPaymentHandler(Guid dataProviderIdentifier)
        {
            return PaymentHandlers.SingleOrDefault(paymentHandler => paymentHandler.Identifier == dataProviderIdentifier);
        }

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