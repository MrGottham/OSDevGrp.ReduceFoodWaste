using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Interface for a provider which can append local claims to a claims identity.
    /// </summary>
    public interface ILocalClaimProvider
    {
        /// <summary>
        /// Add the local claims to a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to add the local claims.</param>
        /// <returns>Task which will adds the local claims to the given claims identity.</returns>
        Task AddLocalClaimsAsync(ClaimsIdentity claimsIdentity);

        /// <summary>
        /// Add a local claim to a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to add the local claim.</param>
        /// <param name="claimToAdd">Local claim which should be added to the claims identity.</param>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>Task which will add the local claim to a given claims identity.</returns>
        Task AddLocalClaimAsync(ClaimsIdentity claimsIdentity, Claim claimToAdd, HttpContext httpContext = null);

        /// <summary>
        /// Generates a claim which indicates that a claim identity has been created as a household member.
        /// </summary>
        /// <returns>Claim which indicates that a claim identity has been created as a household member.</returns>
        Claim GenerateCreatedHouseholdMemberClaim();

        /// <summary>
        /// Generates a claim which indicates that a claim identity is an activated household member.
        /// </summary>
        /// <returns>Claim which indicates that a claim identity is an activated household member.</returns>
        Claim GenerateActivatedHouseholdMemberClaim();

        /// <summary>
        /// Generates a claim which indicates that a claim identity has accepted the privacy policies.
        /// </summary>
        /// <returns>Claim which indicates that a claim identity has accepted the privacy policies.</returns>
        Claim GeneratePrivacyPoliciesAcceptedClaim();

        /// <summary>
        /// Generates a claim which indicates that a claim identity is a validated household member.
        /// </summary>
        /// <returns>Claim which indicates that a claim identity is a validated household member.</returns>
        Claim GenerateValidatedHouseholdMemberClaim();
    }
}