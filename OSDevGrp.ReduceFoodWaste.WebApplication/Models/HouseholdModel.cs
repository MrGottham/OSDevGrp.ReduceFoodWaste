using System;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [Range(1, 64)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the household.
        /// </summary>
        [Range(0, 2048)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the privacy policies for the household.
        /// </summary>
        public PrivacyPolicyModel PrivacyPolicy { get; set; }

        #endregion
    }
}