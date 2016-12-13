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
        /// <param name="membershipConfiguration">Implementation of the configuration for memberships.</param>
        public ConfigurationProvider(IMembershipConfiguration membershipConfiguration)
        {
            if (membershipConfiguration == null)
            {
                throw new ArgumentNullException(nameof(membershipConfiguration));
            }

            MembershipConfiguration = membershipConfiguration;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the configuration for memberships.
        /// </summary>
        public IMembershipConfiguration MembershipConfiguration { get; }

        #endregion
    }
}