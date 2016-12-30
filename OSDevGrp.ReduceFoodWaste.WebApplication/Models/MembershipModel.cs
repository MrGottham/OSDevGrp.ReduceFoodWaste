using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a membership.
    /// </summary>
    [Serializable]
    public class MembershipModel : PayableModel
    {
        #region Private variables

        private string _description;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the membership.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the membership.
        /// </summary>
        public string Description
        {
            get
            {
                if (_description == null)
                {
                    return _description;
                }
                return _description
                    .Replace("[Name]", Name)
                    .Replace("[Price]", Price.ToString("C", PriceCultureInfo));
            }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the billing information for the membership.
        /// </summary>
        public override string BillingInformation
        {
            get
            {
                if (base.BillingInformation == null)
                {
                    return base.BillingInformation;
                }
                return base.BillingInformation
                    .Replace("[Name]", Name);
            }
            set { base.BillingInformation = value; }
        }

        /// <summary>
        /// Gets or sets whether the membership can be renewed.
        /// </summary>
        public bool CanRenew { get; set; }

        /// <summary>
        /// Get or sets whether an upgrade to this membership is possible.
        /// </summary>
        public bool CanUpgrade { get; set; }

        #endregion
    }
}
