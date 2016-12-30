using System.Globalization;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Payments
{
    /// <summary>
    /// Interface for a object which are payable.
    /// </summary>
    public interface IPayable
    {
        /// <summary>
        /// Gets the billing information for the payment.
        /// </summary>
        string BillingInformation { get; }

        /// <summary>
        /// Gets the price to pay.
        /// </summary>
        decimal Price { get; }

        /// <summary>
        /// Gets the culture informations which should be used for payment.
        /// </summary>
        CultureInfo PriceCultureInfo { get; }
    }
}
