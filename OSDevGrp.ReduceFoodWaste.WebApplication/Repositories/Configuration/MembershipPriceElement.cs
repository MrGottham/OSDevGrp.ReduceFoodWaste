using System;
using System.Configuration;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Membership price configuration element.
    /// </summary>
    public class MembershipPriceElement : ConfigurationElement, IMembershipPriceElement
    {
        #region Properties

        /// <summary>
        /// Gets the culture name for price of the membership.
        /// </summary>
        [ConfigurationProperty("cultureName", DefaultValue = "da-DK", IsRequired = true, IsKey = true)]
        [RegexStringValidator("^[a-z]{2}-[A-Z]{2}$")]
        public string CultureName
        {
            get { return (string) base["cultureName"]; }
        }

        /// <summary>
        /// Gets the price for the membership.
        /// </summary>
        [ConfigurationProperty("price", IsRequired = true)]
        public decimal Price
        {
            get { return Convert.ToDecimal(base["price"]); }
        }

        #endregion
    }
}