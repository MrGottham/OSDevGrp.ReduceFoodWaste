using System;
using OSDevGrp.ReduceFoodWaste.WebApplication.Validations;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for the privacy policies.
    /// </summary>
    public class PrivacyPolicyModel : ICloneable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the privacy policies.
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the header for the privacy policies.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the body for the privacy policies.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets whether the privacy policies has been accepted.
        /// </summary>
        [RequiredLocalized]
        public bool IsAccepted { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a clone of the model for the privacy policies.
        /// </summary>
        /// <returns>Clone of the model for the privacy policies.</returns>
        public object Clone()
        {
            return new PrivacyPolicyModel
            {
                Identifier = Identifier,
                Header = Header,
                Body = Body,
                IsAccepted = false
            };
        }

        #endregion
    }
}