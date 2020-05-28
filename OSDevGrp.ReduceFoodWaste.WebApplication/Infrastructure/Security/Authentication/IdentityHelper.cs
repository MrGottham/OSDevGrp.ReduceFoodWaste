using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
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
                throw new ArgumentNullException(nameof(identity));
            }

            IClaimValueProvider claimValueProvider = new ClaimValueProvider();
            return claimValueProvider.IsValidatedHouseholdMember(identity);
        }

        /// <summary>
        /// Sign in with the given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">The claims identity to sign in.</param>
        /// <param name="authenticationManager">The authentication manager used to sign in a claims identity.</param>
        internal static void SignIn(this ClaimsIdentity claimsIdentity, IAuthenticationManager authenticationManager)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException(nameof(claimsIdentity));
            }
            if (authenticationManager == null)
            {
                throw new ArgumentNullException(nameof(authenticationManager));
            }

            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                AllowRefresh = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IssuedUtc = DateTimeOffset.UtcNow
            };
            authenticationManager.SignIn(authenticationProperties, claimsIdentity);
        }

        /// <summary>
        /// Renew the sign in for the given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">The claims identity to renew the sign in.</param>
        /// <param name="authenticationManager">The authentication manager used to sign in the claims identity.</param>
        internal static void RenewSignIn(this ClaimsIdentity claimsIdentity, IAuthenticationManager authenticationManager)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException(nameof(claimsIdentity));
            }
            if (authenticationManager == null)
            {
                throw new ArgumentNullException(nameof(authenticationManager));
            }

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            claimsIdentity.SignIn(authenticationManager);
        }
    }
}