using System.Collections.Generic;
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
        /// Get the household member account for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to the household member account.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the household member account for the given identity.</returns>
        Task<HouseholdMemberModel> GetHouseholdMemberAsync(IIdentity identity, CultureInfo cultureInfo);

        /// <summary>
        /// Gets the collection of household identifications for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to get the collection of household identifications.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Collection of household identifications for the given identity.</returns>
        Task<IEnumerable<HouseholdIdentificationModel>> GetHouseholdIdentificationCollectionAsync(IIdentity identity, CultureInfo cultureInfo);

        /// <summary>
        /// Get a given household for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to get a given household.</param>
        /// <param name="householdModel">Model for the household to get.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the household.</returns>
        Task<HouseholdModel> GetHouseholdAsync(IIdentity identity, HouseholdModel householdModel, CultureInfo cultureInfo);

        /// <summary>
        /// Creates a new household to a given identity.
        /// </summary>
        /// <param name="identity">Identity which should own the household.</param>
        /// <param name="householdModel">Model for the household to create.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the created household.</returns>
        Task<HouseholdModel> CreateHouseholdAsync(IIdentity identity, HouseholdModel householdModel, CultureInfo cultureInfo);

        /// <summary>
        /// Updates a given household for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to update the given household.</param>
        /// <param name="householdModel">Model for the household to update.</param>
        /// <returns>Model for the updated household.</returns>
        Task<HouseholdModel> UpdateHouseholdAsync(IIdentity identity, HouseholdModel householdModel);

        /// <summary>
        /// Adds a given household member to a given household for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to add a given household member on a given household.</param>
        /// <param name="memberOfHouseholdModel">Model for the household member to add.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the added household member on the given household.</returns>
        Task<MemberOfHouseholdModel> AddHouseholdMemberToHouseholdAsync(IIdentity identity, MemberOfHouseholdModel memberOfHouseholdModel, CultureInfo cultureInfo);

        /// <summary>
        /// Removes a given household member from a given household for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to remove a given household member from a given household.</param>
        /// <param name="memberOfHouseholdModel">Model for the household member to remove.</param>
        /// <returns>Model for the removed household member on the given household.</returns>
        Task<MemberOfHouseholdModel> RemoveHouseholdMemberFromHouseholdAsync(IIdentity identity, MemberOfHouseholdModel memberOfHouseholdModel);

        /// <summary>
        /// Activates the household member account for a given identity.
        /// </summary>
        /// <param name="identity">Identity whos household member account should be activated.</param>
        /// <param name="householdMemberModel">Household member account which should be activated.</param>
        /// <returns>Model for the activated household member.</returns>
        Task<HouseholdMemberModel> ActivateHouseholdMemberAsync(IIdentity identity, HouseholdMemberModel householdMemberModel);

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
