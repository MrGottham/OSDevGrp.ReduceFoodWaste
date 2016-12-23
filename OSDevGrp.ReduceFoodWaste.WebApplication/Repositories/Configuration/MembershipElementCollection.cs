using System;
using System.Configuration;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Collection of membership configuration elements.
    /// </summary>
    public class MembershipElementCollection : ConfigurationElementCollection
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
        /// Gets the membership configuration element with a given name.
        /// </summary>
        /// <param name="name">Name for the membership configuration element to get.</param>
        /// <returns>Membership configuration element for the given name.</returns>
        public new IMembershipElement this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }
                return (IMembershipElement) BaseGet(name);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of a membership configuration element.
        /// </summary>
        /// <returns>New instance of a membership configuration element.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new MembershipElement();
        }

        /// <summary>
        /// Gets the key for a given membership configuration element.
        /// </summary>
        /// <param name="element">Memberhsip configuration element on which to get the key.</param>
        /// <returns>Key for the given membership configuration element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return ((MembershipElement) element).Name;
        }

        #endregion
    }
}