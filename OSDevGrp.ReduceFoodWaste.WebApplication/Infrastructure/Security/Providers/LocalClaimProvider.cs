using System;
using System.Security.Claims;
using System.Threading.Tasks;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
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
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException("householdDataRepository");
            }
            _householdDataRepository = householdDataRepository;
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
                throw new ArgumentNullException("claimsIdentity");
            }

            Action action = () =>
            {
                try
                {
                    var isHouseholdMemberCreatedTask = _householdDataRepository.IsHouseholdMemberCreatedAsync(claimsIdentity);
                    isHouseholdMemberCreatedTask.Wait();
                    if (isHouseholdMemberCreatedTask.Result == false)
                    {
                        return;
                    }
                    claimsIdentity.AddClaim(new Claim(LocalClaimTypes.CreatedHouseholdMember, Convert.ToString(true), ClaimValueTypes.Boolean, LocalClaimIssuer, LocalClaimIssuer));

                    var isHouseholdMemberActivatedTask = _householdDataRepository.IsHouseholdMemberActivatedAsync(claimsIdentity);
                    var hasHouseholdMemberAcceptedPrivacyPolicyTask = _householdDataRepository.HasHouseholdMemberAcceptedPrivacyPolicyAsync(claimsIdentity);
                    Task.WaitAll(isHouseholdMemberActivatedTask, hasHouseholdMemberAcceptedPrivacyPolicyTask);
                    if (isHouseholdMemberActivatedTask.Result)
                    {
                        claimsIdentity.AddClaim(new Claim(LocalClaimTypes.ActivatedHouseholdMember, Convert.ToString(true), ClaimValueTypes.Boolean, LocalClaimIssuer, LocalClaimIssuer));
                    }
                    if (hasHouseholdMemberAcceptedPrivacyPolicyTask.Result)
                    {
                        claimsIdentity.AddClaim(new Claim(LocalClaimTypes.PrivacyPoliciesAccepted, Convert.ToString(true), ClaimValueTypes.Boolean, LocalClaimIssuer, LocalClaimIssuer));
                    }

                    if (claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember) == null || claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted) == null)
                    {
                        return;
                    }
                    claimsIdentity.AddClaim(new Claim(LocalClaimTypes.ValidatedHouseholdMember, Convert.ToString(true), ClaimValueTypes.Boolean, LocalClaimIssuer, LocalClaimIssuer));
                }
                catch (ReduceFoodWasteExceptionBase)
                {
                    throw;
                }
                catch (AggregateException ex)
                {
                    Exception exceptionToThrow = null;
                    ex.Handle(exceptionToHandle =>
                    {
                        if (exceptionToHandle is ReduceFoodWasteExceptionBase)
                        {
                            exceptionToThrow = exceptionToHandle;
                            return true;
                        }
                        exceptionToThrow = new ReduceFoodWasteSystemException(exceptionToHandle.Message, exceptionToHandle);
                        return true;
                    });
                    throw exceptionToThrow;
                }
                catch (Exception ex)
                {
                    throw new ReduceFoodWasteSystemException(ex.Message, ex);
                }
            };

            return Task.Run(action);
        }

        #endregion
    }
}