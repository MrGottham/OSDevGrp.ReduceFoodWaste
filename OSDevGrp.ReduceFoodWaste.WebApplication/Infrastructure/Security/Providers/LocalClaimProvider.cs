using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Provider which can append local claims to a claims identity.
    /// </summary>
    public class LocalClaimProvider : ILocalClaimProvider
    {
        #region Public constants

        public const string LocalClaimIssuer = "OSDevGrp.ReduceFoodWaste.WebApplication";
 
        #endregion

        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a provider which can append local claims to a claims identity.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        public LocalClaimProvider(IHouseholdDataRepository householdDataRepository)
        {
            _householdDataRepository = householdDataRepository ?? throw new ArgumentNullException(nameof(householdDataRepository));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add the local claims to a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to add the local claims.</param>
        /// <returns>Task which will adds the local claims to the given claims identity.</returns>
        public Task AddLocalClaimsAsync(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException(nameof(claimsIdentity));
            }

            return Task.Run(() =>
            {
                try
                {
                    if (_householdDataRepository.IsHouseholdMemberCreatedAsync(claimsIdentity).GetAwaiter().GetResult() == false)
                    {
                        return;
                    }

                    claimsIdentity.AddClaim(GenerateCreatedHouseholdMemberClaim());

                    Task<bool> isHouseholdMemberActivatedTask = _householdDataRepository.IsHouseholdMemberActivatedAsync(claimsIdentity);
                    Task<bool> hasHouseholdMemberAcceptedPrivacyPolicyTask = _householdDataRepository.HasHouseholdMemberAcceptedPrivacyPolicyAsync(claimsIdentity);
                    Task.WhenAll(isHouseholdMemberActivatedTask, hasHouseholdMemberAcceptedPrivacyPolicyTask).GetAwaiter().GetResult();
                
                    if (isHouseholdMemberActivatedTask.Result)
                    {
                        claimsIdentity.AddClaim(GenerateActivatedHouseholdMemberClaim());
                    }

                    if (hasHouseholdMemberAcceptedPrivacyPolicyTask.Result)
                    {
                        claimsIdentity.AddClaim(GeneratePrivacyPoliciesAcceptedClaim());
                    }

                    if (claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember) == null || claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted) == null)
                    {
                        return;
                    }

                    claimsIdentity.AddClaim(GenerateValidatedHouseholdMemberClaim());
                }
                catch (ReduceFoodWasteExceptionBase)
                {
                    throw;
                }
                catch (AggregateException ex)
                {
                    throw ex.ToReduceFoodWasteException();
                }
                catch (Exception ex)
                {
                    throw new ReduceFoodWasteSystemException(ex.Message, ex);
                }
            });
        }

        /// <summary>
        /// Add a local claim to a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to add the local claim.</param>
        /// <param name="claimToAdd">Local claim which should be added to the claims identity.</param>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>Task which will add the local claim to a given claims identity.</returns>
        public Task AddLocalClaimAsync(ClaimsIdentity claimsIdentity, Claim claimToAdd, HttpContext httpContext = null)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException(nameof(claimsIdentity));
            }
            if (claimToAdd == null)
            {
                throw new ArgumentNullException(nameof(claimToAdd));
            }

            return Task.Run(() =>
            {
                try
                {
                    claimsIdentity.AddClaim(claimToAdd);

                    IAuthenticationManager authenticationManager = httpContext?.GetOwinContext().Authentication;
                    if (authenticationManager == null)
                    {
                        return;
                    }

                    claimsIdentity.RenewSignIn(authenticationManager);
                }
                catch (ReduceFoodWasteExceptionBase)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ReduceFoodWasteSystemException(ex.Message, ex);
                }
            });
        }

        /// <summary>
        /// Generates a claim which indicates that a claim identity has been created as a household member.
        /// </summary>
        /// <returns>Claim which indicates that a claim identity has been created as a household member.</returns>
        public Claim GenerateCreatedHouseholdMemberClaim()
        {
            return new Claim(LocalClaimTypes.CreatedHouseholdMember, Convert.ToString(true), ClaimValueTypes.Boolean, LocalClaimIssuer, LocalClaimIssuer);
        }

        /// <summary>
        /// Generates a claim which indicates that a claim identity is an activated household member.
        /// </summary>
        /// <returns>Claim which indicates that a claim identity is an activated household member.</returns>
        public Claim GenerateActivatedHouseholdMemberClaim()
        {
            return new Claim(LocalClaimTypes.ActivatedHouseholdMember, Convert.ToString(true), ClaimValueTypes.Boolean, LocalClaimIssuer, LocalClaimIssuer);
        }

        /// <summary>
        /// Generates a claim which indicates that a claim identity has accepted the privacy policies.
        /// </summary>
        /// <returns>Claim which indicates that a claim identity has accepted the privacy policies.</returns>
        public Claim GeneratePrivacyPoliciesAcceptedClaim()
        {
            return new Claim(LocalClaimTypes.PrivacyPoliciesAccepted, Convert.ToString(true), ClaimValueTypes.Boolean, LocalClaimIssuer, LocalClaimIssuer);
        }

        /// <summary>
        /// Generates a claim which indicates that a claim identity is a validated household member.
        /// </summary>
        /// <returns>Claim which indicates that a claim identity is a validated household member.</returns>
        public Claim GenerateValidatedHouseholdMemberClaim()
        {
            return new Claim(LocalClaimTypes.ValidatedHouseholdMember, Convert.ToString(true), ClaimValueTypes.Boolean, LocalClaimIssuer, LocalClaimIssuer);
        }

        #endregion
    }
}