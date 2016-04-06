using System;
using System.Security.Claims;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Provider which can get values from claims.
    /// </summary>
    public sealed class ClaimValueProvider : IClaimValueProvider
    {
        /// <summary>
        /// Gets the mail address for a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to get the mail address.</param>
        /// <returns>Mail address for the given claims identity.</returns>
        public string GetMailAddress(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            return emailClaim == null ? null : emailClaim.Value;
        }
    }
}