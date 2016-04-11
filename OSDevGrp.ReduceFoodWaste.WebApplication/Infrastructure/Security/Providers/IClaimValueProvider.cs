using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Interface for a provider which can get values from claims.
    /// </summary>
    public interface IClaimValueProvider
    {
        /// <summary>
        /// Checks whether an identitiy has been authenticated.
        /// </summary>
        /// <param name="identity">Identity.</param>
        /// <returns>True when the identity has been authenticated otherwise false.</returns>
        bool IsAuthenticated(IIdentity identity);

        /// <summary>
        /// Checks whether a given identity is a validated household member.
        /// </summary>
        /// <param name="identity">Identity which should be checked.</param>
        /// <returns>True when the given identity is a validated household member otherwise false.</returns>
        bool IsValidatedHouseholdMember(IIdentity identity);

        /// <summary>
        /// Checks whether a given claims identity is a validated household member.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity which should be checked.</param>
        /// <returns>True when the given claims identity is a validated household member otherwise false.</returns>
        bool IsValidatedHouseholdMember(ClaimsIdentity claimsIdentity);

        /// <summary>
        /// Gets the user name identifier for a given identity.
        /// </summary>
        /// <param name="identity">Identity on which to get the user name identifier.</param>
        /// <returns>User name identifier for the given identity.</returns>
        string GetUserNameIdentifier(IIdentity identity);

        /// <summary>
        /// Gets the user name identifier for a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to get the user name identifier.</param>
        /// <returns>User name identifier for the given claims identity.</returns>
        string GetUserNameIdentifier(ClaimsIdentity claimsIdentity);

        /// <summary>
        /// Gets the mail address for a given identity.
        /// </summary>
        /// <param name="identity">Identity on which to get the mail address.</param>
        /// <returns>Mail address for the given identity.</returns>
        string GetMailAddress(IIdentity identity);

        /// <summary>
        /// Gets the mail address for a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to get the mail address.</param>
        /// <returns>Mail address for the given claims identity.</returns>
        string GetMailAddress(ClaimsIdentity claimsIdentity);
    }
}