using System.Globalization;
using System.Security.Principal;
using System.Threading.Tasks;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Interface for a repository which can access household data.
    /// </summary>
    public interface IHouseholdDataRepository
    {
        /// <summary>
        /// Determinates whether a given identity has been created as a household member.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the given identity has been created as a household member otherwise false.</returns>
        Task<bool> IsHouseholdMemberCreatedAsync(IIdentity identity);

        /// <summary>
        /// Determinates whether the household member for a given identity has been activated.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the household member for the given identity has been activated otherwise false.</returns>
        Task<bool> IsHouseholdMemberActivatedAsync(IIdentity identity);

        /// <summary>
        /// Determinates whether the household member for a given identity has accepted the privacy policies.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the household membner for the given identity has accepted the privacy policies otherwise false.</returns>
        Task<bool> HasHouseholdMemberAcceptedPrivacyPolicyAsync(IIdentity identity);

        /// <summary>
        /// Creates a new household to a given identity.
        /// </summary>
        /// <param name="identity">Identity which should own the household.</param>
        /// <param name="householdModel">Model for the household to create.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the created household.</returns>
        Task<HouseholdModel> CreateHouseholdAsync(IIdentity identity, HouseholdModel householdModel, CultureInfo cultureInfo);

        /// <summary>
        /// Accepts the privacy policies on the household member which has been created for the given identity.
        /// </summary>
        /// <param name="identity">Identity on which to accept the privacy policies.</param>
        /// <param name="privacyPolicyModel">Model for the privacy policies to accept.</param>
        /// <returns>Model for the privacy policies which has been accepted.</returns>
        Task<PrivacyPolicyModel> AcceptPrivacyPolicyAsync(IIdentity identity, PrivacyPolicyModel privacyPolicyModel);

        /// <summary>
        /// Gets the privacy policies which should be accepted by a given identity.
        /// </summary>
        /// <param name="identity">Identity which should accept the privacy policies.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Privacy policies which should be accepted by a given identity.</returns>
        Task<PrivacyPolicyModel> GetPrivacyPoliciesAsync(IIdentity identity, CultureInfo cultureInfo);
    }
}
