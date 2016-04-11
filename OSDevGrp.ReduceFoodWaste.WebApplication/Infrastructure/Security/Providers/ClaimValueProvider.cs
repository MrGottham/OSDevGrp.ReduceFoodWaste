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
        #region Constants

        public const string ValidatedHouseholdMemberClaim = "http://osdevgrp.local/foodwaste/security/validatedhouseholdmember";

        #endregion

        /// <summary>
        /// Checks whether an identitiy has been authenticated.
        /// </summary>
        /// <param name="identity">Identity.</param>
        /// <returns>True when the identity has been authenticated otherwise false.</returns>
        public bool IsAuthenticated(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            return identity.IsAuthenticated;
        }

        /// <summary>
        /// Checks whether a given identity is a validated household member.
        /// </summary>
        /// <param name="identity">Identity which should be checked.</param>
        /// <returns>True when the given identity is a validated household member otherwise false.</returns>
        public bool IsValidatedHouseholdMember(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (identity is ClaimsIdentity)
            {
                return IsValidatedHouseholdMember(identity as ClaimsIdentity);
            }
            return IsValidatedHouseholdMember(new ClaimsIdentity(identity));
        }

        /// <summary>
        /// Checks whether a given claims identity is a validated household member.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity which should be checked.</param>
        /// <returns>True when the given claims identity is a validated household member otherwise false.</returns>
        public bool IsValidatedHouseholdMember(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            var validatedHouseholdMemberClaim = claimsIdentity.FindFirst(ValidatedHouseholdMemberClaim);
            return validatedHouseholdMemberClaim != null && Convert.ToBoolean(validatedHouseholdMemberClaim.Value);
        }

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