using System;
using OSDevGrp.ReduceFoodWaste.WebApplication.Validations;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a household identification.
    /// </summary>
    public class HouseholdIdentificationModel
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

        #endregion
    }
}