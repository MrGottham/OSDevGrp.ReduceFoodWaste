using System;
using OSDevGrp.ReduceFoodWaste.WebApplication.Validations;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a given household member who are a member of a given household.
    /// </summary>
    public class MemberOfHouseholdModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the household member who are a member of the household.
        /// </summary>
        public Guid HouseholdMemberIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the household on which the household member has a membership.
        /// </summary>
        public Guid HouseholdIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the mail address for the household member who are a member of the household.
        /// </summary>
        [RequiredLocalized]
        [MaxLengthLocalized(128)]
        [MailAddressLocalized]
        public string MailAddress { get; set; }

        /// <summary>
        /// Gets or sets the value for whether the household member can be removed from the household.
        /// </summary>
        public bool Removable { get; set; }

        #endregion
    }
}