using System;
using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Provider which can get values from claims.
    /// </summary>
    public sealed class ClaimValueProvider : IClaimValueProvider
    {
        /// <summary>
        /// Gets the user name identifier for a given identity.
        /// </summary>
        /// <param name="identity">Identity on which to get the user name identifier.</param>
        /// <returns>User name identifier for the given identity.</returns>
        public string GetUserNameIdentifier(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (identity is ClaimsIdentity)
            {
                return GetUserNameIdentifier(identity as ClaimsIdentity);
            }
            return GetUserNameIdentifier(new ClaimsIdentity(identity));
        }

        /// <summary>
        /// Gets the user name identifier for a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to get the user name identifier.</param>
        /// <returns>User name identifier for the given claims identity.</returns>
        public string GetUserNameIdentifier(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            var nameIdentifierClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return nameIdentifierClaim == null ? null : nameIdentifierClaim.Value;
        }

        /// <summary>
        /// Gets the mail address for a given identity.
        /// </summary>
        /// <param name="identity">Identity on which to get the mail address.</param>
        /// <returns>Mail address for the given identity.</returns>
        public string GetMailAddress(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (identity is ClaimsIdentity)
            {
                return GetMailAddress(identity as ClaimsIdentity);
            }
            return GetMailAddress(new ClaimsIdentity(identity));
        }

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