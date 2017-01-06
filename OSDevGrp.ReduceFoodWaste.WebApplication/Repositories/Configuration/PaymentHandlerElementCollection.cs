using System;
using System.Configuration;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Collection of configuration for one or more data providers who can handle payments.
    /// </summary>
    public class PaymentHandlerElementCollection : ConfigurationElementCollection
    {
        #region Properties

        /// <summary>
        /// Gets the type of the ConfigurationElementCollection.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

        /// <summary>
        /// Gets the payment handler configuration element for a data provider with a given identification.
        /// </summary>
        /// <param name="id">Identification for the payment handler configuration element to get.</param>
        /// <returns>Payment handler configuration element for the data provider with the given identification.</returns>
        public new IPaymentHandlerElement this[string id]
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentNullException(nameof(id));
                }
                return (IPaymentHandlerElement) BaseGet(id);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of a payment handler configuration element for a data provider who can handle payments.
        /// </summary>
        /// <returns>New instance of a payment handler configuration element for a data provider who can handle payments.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new PaymentHandlerElement();
        }

        /// <summary>
        /// Gets the key for a given payment handler configuration element for a data provider who can handle payments.
        /// </summary>
        /// <param name="element">Payment handler configuration element for a data provider who can handle payments.</param>
        /// <returns>Key for the given payment handler configuration element for a data provider who can handle payments.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return ((PaymentHandlerElement) element).Identifier;
        }

        #endregion
    }
}