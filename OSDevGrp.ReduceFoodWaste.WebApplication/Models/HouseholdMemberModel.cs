using System;

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
        /// Gets or sets the household members activation code.
        /// </summary>
        public string ActivationCode { get; set; }

        /// <summary>
        /// Gets whether the household member has been activated.
        /// </summary>
        public bool Activated
        {
            get { return ActivatedTime.HasValue; }
        }

        /// <summary>
        /// Gets or sets the date and time for when the household member has been activated.
        /// </summary>
        public DateTime? ActivatedTime { get; set; }

        /// <summary>
        /// Gets or sets the privacy policies for the household member.
        /// </summary>
        public PrivacyPolicyModel PrivacyPolicy { get; set; }

        /// <summary>
        /// Gets whether the household member has accepted the privacy policies.
        /// </summary>
        public bool PrivacyPolicyAccepted
        {
            get { return PrivacyPolicyAcceptedTime.HasValue; }
        }

        /// <summary>
        /// Gets or sets the date and time for when the household member has accepted the privacy policies.
        /// </summary>
        public DateTime? PrivacyPolicyAcceptedTime { get; set; }

        #endregion
    }
}