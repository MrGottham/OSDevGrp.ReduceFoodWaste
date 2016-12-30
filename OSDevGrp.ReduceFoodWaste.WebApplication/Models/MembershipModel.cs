using System.Globalization;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Payments;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a membership.
    /// </summary>
    public class MembershipModel : IPayable
    {
        #region Private variables

        private string _description;
        private string _billingInformation;

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
        public string BillingInformation
        {
            get
            {
                if (_billingInformation == null)
                {
                    return _billingInformation;
                }
                return _billingInformation
                    .Replace("[Name]", Name)
                    .Replace("[Price]", Price.ToString("C", PriceCultureInfo));
            }
            set { _billingInformation = value; }
        }

        /// <summary>
        /// Gets or sets the price of the membership.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the name of the culture informations for the price of the membership.
        /// </summary>
        public string PriceCultureInfoName { get; set; }

        /// <summary>
        /// Gets the culture informations for the price of the membership.
        /// </summary>
        public CultureInfo PriceCultureInfo
        {
            get
            {
                return string.IsNullOrWhiteSpace(PriceCultureInfoName) ? CultureInfo.CurrentUICulture : new CultureInfo(PriceCultureInfoName);
            }
        }

        /// <summary>
        /// Gets whether the membership is for free.
        /// </summary>
        public bool IsFree
        {
            get { return Price <= 0M; }
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
