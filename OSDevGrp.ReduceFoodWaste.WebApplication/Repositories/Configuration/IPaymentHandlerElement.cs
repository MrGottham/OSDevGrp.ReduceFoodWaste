using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Interface for the configuration of a data provider who can handle payments.
    /// </summary>
    public interface IPaymentHandlerElement
    {
        /// <summary>
        /// Gets the identifier for the data provider who can handle payments.
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Gets the path to the image which describes the data provider who can handle payments.
        /// </summary>
        string ImagePath { get; }
    }
}
