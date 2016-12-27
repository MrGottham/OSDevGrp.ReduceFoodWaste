using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading.Tasks;
using OSDevGrp.ReduceFoodWaste.WebApplication.HouseholdDataService;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Repository which can access household data.
    /// </summary>
    public class HouseholdDataRepository : IHouseholdDataRepository
    {
        #region Private constants

        private const string HouseholdDataServiceEndpointConfigurationName = "HouseholdDataService";
        private const string HouseholdIdentificationCollectionCacheName = "OSDevGrp.ReduceFoodWaste.WebApplication.HouseholdIdentificationCollectionCache";
        private const string TranslationInfoIdentifierCacheName = "OSDevGrp.ReduceFoodWaste.WebApplication.TranslationInfoIdentifierCache";
        private const string PrivacyPolicyModelCacheName = "OSDevGrp.ReduceFoodWaste.WebApplication.PrivacyPolicyModelCache";

        #endregion

        #region Private variables

        private readonly ICredentialsProvider _credentialsProvider;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IHouseholdDataConverter _householdDataConverter;
        private readonly ObjectCache _objectCache = MemoryCache.Default; 

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a repository which can access household data.
        /// </summary>
        /// <param name="credentialsProvider">Implementation of a provider which can creates credentials.</param>
        /// <param name="configurationProvider">Implementation of a provider which can provide configuration.</param>
        /// <param name="householdDataConverter">Implementation of a converter which can convert household data.</param>
        public HouseholdDataRepository(ICredentialsProvider credentialsProvider, IConfigurationProvider configurationProvider, IHouseholdDataConverter householdDataConverter)
        {
            if (credentialsProvider == null)
            {
                throw new ArgumentNullException(nameof(credentialsProvider));
            }
            if (configurationProvider == null)
            {
                throw new ArgumentNullException(nameof(configurationProvider));
            }
            if (householdDataConverter == null)
            {
                throw new ArgumentNullException(nameof(householdDataConverter));
            }
            _credentialsProvider = credentialsProvider;
            _configurationProvider = configurationProvider;
            _householdDataConverter = householdDataConverter;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determinates whether a given identity has been created as a household member.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the given identity has been created as a household member otherwise false.</returns>
        public Task<bool> IsHouseholdMemberCreatedAsync(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            Func<HouseholdDataServiceChannel, bool> callbackFunc = channel =>
            {
                var query = new HouseholdMemberIsCreatedQuery();
                var result = channel.HouseholdMemberIsCreated(query);

                return _householdDataConverter.Convert<BooleanResult, bool>(result);
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Determinates whether the household member for a given identity has been activated.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the household member for the given identity has been activated otherwise false.</returns>
        public Task<bool> IsHouseholdMemberActivatedAsync(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            Func<HouseholdDataServiceChannel, bool> callbackFunc = channel =>
            {
                var query = new HouseholdMemberIsActivatedQuery();
                var result = channel.HouseholdMemberIsActivated(query);

                return _householdDataConverter.Convert<BooleanResult, bool>(result);
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Determinates whether the household member for a given identity has accepted the privacy policies.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the household membner for the given identity has accepted the privacy policies otherwise false.</returns>
        public Task<bool> HasHouseholdMemberAcceptedPrivacyPolicyAsync(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            Func<HouseholdDataServiceChannel, bool> callbackFunc = channel =>
            {
                var query = new HouseholdMemberHasAcceptedPrivacyPolicyQuery();
                var result = channel.HouseholdMemberHasAcceptedPrivacyPolicy(query);

                return _householdDataConverter.Convert<BooleanResult, bool>(result);
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Get the household member account for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to the household member account.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the household member account for the given identity.</returns>
        public Task<HouseholdMemberModel> GetHouseholdMemberAsync(IIdentity identity, CultureInfo cultureInfo)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Func<HouseholdDataServiceChannel, HouseholdMemberModel> callbackFunc = channel => GetHouseholdMember(channel, identity, cultureInfo);

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Gets the collection of household identifications for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to get the collection of household identifications.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Collection of household identifications for the given identity.</returns>
        public Task<IEnumerable<HouseholdIdentificationModel>> GetHouseholdIdentificationCollectionAsync(IIdentity identity, CultureInfo cultureInfo)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Func<HouseholdDataServiceChannel, IEnumerable<HouseholdIdentificationModel>> callbackFunc = channel =>
            {
                var householdIdentificationCollection = GetHouseholdIdentificationCollection(identity);
                if (householdIdentificationCollection != null)
                {
                    return householdIdentificationCollection;
                }

                var householdMember = GetHouseholdMember(channel, identity, cultureInfo);
                if (householdMember.Households == null || householdMember.Households.Any() == false)
                {
                    return new List<HouseholdIdentificationModel>();
                }

                householdIdentificationCollection = householdMember.Households.Cast<HouseholdIdentificationModel>().ToList();
                StoreHouseholdIdentificationCollection(identity, householdIdentificationCollection);

                return householdIdentificationCollection;
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Get a given household for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to get a given household.</param>
        /// <param name="householdModel">Model for the household to get.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the household.</returns>
        public Task<HouseholdModel> GetHouseholdAsync(IIdentity identity, HouseholdModel householdModel, CultureInfo cultureInfo)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (householdModel == null)
            {
                throw new ArgumentNullException(nameof(householdModel));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Func<HouseholdDataServiceChannel, HouseholdModel> callbackFunc = channel =>
            {
                var currentHouseholdMember = GetHouseholdMember(channel, identity, cultureInfo);

                var query = new HouseholdDataGetQuery
                {
                    HouseholdIdentifier = householdModel.Identifier,
                    TranslationInfoIdentifier = GetTranslationInfoIdentifier(channel, cultureInfo)
                };
                var result = channel.HouseholdDataGet(query);

                var household = _householdDataConverter.Convert<HouseholdView, HouseholdModel>(result);
                household.PrivacyPolicy = currentHouseholdMember.PrivacyPolicy;

                foreach (var householdMember in household.HouseholdMembers)
                {
                    householdMember.HouseholdIdentifier = household.Identifier;
                    householdMember.Removable = householdMember.HouseholdMemberIdentifier != currentHouseholdMember.Identifier;
                }

                return household;
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Creates a new household to a given identity.
        /// </summary>
        /// <param name="identity">Identity which should own the household.</param>
        /// <param name="householdModel">Model for the household to create.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the created household.</returns>
        public Task<HouseholdModel> CreateHouseholdAsync(IIdentity identity, HouseholdModel householdModel, CultureInfo cultureInfo)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (householdModel == null)
            {
                throw new ArgumentNullException(nameof(householdModel));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Func<HouseholdDataServiceChannel, HouseholdModel> callbackFunc = channel =>
            {
                var command = _householdDataConverter.Convert<HouseholdModel, HouseholdAddCommand>(householdModel);
                command.TranslationInfoIdentifier = GetTranslationInfoIdentifier(channel, cultureInfo);

                var result = channel.HouseholdAdd(command);

                ClearHouseholdIdentificationCollection(identity);

                householdModel.Identifier = result.Identifier ?? default(Guid);
                return householdModel;
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Updates a given household for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to update the given household.</param>
        /// <param name="householdModel">Model for the household to update.</param>
        /// <returns>Model for the updated household.</returns>
        public Task<HouseholdModel> UpdateHouseholdAsync(IIdentity identity, HouseholdModel householdModel)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (householdModel == null)
            {
                throw new ArgumentNullException(nameof(householdModel));
            }

            Func<HouseholdDataServiceChannel, HouseholdModel> callbackFunc = channel =>
            {
                var command = _householdDataConverter.Convert<HouseholdModel, HouseholdUpdateCommand>(householdModel);

                var result = channel.HouseholdUpdate(command);

                ClearHouseholdIdentificationCollection(identity);

                householdModel.Identifier = result.Identifier ?? default(Guid);
                return householdModel;
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Adds a given household member to a given household for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to add a given household member on a given household.</param>
        /// <param name="memberOfHouseholdModel">Model for the household member to add.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the added household member on the given household.</returns>
        public Task<MemberOfHouseholdModel> AddHouseholdMemberToHouseholdAsync(IIdentity identity, MemberOfHouseholdModel memberOfHouseholdModel, CultureInfo cultureInfo)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (memberOfHouseholdModel == null)
            {
                throw new ArgumentNullException(nameof(memberOfHouseholdModel));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Func<HouseholdDataServiceChannel, MemberOfHouseholdModel> callbackFunc = channel =>
            {
                var command = _householdDataConverter.Convert<MemberOfHouseholdModel, HouseholdAddHouseholdMemberCommand>(memberOfHouseholdModel);
                command.TranslationInfoIdentifier = GetTranslationInfoIdentifier(channel, cultureInfo);

                var result = channel.HouseholdAddHouseholdMember(command);

                memberOfHouseholdModel.HouseholdMemberIdentifier = result.Identifier ?? default(Guid);
                return memberOfHouseholdModel;
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Removes a given household member from a given household for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to remove a given household member from a given household.</param>
        /// <param name="memberOfHouseholdModel">Model for the household member to remove.</param>
        /// <returns>Model for the removed household member on the given household.</returns>
        public Task<MemberOfHouseholdModel> RemoveHouseholdMemberFromHouseholdAsync(IIdentity identity, MemberOfHouseholdModel memberOfHouseholdModel)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (memberOfHouseholdModel == null)
            {
                throw new ArgumentNullException(nameof(memberOfHouseholdModel));
            }

            Func<HouseholdDataServiceChannel, MemberOfHouseholdModel> callbackFunc = channel =>
            {
                var command = _householdDataConverter.Convert<MemberOfHouseholdModel, HouseholdRemoveHouseholdMemberCommand>(memberOfHouseholdModel);

                var result = channel.HouseholdRemoveHouseholdMember(command);

                memberOfHouseholdModel.HouseholdMemberIdentifier = result.Identifier ?? default(Guid);
                return memberOfHouseholdModel;
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Activates the household member account for a given identity.
        /// </summary>
        /// <param name="identity">Identity whos household member account should be activated.</param>
        /// <param name="householdMemberModel">Household member account which should be activated.</param>
        /// <returns>Model for the activated household member.</returns>
        public Task<HouseholdMemberModel> ActivateHouseholdMemberAsync(IIdentity identity, HouseholdMemberModel householdMemberModel)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (householdMemberModel == null)
            {
                throw new ArgumentNullException(nameof(householdMemberModel));
            }

            Func<HouseholdDataServiceChannel, HouseholdMemberModel> callbackFunc = channel =>
            {
                var command = _householdDataConverter.Convert<HouseholdMemberModel, HouseholdMemberActivateCommand>(householdMemberModel);
                var result = channel.HouseholdMemberActivate(command);

                householdMemberModel.Identifier = result.Identifier ?? default(Guid);
                householdMemberModel.ActivatedTime = result.EventDate;
                return householdMemberModel;
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Accepts the privacy policies on the household member which has been created for the given identity.
        /// </summary>
        /// <param name="identity">Identity on which to accept the privacy policies.</param>
        /// <param name="privacyPolicyModel">Model for the privacy policies to accept.</param>
        /// <returns>Model for the privacy policies which has been accepted.</returns>
        public Task<PrivacyPolicyModel> AcceptPrivacyPolicyAsync(IIdentity identity, PrivacyPolicyModel privacyPolicyModel)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (privacyPolicyModel == null)
            {
                throw new ArgumentNullException(nameof(privacyPolicyModel));
            }

            Func<HouseholdDataServiceChannel, PrivacyPolicyModel> callbackFunc = channel =>
            {
                var command = _householdDataConverter.Convert<PrivacyPolicyModel, HouseholdMemberAcceptPrivacyPolicyCommand>(privacyPolicyModel);
                var result = channel.HouseholdMemberAcceptPrivacyPolicy(command);

                privacyPolicyModel.AcceptedTime = result.EventDate;
                return privacyPolicyModel;
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Gets all the possible memberships for a given identity.
        /// </summary>
        /// <param name="identity">Identity for who the possible memberships should be returned.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Possible memberships for the given identity.</returns>
        public Task<IEnumerable<MembershipModel>> GetMembershipsAsync(IIdentity identity, CultureInfo cultureInfo)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Func<HouseholdDataServiceChannel, IEnumerable<MembershipModel>> callbackFunc = channel =>
            {
                var householdMemberModel = GetHouseholdMember(channel, identity, cultureInfo);

                IMembershipConfiguration membershipConfiguration = _configurationProvider.MembershipConfiguration;
                IMembershipElement basicMembership = membershipConfiguration.BasicMembership;
                IMembershipPriceElement basicMembershipPrice = basicMembership.GetMembershipPriceElement(cultureInfo);
                IMembershipElement deluxeMembership = membershipConfiguration.DeluxeMembership;
                IMembershipPriceElement deluxeMembershipPrice = deluxeMembership.GetMembershipPriceElement(cultureInfo);
                IMembershipElement premiumMembership = membershipConfiguration.PremiumMembership;
                IMembershipPriceElement premiumMembershipPrice = premiumMembership.GetMembershipPriceElement(cultureInfo);

                return new List<MembershipModel>
                {
                    new MembershipModel
                    {
                        Name = basicMembership.Name,
                        Description = Texts.MembershipBasicDescription,
                        Price = basicMembershipPrice.Price,
                        PriceCultureInfoName = basicMembershipPrice.CultureName,
                        CanRenew = householdMemberModel.CanRenewMembership && string.Compare(householdMemberModel.Membership, basicMembership.Name, StringComparison.Ordinal) == 0,
                        CanUpgrade = householdMemberModel.CanUpgradeMembership && householdMemberModel.UpgradeableMemberships != null && householdMemberModel.UpgradeableMemberships.Contains(basicMembership.Name)
                    },
                    new MembershipModel
                    {
                        Name = deluxeMembership.Name,
                        Description = Texts.MembershipDeluxeDescription,
                        Price = deluxeMembershipPrice.Price,
                        PriceCultureInfoName = deluxeMembershipPrice.CultureName,
                        CanRenew = householdMemberModel.CanRenewMembership && string.Compare(householdMemberModel.Membership, deluxeMembership.Name, StringComparison.Ordinal) == 0,
                        CanUpgrade = householdMemberModel.CanUpgradeMembership && householdMemberModel.UpgradeableMemberships != null && householdMemberModel.UpgradeableMemberships.Contains(deluxeMembership.Name)
                    },
                    new MembershipModel
                    {
                        Name = premiumMembership.Name,
                        Description = Texts.MembershipPremiumDescription,
                        Price = premiumMembershipPrice.Price,
                        PriceCultureInfoName = premiumMembershipPrice.CultureName,
                        CanRenew = householdMemberModel.CanRenewMembership &&  string.Compare(householdMemberModel.Membership, premiumMembership.Name, StringComparison.Ordinal) == 0,
                        CanUpgrade = householdMemberModel.CanUpgradeMembership && householdMemberModel.UpgradeableMemberships != null && householdMemberModel.UpgradeableMemberships.Contains(premiumMembership.Name)
                    },
                };
            };

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), callbackFunc));
        }

        /// <summary>
        /// Gets the privacy policies which should be accepted by a given identity.
        /// </summary>
        /// <param name="identity">Identity which should accept the privacy policies.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Privacy policies which should be accepted by a given identity.</returns>
        public Task<PrivacyPolicyModel> GetPrivacyPoliciesAsync(IIdentity identity, CultureInfo cultureInfo)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            return Task.Run(CallWrapper(identity, MethodBase.GetCurrentMethod(), channel => GetPrivacyPolicies(channel, cultureInfo)));
        }

        /// <summary>
        /// Encapsulate calls to a service method.
        /// </summary>
        /// <typeparam name="TResult">Type of the result the encapsulated call should produce.</typeparam>
        /// <param name="identity">Identity which should used for the credentials.</param>
        /// <param name="methodBase">Method base for the calling method.</param>
        /// <param name="callbackFunc">Callback function which produce and result the result.</param>
        /// <returns>Result from the callback funciton.</returns>
        private Func<TResult> CallWrapper<TResult>(IIdentity identity, MethodBase methodBase, Func<HouseholdDataServiceChannel, TResult> callbackFunc)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (methodBase == null)
            {
                throw new ArgumentNullException(nameof(methodBase));
            }
            if (callbackFunc == null)
            {
                throw new ArgumentNullException(nameof(callbackFunc));
            }

            return () =>
            {
                try
                {
                    var channelFactory = CreateChannelFactory(identity);
                    var channel = channelFactory.CreateChannel();
                    try
                    {
                        var result = callbackFunc.Invoke(channel);
                        channel.Close();

                        return result;
                    }
                    catch
                    {
                        channel.Abort();
                        throw;
                    }
                    finally
                    {
                        var dispoableChannel = (IDisposable) channel;
                        dispoableChannel.Dispose();
                    }
                }
                catch (FaultException<Fault> ex)
                {
                    switch (ex.Detail.FaultType)
                    {
                        case FaultType.BusinessFault:
                            throw new ReduceFoodWasteBusinessException(ex.Detail.ErrorMessage, ex);

                        case FaultType.RepositoryFault:
                            throw new ReduceFoodWasteRepositoryException(ex.Detail.ErrorMessage, methodBase, ex);

                        case FaultType.SystemFault:
                            throw new ReduceFoodWasteSystemException(ex.Detail.ErrorMessage, ex);
                    }
                    throw new ReduceFoodWasteRepositoryException(ex.Reason.GetMatchingTranslation().Text, methodBase, ex);
                }
                catch (FaultException ex)
                {
                    throw new ReduceFoodWasteRepositoryException(ex.Reason.GetMatchingTranslation().Text, methodBase, ex);
                }
                catch (ReduceFoodWasteExceptionBase)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new ReduceFoodWasteRepositoryException(ex.Message, methodBase, ex);
                }
            };
        }

        /// <summary>
        /// Get the household member account for a given identity.
        /// </summary>
        /// <param name="channel">Channel on which to get the household member account fot the given identity.</param>
        /// <param name="identity">Identity for which to the household member account.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the household member account for the given identity.</returns>
        private HouseholdMemberModel GetHouseholdMember(HouseholdDataServiceChannel channel, IIdentity identity, CultureInfo cultureInfo)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            var query = new HouseholdMemberDataGetQuery
            {
                TranslationInfoIdentifier = GetTranslationInfoIdentifier(channel, cultureInfo)
            };
            var result = channel.HouseholdMemberDataGet(query);

            var householdMember = _householdDataConverter.Convert<HouseholdMemberView, HouseholdMemberModel>(result);
            householdMember.Name = identity.Name;

            var privacyPolicy = GetPrivacyPolicies(channel, cultureInfo);
            privacyPolicy.IsAccepted = householdMember.HasAcceptedPrivacyPolicy;
            privacyPolicy.AcceptedTime = householdMember.PrivacyPolicyAcceptedTime;
            householdMember.PrivacyPolicy = privacyPolicy;

            return householdMember;
        }

        /// <summary>
        /// Gets the privacy policies for a given culture.
        /// </summary>
        /// <param name="channel">Channel on which to get the privacy policies.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Privacy policies for the given culture.</returns>
        private PrivacyPolicyModel GetPrivacyPolicies(HouseholdDataServiceChannel channel, CultureInfo cultureInfo)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            var query = new PrivacyPolicyGetQuery
            {
                TranslationInfoIdentifier = GetTranslationInfoIdentifier(channel, cultureInfo)
            };

            StaticTextView privacyPolicyView;
            PrivacyPolicyModel privacyPolicyModel;

            var privacyPolicyModelCache = _objectCache.Get(PrivacyPolicyModelCacheName) as IDictionary<Guid, PrivacyPolicyModel>;
            if (privacyPolicyModelCache == null)
            {
                privacyPolicyView = channel.PrivacyPolicyGet(query);
                privacyPolicyModel = _householdDataConverter.Convert<StaticTextView, PrivacyPolicyModel>(privacyPolicyView);

                privacyPolicyModelCache = new Dictionary<Guid, PrivacyPolicyModel>
                {
                    {query.TranslationInfoIdentifier, privacyPolicyModel}
                };
                _objectCache.Set(PrivacyPolicyModelCacheName, privacyPolicyModelCache, new DateTimeOffset(DateTime.Now.AddHours(1)));

                return (PrivacyPolicyModel) privacyPolicyModelCache[query.TranslationInfoIdentifier].Clone();
            }

            if (privacyPolicyModelCache.ContainsKey(query.TranslationInfoIdentifier))
            {
                return (PrivacyPolicyModel) privacyPolicyModelCache[query.TranslationInfoIdentifier].Clone();
            }

            privacyPolicyView = channel.PrivacyPolicyGet(query);
            privacyPolicyModel = _householdDataConverter.Convert<StaticTextView, PrivacyPolicyModel>(privacyPolicyView);

            privacyPolicyModelCache.Add(query.TranslationInfoIdentifier, privacyPolicyModel);
            _objectCache.Set(PrivacyPolicyModelCacheName, privacyPolicyModelCache, new DateTimeOffset(DateTime.Now.AddHours(1)));

            return (PrivacyPolicyModel) privacyPolicyModelCache[query.TranslationInfoIdentifier].Clone();
        }

        /// <summary>
        /// Gets the identifier for the translation informations which should be used for translations.
        /// </summary>
        /// <param name="channel">Channel on which to get the supported translation informations.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Identifier for the translation informations which should be used for translations.</returns>
        private Guid GetTranslationInfoIdentifier(HouseholdDataServiceChannel channel, CultureInfo cultureInfo)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            var translationInfoIdentifierCache = _objectCache.Get(TranslationInfoIdentifierCacheName) as IDictionary<string, Guid>;
            if (translationInfoIdentifierCache == null)
            {
                var query = new TranslationInfoCollectionGetQuery();
                var result = channel.TranslationInfoGetAll(query);

                translationInfoIdentifierCache = result.ToDictionary(m => m.CultureName, m => m.TranslationInfoIdentifier);

                _objectCache.Set(TranslationInfoIdentifierCacheName, translationInfoIdentifierCache, new DateTimeOffset(DateTime.Now.AddHours(1)));
            }

            if (translationInfoIdentifierCache.ContainsKey(cultureInfo.Name))
            {
                return translationInfoIdentifierCache[cultureInfo.Name];
            }
            return translationInfoIdentifierCache.ContainsKey("en-US") ? translationInfoIdentifierCache["en-US"] : default(Guid);
        }

        /// <summary>
        /// Creates a channel factory with credentials for the given identity.
        /// </summary>
        /// <param name="identity">Identity which should used for the credentials.</param>
        /// <returns>Channel factory.</returns>
        private ChannelFactory<HouseholdDataServiceChannel> CreateChannelFactory(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var channelFactory = new ChannelFactory<HouseholdDataServiceChannel>(HouseholdDataServiceEndpointConfigurationName);
            if (channelFactory.Credentials == null)
            {
                return channelFactory;
            }
            var userNamePasswordCredential = _credentialsProvider.CreateUserNamePasswordCredential(identity);
            channelFactory.Credentials.UserName.UserName = userNamePasswordCredential.UserName;
            channelFactory.Credentials.UserName.Password = userNamePasswordCredential.Password;
            return channelFactory;
        }

        /// <summary>
        /// Gets the household identification collection for a given identity from the cache.
        /// </summary>
        /// <param name="identity">Identity which for which to get the household identification collection from the cache..</param>
        /// <returns>Household identification collection for a given identity from the cache.</returns>
        private IList<HouseholdIdentificationModel> GetHouseholdIdentificationCollection(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var householdIdentificationCollectionCacheName = GetHouseholdIdentificationCollectionCacheName(identity);
            return _objectCache.Get(householdIdentificationCollectionCacheName) as IList<HouseholdIdentificationModel>;
        }

        /// <summary>
        /// Store the household identification collection for a given identity in the cache.
        /// </summary>
        /// <param name="identity">Identity which for which to store the household identification collection in the cache.</param>
        /// <param name="householdIdentificationCollection">The household identification collection to store in the cache.</param>
        private void StoreHouseholdIdentificationCollection(IIdentity identity, IList<HouseholdIdentificationModel> householdIdentificationCollection)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }
            if (householdIdentificationCollection == null)
            {
                throw new ArgumentNullException(nameof(householdIdentificationCollection));
            }

            var householdIdentificationCollectionCacheName = GetHouseholdIdentificationCollectionCacheName(identity);
            _objectCache.Set(householdIdentificationCollectionCacheName, householdIdentificationCollection, DateTimeOffset.Now.AddMinutes(15));
        }

        /// <summary>
        /// Clears the household identification collection for a given identity in the cache.
        /// </summary>
        /// <param name="identity">Identity which for which to clear the household identification collection in the cache.</param>
        private void ClearHouseholdIdentificationCollection(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var householdIdentificationCollectionCacheName = GetHouseholdIdentificationCollectionCacheName(identity);
            if (_objectCache.Contains(householdIdentificationCollectionCacheName) == false)
            {
                return;
            }
            _objectCache.Remove(householdIdentificationCollectionCacheName);
        }

        /// <summary>
        /// Gets the cache name for the household identification collection for a given identity.
        /// </summary>
        /// <param name="identity">Identity which should be used in the cache name.</param>
        /// <returns>Cache name for the household identification collection for a given identity.</returns>
        private string GetHouseholdIdentificationCollectionCacheName(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var userNamePasswordCredential = _credentialsProvider.CreateUserNamePasswordCredential(identity);
            return $"{HouseholdIdentificationCollectionCacheName}:{_credentialsProvider.CalculateHashForCredential(userNamePasswordCredential)}";
        }

        #endregion
    }
}
