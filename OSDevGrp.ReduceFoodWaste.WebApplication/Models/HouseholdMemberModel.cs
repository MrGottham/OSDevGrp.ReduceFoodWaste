using System;
using System.Collections.Generic;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a household member.
    /// </summary>
    public class HouseholdMemberModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the household member.
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the household members name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the household members mail address.
        /// </summary>
        public string MailAddress { get; set; }

        /// <summary>
        /// Gets or sets the household members activation code.
        /// </summary>
        public string ActivationCode { get; set; }

        /// <summary>
        /// Gets whether the household member has been activated.
        /// </summary>
        public bool IsActivated
        {
            get { return ActivatedTime.HasValue; }
        }

        /// <summary>
        /// Gets or sets the date and time for when the household member has been activated.
        /// </summary>
        public DateTime? ActivatedTime { get; set; }

        /// <summary>
        /// Gets or sets the name of the household members membership.
        /// </summary>
        public string Membership { get; set; }

        /// <summary>
        /// Gets or sets the date and time for when the household members membership will expire.
        /// </summary>
        public DateTime? MembershipExpireTime { get; set; }

        /// <summary>
        /// Gets or sets whether the current membership can be renewed.
        /// </summary>
        public bool CanRenewMembership { get; set; }

        /// <summary>
        /// Gets or sets whether the current membership can be upgraded.
        /// </summary>
        public bool CanUpgradeMembership { get; set; }

        /// <summary>
        /// Gets or sets the privacy policies for the household member.
        /// </summary>
        public PrivacyPolicyModel PrivacyPolicy { get; set; }

        /// <summary>
        /// Gets whether the household member has accepted the privacy policies.
        /// </summary>
        public bool HasAcceptedPrivacyPolicy
        {
            get { return PrivacyPolicyAcceptedTime.HasValue; }
        }

        /// <summary>
        /// Gets or sets the date and time for when the household member has accepted the privacy policies.
        /// </summary>
        public DateTime? PrivacyPolicyAcceptedTime { get; set; }

        /// <summary>
        /// Gets or sets whether the household member has reached the household limet (number of households).
        /// </summary>
        public bool HasReachedHouseholdLimit { get; set; }

        /// <summary>
        /// Gets or sets the date and time for when the household member was created.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the collection of memberships which the household member can upgrade to.
        /// </summary>
        public IEnumerable<string> UpgradeableMemberships { get; set; }

        /// <summary>
        /// Gets or sets the collection of household where the household member has a membership.
        /// </summary>
        public IEnumerable<HouseholdModel> Households { get; set; }

        #endregion
    }
}