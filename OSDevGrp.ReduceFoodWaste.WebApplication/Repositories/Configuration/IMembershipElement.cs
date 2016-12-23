using System.Collections.Generic;
using System.Globalization;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Interface for a membership configuration elememt.
    /// </summary>
    public interface IMembershipElement
    {
        /// <summary>
        /// Gets the name of the membership configuration element.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the dictionary containing the pricing for the membership.
        /// </summary>
        IEnumerable<KeyValuePair<CultureInfo, decimal>> Pricing { get; }
    }
}
