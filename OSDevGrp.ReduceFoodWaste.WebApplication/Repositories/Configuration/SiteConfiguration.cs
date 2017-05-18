using System;
using System.Configuration;
using System.Reflection;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Configuration for the site.
    /// </summary>
    public class SiteConfiguration : ConfigurationSection, ISiteConfiguration
    {
        #region Private variables

        private static ISiteConfiguration _siteConfiguration;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the callback address for the site.
        /// </summary>
        public Uri CallbackAddress => new Uri(CallbackAddressElement.Value);

        /// <summary>
        /// Gets the callback address configuration element.
        /// </summary>
        [ConfigurationProperty("callbackAddress", IsRequired = true)]
        public CallbackAddressElement CallbackAddressElement => (CallbackAddressElement) this["callbackAddress"];

        #endregion

        #region Methods

        /// <summary>
        /// Creates and initialize the configuration for the site.
        /// </summary>
        /// <returns>Created and intialized configuration for the site.</returns>
        public static ISiteConfiguration Create()
        {
            try
            {
                lock (SyncRoot)
                {
                    if (_siteConfiguration != null)
                    {
                        return _siteConfiguration;
                    }
                    _siteConfiguration = (ISiteConfiguration) ConfigurationManager.GetSection("siteConfiguration");
                    return _siteConfiguration;
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