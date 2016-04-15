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
        #region Methods

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
            var validatedHouseholdMemberClaim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            if (validatedHouseholdMemberClaim == null)
            {
                return false;
            }
            if (string.Compare(validatedHouseholdMemberClaim.Issuer, LocalClaimProvider.LocalClaimIssuer, StringComparison.Ordinal) != 0)
            {
                return false;
            }
            return string.Compare(validatedHouseholdMemberClaim.OriginalIssuer, LocalClaimProvider.LocalClaimIssuer, StringComparison.Ordinal) == 0 && Convert.ToBoolean(validatedHouseholdMemberClaim.Value);
        }

        /// <summary>
        /// Checks whether a given identity is a created household member.
        /// </summary>
        /// <param name="identity">Identity which should be checked.</param>
        /// <returns>True when the given identity is a created household member otherwise false.</returns>
        public bool IsCreatedHouseholdMember(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (identity is ClaimsIdentity)
            {
                return IsCreatedHouseholdMember(identity as ClaimsIdentity);
            }
            return IsCreatedHouseholdMember(new ClaimsIdentity(identity));
        }

        /// <summary>
        /// Checks whether a given claims identity is a created household member.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity which should be checked.</param>
        /// <returns>True when the given claims identity is a created household member otherwise false.</returns>
        public bool IsCreatedHouseholdMember(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            var createdHouseholdMemberClaim = claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember);
            if (createdHouseholdMemberClaim == null)
            {
                return false;
            }
            if (string.Compare(createdHouseholdMemberClaim.Issuer, LocalClaimProvider.LocalClaimIssuer, StringComparison.Ordinal) != 0)
            {
                return false;
            }
            return string.Compare(createdHouseholdMemberClaim.OriginalIssuer, LocalClaimProvider.LocalClaimIssuer, StringComparison.Ordinal) == 0 && Convert.ToBoolean(createdHouseholdMemberClaim.Value);
        }

        /// <summary>
        /// Checks whether a given identity is an activated household member.
        /// </summary>
        /// <param name="identity">Identity which should be checked.</param>
        /// <returns>True when the given identity is an activated household member otherwise false.</returns>
        public bool IsActivatedHouseholdMember(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (identity is ClaimsIdentity)
            {
                return IsActivatedHouseholdMember(identity as ClaimsIdentity);
            }
            return IsActivatedHouseholdMember(new ClaimsIdentity(identity));
        }

        /// <summary>
        /// Checks whether a given claims identity is an activated  household member.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity which should be checked.</param>
        /// <returns>True when the given claims identity is an activated household member otherwise false.</returns>
        public bool IsActivatedHouseholdMember(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            var activatedHouseholdMemberClaim = claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember);
            if (activatedHouseholdMemberClaim == null)
            {
                return false;
            }
            if (string.Compare(activatedHouseholdMemberClaim.Issuer, LocalClaimProvider.LocalClaimIssuer, StringComparison.Ordinal) != 0)
            {
                return false;
            }
            return string.Compare(activatedHouseholdMemberClaim.OriginalIssuer, LocalClaimProvider.LocalClaimIssuer, StringComparison.Ordinal) == 0 && Convert.ToBoolean(activatedHouseholdMemberClaim.Value);
        }

        /// <summary>
        /// Checks whether a given identity has accepted the privacy policies.
        /// </summary>
        /// <param name="identity">Identity which should be checked.</param>
        /// <returns>True when the given identity has accepted the privacy policies otherwise false.</returns>
        public bool IsPrivacyPoliciesAccepted(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            if (identity is ClaimsIdentity)
            {
                return IsPrivacyPoliciesAccepted(identity as ClaimsIdentity);
            }
            return IsPrivacyPoliciesAccepted(new ClaimsIdentity(identity));
        }

        /// <summary>
        /// Checks whether a given claims identity has accepted the privacy policies.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity which should be checked.</param>
        /// <returns>True when the given claims identity has accepted the privacy policies otherwise false.</returns>
        public bool IsPrivacyPoliciesAccepted(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            var privacyPoliciesAcceptedClaim = claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted);
            if (privacyPoliciesAcceptedClaim == null)
            {
                return false;
            }
            if (string.Compare(privacyPoliciesAcceptedClaim.Issuer, LocalClaimProvider.LocalClaimIssuer, StringComparison.Ordinal) != 0)
            {
                return false;
            }
            return string.Compare(privacyPoliciesAcceptedClaim.OriginalIssuer, LocalClaimProvider.LocalClaimIssuer, StringComparison.Ordinal) == 0 && Convert.ToBoolean(privacyPoliciesAcceptedClaim.Value);
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

        #endregion
    }
}