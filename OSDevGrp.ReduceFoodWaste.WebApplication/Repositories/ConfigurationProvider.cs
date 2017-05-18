using System;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Configuration provider.
    /// </summary>
    public class ConfigurationProvider : IConfigurationProvider
    {
        #region Constructor

        /// <summary>
        /// Create an instance of the configuration provider.
        /// </summary>
        /// <param name="siteConfiguration">Implementation of the configuration for the site.</param>
        /// <param name="membershipConfiguration">Implementation of the configuration for memberships.</param>
        /// <param name="paymentConfiguration">Implementation of the configuration for payments.</param>
        public ConfigurationProvider(ISiteConfiguration siteConfiguration, IMembershipConfiguration membershipConfiguration, IPaymentConfiguration paymentConfiguration)
        {
            if (siteConfiguration == null)
            {
                throw new ArgumentNullException(nameof(siteConfiguration));
            }
            if (membershipConfiguration == null)
            {
                throw new ArgumentNullException(nameof(membershipConfiguration));
            }
            if (paymentConfiguration == null)
            {
                throw new ArgumentNullException(nameof(paymentConfiguration));
            }

            SiteConfiguration = siteConfiguration;
            MembershipConfiguration = membershipConfiguration;
            PaymentConfiguration = paymentConfiguration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration for the site.
        /// </summary>
        public ISiteConfiguration SiteConfiguration { get; }

        /// <summary>
        /// Gets the configuration for memberships.
        /// </summary>
        public IMembershipConfiguration MembershipConfiguration { get; }

        /// <summary>
        /// Gets the configuration for payments.
        /// </summary>
        public IPaymentConfiguration PaymentConfiguration { get; }

        #endregion
    }
}