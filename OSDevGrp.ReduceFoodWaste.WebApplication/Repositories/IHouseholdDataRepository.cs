﻿using System.Globalization;
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
        /// Gets the privacy policies which should be accepted by a given identity.
        /// </summary>
        /// <param name="identity">Identity which should accept the privacy policies.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Privacy policies which should be accepted by a given identity.</returns>
        Task<PrivacyPolicyModel> GetPrivacyPoliciesAsync(IIdentity identity, CultureInfo cultureInfo);
    }
}
