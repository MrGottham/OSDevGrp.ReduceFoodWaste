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
        /// Gets or sets the privacy policies for the household member.
        /// </summary>
        public PrivacyPolicyModel PrivacyPolicy { get; set; }

        #endregion
    }
}