using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

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
        /// Gets the membership price configuration elements.
        /// </summary>
        [ConfigurationProperty("pricing", IsDefaultCollection = false, IsRequired = true)]
        [ConfigurationCollection(typeof(MembershipPriceElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public MembershipPriceElementCollection MembershipPriceElements
        {
            get { return (MembershipPriceElementCollection) base["pricing"]; }
        }

        /// <summary>
        /// Gets the dictionary containing the pricing for the membership.
        /// </summary>
        public IEnumerable<IMembershipPriceElement> Pricing
        {
            get
            {
                return MembershipPriceElements.Cast<IMembershipPriceElement>().ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the membership price configuration element for a given culture information.
        /// </summary>
        /// <param name="cultureInfo">Culture information for which to get the membership price configuration element.</param>
        /// <returns>Membership price configuration element for the given culture information.</returns>
        public IMembershipPriceElement GetMembershipPriceElement(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            IList<IMembershipPriceElement> membershipPriceElementCollection = MembershipPriceElements.Cast<IMembershipPriceElement>().ToList();

            IMembershipPriceElement membershipPriceElement = membershipPriceElementCollection.SingleOrDefault(m => string.Compare(m.CultureName, cultureInfo.Name, StringComparison.Ordinal) == 0);
            if (membershipPriceElement != null)
            {
                return membershipPriceElement;
            }

            membershipPriceElement = membershipPriceElementCollection.SingleOrDefault(m => string.Compare(m.CultureName, CultureInfo.CurrentUICulture.Name, StringComparison.Ordinal) == 0);
            return membershipPriceElement ?? membershipPriceElementCollection.First();
        }

        #endregion
    }
}