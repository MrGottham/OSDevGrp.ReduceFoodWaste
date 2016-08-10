using System.Collections.Generic;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for the dashboard.
    /// </summary>
    public class DashboardModel
    {
        /// <summary>
        /// Gets or sets the household member.
        /// </summary>
        public HouseholdMemberModel HouseholdMember { get; set; }

        /// <summary>
        /// Gets the households on which the household member has a membership.
        /// </summary>
        public IEnumerable<HouseholdModel> Households
        {
            get
            {
                if (HouseholdMember == null)
                {
                    return null;
                }
                return HouseholdMember.Households;
            }
        }
    }
}