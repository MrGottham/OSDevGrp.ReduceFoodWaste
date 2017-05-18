using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Interface for the configuration for the site.
    /// </summary>
    public interface ISiteConfiguration
    {
        /// <summary>
        /// Gets the callback address for the site.
        /// </summary>
        Uri CallbackAddress { get; }
    }
}
