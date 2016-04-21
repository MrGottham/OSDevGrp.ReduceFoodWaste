﻿using System;

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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description for the household.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the privacy policies for the household.
        /// </summary>
        public PrivacyPolicyModel PrivacyPolicy { get; set; }

        #endregion
    }
}