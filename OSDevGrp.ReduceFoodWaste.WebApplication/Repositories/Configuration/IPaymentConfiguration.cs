using System;
using System.Collections.Generic;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Interface for the configuration for payments.
    /// </summary>
    public interface IPaymentConfiguration
    {
        /// <summary>
        /// Gets the configuration for all the data providers who can handle payments.
        /// </summary>
        IEnumerable<IPaymentHandlerElement> PaymentHandlers { get; }

        /// <summary>
        /// Gets the configuration for a given data provider who can handle payments.
        /// </summary>
        /// <param name="dataProviderIdentifier">Identifier for the data provider who can handle payments.</param>
        /// <returns>Configuration for a given data provider who can handle payments.</returns>
        IPaymentHandlerElement GetPaymentHandler(Guid dataProviderIdentifier);
    }
}
