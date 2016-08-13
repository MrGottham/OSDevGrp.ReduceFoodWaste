using System;
using System.Collections.Generic;
using OSDevGrp.ReduceFoodWaste.WebApplication.Validations;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a household.
    /// </summary>
    public class HouseholdModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the household.
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the name for the household.
        /// </summary>
        [RequiredLocalized]
        [MaxLengthLocalized(64)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the household.
        /// </summary>
        [MaxLengthLocalized(2048)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the privacy policies for the household.
        /// </summary>
        public PrivacyPolicyModel PrivacyPolicy { get; set; }

        /// <summary>
        /// Gets or sets the date and time for when the household created.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the collection of household member who has a membership of this household.
        /// </summary>
        public IEnumerable<HouseholdMemberModel> HouseholdMembers { get; set; }

        #endregion
    }
}