using System;
using System.Configuration;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Collection of membership price configuration elements.
    /// </summary>
    public class MembershipPriceElementCollection : ConfigurationElementCollection
    {
        #region Properties

        /// <summary>
        /// Gets the type of the ConfigurationElementCollection.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        /// <summary>
        /// Gets the membership price configuration element with a given culture name.
        /// </summary>
        /// <param name="cultureName">Culture name for the membership price configuration element to get.</param>
        /// <returns>Membership price configuration element with a given culture name.</returns>
        public new IMembershipElement this[string cultureName]
        {
            get
            {
                if (string.IsNullOrEmpty(cultureName))
                {
                    throw new ArgumentNullException(nameof(cultureName));
                }
                return (IMembershipElement) BaseGet(cultureName);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of a membership price configuration element.
        /// </summary>
        /// <returns>New instance of a membership price configuration element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MembershipPriceElement();
        }

        /// <summary>
        /// Gets the key for a given membership price configuration element.
        /// </summary>
        /// <param name="element">Memberhsip price configuration element on which to get the key.</param>
        /// <returns>Key for the given membership price configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return ((MembershipPriceElement) element).CultureName;
        }

        #endregion
    }
}