using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Payments
{
    /// <summary>
    /// Interface for a provider who can handle payments.
    /// </summary>
    public interface IPaymentHandler
    {
        /// <summary>
        /// Gets the identifier of provider who can handle payments.
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// Gets the name of the provider who can handle payment.
        /// </summary>
        string Name { get; }
    }
}
