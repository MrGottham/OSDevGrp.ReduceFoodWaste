using System.Security.Claims;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Interface for a provider which can get values from claims.
    /// </summary>
    public interface IClaimValueProvider
    {
        /// <summary>
        /// Gets the mail address for a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to get the mail address.</param>
        /// <returns>Mail address for the given claims identity.</returns>
        string GetMailAddress(ClaimsIdentity claimsIdentity);
    }
}