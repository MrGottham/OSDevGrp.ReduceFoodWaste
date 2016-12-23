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
        /// Gets the membership price configuration elements for the membership.
        /// </summary>
        IEnumerable<IMembershipPriceElement> Pricing { get; }

        /// <summary>
        /// Gets the membership price configuration element for a given culture information.
        /// </summary>
        /// <param name="cultureInfo">Culture information for which to get the membership price configuration element.</param>
        /// <returns>Membership price configuration element for the given culture information.</returns>
        IMembershipPriceElement GetMembershipPriceElement(CultureInfo cultureInfo);
    }
}
