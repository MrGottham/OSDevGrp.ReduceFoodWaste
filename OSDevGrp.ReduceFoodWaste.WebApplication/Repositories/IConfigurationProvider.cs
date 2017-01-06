using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Interface for a configuration provider.
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Gets the configuration for memberships.
        /// </summary>
        IMembershipConfiguration MembershipConfiguration { get; }

        /// <summary>
        /// Gets the configuration for payments.
        /// </summary>
        IPaymentConfiguration PaymentConfiguration { get; }
    }
}
