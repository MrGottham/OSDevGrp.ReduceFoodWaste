using System;
using System.Configuration;
using System.Reflection;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Configuration for memberships.
    /// </summary>
    public class MembershipConfiguration : ConfigurationSection, IMembershipConfiguration
    {
        #region Private variables

        private static IMembershipConfiguration _membershipConfiguration;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the membership configuration elements.
        /// </summary>
        [ConfigurationProperty("memberships", IsDefaultCollection = false, IsRequired = true)]
        [ConfigurationCollection(typeof(MembershipElementCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public MembershipElementCollection MembershipElements
        {
            get { return (MembershipElementCollection) base["memberships"]; }
        }

        /// <summary>
        /// Gets the membership configuration element for the membership named Basic.
        /// </summary>
        public IMembershipElement BasicMembership
        {
            get { return MembershipElements["Basic"]; }
        }

        /// <summary>
        /// Gets the membership configuration element for the membership named Deluxe.
        /// </summary>
        public IMembershipElement DeluxeMembership
        {
            get { return MembershipElements["Deluxe"]; }
        }

        /// <summary>
        /// Gets the membership configuration element for the membership named Premium.
        /// </summary>
        public IMembershipElement PremiumMembership
        {
            get { return MembershipElements["Premium"]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates and initialize the membership configuration.
        /// </summary>
        /// <returns>Created and intialized membership configuration.</returns>
        public static IMembershipConfiguration Create()
        {
            try
            {
                lock (SyncRoot)
                {
                    if (_membershipConfiguration != null)
                    {
                        return _membershipConfiguration;
                    }
                    _membershipConfiguration = (IMembershipConfiguration) ConfigurationManager.GetSection("membershipConfiguration");
                    return _membershipConfiguration;
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