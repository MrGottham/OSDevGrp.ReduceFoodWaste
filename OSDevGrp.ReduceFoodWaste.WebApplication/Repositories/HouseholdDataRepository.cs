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

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Repository which can access household data.
    /// </summary>
    public class HouseholdDataRepository : IHouseholdDataRepository
    {
        #region Private constants

        private const string HouseholdDataServiceEndpointConfigurationName = "HouseholdDataService";
        private const string TranslationInfoIdentifierCacheName = "OSDevGrp.ReduceFoodWaste.WebApplication.TranslationInfoIdentifierCache";
        private const string PrivacyPolicyModelCacheName = "OSDevGrp.ReduceFoodWaste.WebApplication.PrivacyPolicyModelCache";

        #endregion

        #region Private variables

        private readonly ICredentialsProvider _credentialsProvider;
        private readonly IHouseholdDataConverter _householdDataConverter;
        private readonly ObjectCache _objectCache = MemoryCache.Default; 

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a repository which can access household data.
        /// </summary>
        /// <param name="credentialsProvider">Implementation of a provider which can creates credentials.</param>
        /// <param name="householdDataConverter">Implementation of a converter which can convert household data.</param>
        public HouseholdDataRepository(ICredentialsProvider credentialsProvider, IHouseholdDataConverter householdDataConverter)
        {
            if (credentialsProvider == null)
            {
                throw new ArgumentNullException("credentialsProvider");
            }
            if (householdDataConverter == null)
            {
                throw new ArgumentNullException("householdDataConverter");
            }
            _credentialsProvider = credentialsProvider;
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
                throw new ArgumentNullException("identity");
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
                throw new ArgumentNullException("identity");
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
                throw new ArgumentNullException("identity");
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
        /// Creates a new household to a given identity.
        /// </summary>
        /// <param name="identity">Identity which should own the household.</param>
        /// <param name="householdModel">Model for the household to create.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Model for the created household.</returns>
        public Task<HouseholdModel> CreateHousehold(IIdentity identity, HouseholdModel householdModel, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
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
                throw new ArgumentNullException("identity");
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException("cultureInfo");
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
                throw new ArgumentNullException("identity");
            }
            if (methodBase == null)
            {
                throw new ArgumentNullException("methodBase");
            }
            if (callbackFunc == null)
            {
                throw new ArgumentNullException("callbackFunc");
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
                        var dispoableChannel = channel as IDisposable;
                        if (dispoableChannel != null)
                        {
                            dispoableChannel.Dispose();
                        }
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
        /// Gets the privacy policies for a given culture.
        /// </summary>
        /// <param name="channel">Channel on which to get the privacy policies.</param>
        /// <param name="cultureInfo">Culture informations which should be used for translation.</param>
        /// <returns>Privacy policies for the given culture.</returns>
        private PrivacyPolicyModel GetPrivacyPolicies(HouseholdDataServiceChannel channel, CultureInfo cultureInfo)
        {
            if (channel == null)
            {
                throw new ArgumentNullException("channel");
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException("cultureInfo");
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
                throw new ArgumentNullException("channel");
            }
            if (cultureInfo == null)
            {
                throw new ArgumentNullException("cultureInfo");
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
            var channelFactory = new ChannelFactory<HouseholdDataServiceChannel>(HouseholdDataServiceEndpointConfigurationName);

            var userNamePasswordCredential = _credentialsProvider.CreateUserNamePasswordCredential(identity);
            channelFactory.Credentials.UserName.UserName = userNamePasswordCredential.UserName;
            channelFactory.Credentials.UserName.Password = userNamePasswordCredential.Password;
            
            return channelFactory;
        }

        #endregion
    }
}