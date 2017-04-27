using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a read only collection of household identifications.
    /// </summary>
    public class HouseholdIdentificationCollectionModel : ReadOnlyCollection<HouseholdIdentificationModel>
    {
        #region Constructor

        /// <summary>
        /// Creates model for a read only collection of household identifications.
        /// </summary>
        /// <param name="householdIdentificationModels">List of household identification models which should be contained in the read only collection.</param>
        /// <param name="householdMemberCanAddHouseholds">Indicates whether the current household member can add households.</param>
        public HouseholdIdentificationCollectionModel(IList<HouseholdIdentificationModel> householdIdentificationModels, bool householdMemberCanAddHouseholds) 
            : base(householdIdentificationModels)
        {
            HouseholdMemberCanAddHouseholds = householdMemberCanAddHouseholds;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether the current household member can add households.
        /// </summary>
        public bool HouseholdMemberCanAddHouseholds { get; }

        #endregion
    }
}