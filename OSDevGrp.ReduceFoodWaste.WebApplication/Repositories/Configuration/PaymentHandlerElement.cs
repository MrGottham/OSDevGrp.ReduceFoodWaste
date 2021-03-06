﻿using System;
using System.Configuration;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Configuration of a data provider who can handle payments.
    /// </summary>
    public class PaymentHandlerElement : ConfigurationElement, IPaymentHandlerElement
    {
        #region Properties

        /// <summary>
        /// Gets the identifier for the data provider who can handle payments.
        /// </summary>
        [ConfigurationProperty("id", DefaultValue = "{00000000-0000-0000-0000-000000000000}", IsRequired = true, IsKey = true)]
        public Guid Identifier => (Guid) this["id"];

        /// <summary>
        /// Gets the action name which starts the payment process handled by the data provider.
        /// </summary>
        [ConfigurationProperty("action", DefaultValue = "", IsRequired = true)]
        public string ActionName => (string) this["action"];

        /// <summary>
        /// Gets the path to the image which describes the data provider who can handle payments.
        /// </summary>
        [ConfigurationProperty("image", DefaultValue = "", IsRequired = true)]
        public string ImagePath => (string) this["image"];

        #endregion
    }
}