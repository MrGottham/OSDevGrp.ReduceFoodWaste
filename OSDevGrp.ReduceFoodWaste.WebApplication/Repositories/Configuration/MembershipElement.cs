using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Membership configuration element.
    /// </summary>
    public class MembershipElement : ConfigurationElement, IMembershipElement
    {
        #region Properties

        /// <summary>
        /// Gets the name of the membership configuration element.
        /// </summary>
        [ConfigurationProperty("name", DefaultValue = "Basic", IsRequired = true, IsKey = true)]
        [RegexStringValidator("^(Basic|Deluxe|Premium)$")]
        public string Name
        {
            get { return (string) this["name"]; }
        }

        /// <summary>
        /// Gets the membership price for the da-DK culture.
        /// </summary>
        [ConfigurationProperty("price_da-DK", IsRequired = true)]
        public decimal PriceDaDk
        {
            get { return Convert.ToDecimal(base["price_da-DK"]); }
        }

        /// <summary>
        /// Gets the membership price for the en-US culture.
        /// </summary>
        [ConfigurationProperty("price_en-US", IsRequired = true)]
        public decimal PriceEnUs
        {
            get { return Convert.ToDecimal(base["price_en-US"]); }
        }

        /// <summary>
        /// Gets the dictionary containing the pricing for the membership.
        /// </summary>
        public IEnumerable<KeyValuePair<CultureInfo, decimal>> Pricing
        {
            get
            {
                return new Dictionary<CultureInfo, decimal>
                {
                    {new CultureInfo("da-DK"), PriceDaDk},
                    {new CultureInfo("en-US"), PriceEnUs}
                };
            }
        }

        #endregion
    }
}