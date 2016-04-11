using System;
using System.Security.Principal;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication
{
    /// <summary>
    /// Helpers for an identity.
    /// </summary>
    public static class IdentityHelper
    {
        /// <summary>
        /// Checks whether a given identity is a validated household member.
        /// </summary>
        /// <param name="identity">Identity which should be checked.</param>
        /// <returns>True when the given identity is a validated household member otherwise false.</returns>
        public static bool IsValidatedHouseholdMember(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            IClaimValueProvider claimValueProvider = new ClaimValueProvider();
            return claimValueProvider.IsValidatedHouseholdMember(identity);
        }
    }
}