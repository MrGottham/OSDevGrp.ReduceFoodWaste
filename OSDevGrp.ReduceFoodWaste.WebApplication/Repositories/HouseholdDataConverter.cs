﻿using System;
using AutoMapper;
using OSDevGrp.ReduceFoodWaste.WebApplication.HouseholdDataService;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Converter which can convert household data.
    /// </summary>
    public class HouseholdDataConverter : IHouseholdDataConverter
    {
        #region Private variables

        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a converter which can convert household data.
        /// </summary>
        public HouseholdDataConverter()
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.CreateMap<HouseholdIdentificationView, HouseholdModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.HouseholdIdentifier))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.Ignore())
                    .ForMember(dest => dest.PrivacyPolicy, opt => opt.Ignore())
                    .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
                    .ForMember(dest => dest.HouseholdMembers, opt => opt.Ignore());

                configuration.CreateMap<HouseholdView, HouseholdModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.HouseholdIdentifier))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.PrivacyPolicy, opt => opt.Ignore())
                    .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                    .ForMember(dest => dest.HouseholdMembers, opt =>
                    {
                        opt.Condition(src => src.HouseholdMembers != null);
                        opt.MapFrom(src => src.HouseholdMembers);
                    });

                configuration.CreateMap<HouseholdModel, HouseholdAddCommand>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Description) ? null : src.Description))
                    .ForMember(dest => dest.TranslationInfoIdentifier, opt => opt.Ignore())
                    .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());

                configuration.CreateMap<HouseholdModel, HouseholdUpdateCommand>()
                    .ForMember(dest => dest.HouseholdIdentifier, opt => opt.MapFrom(src => src.Identifier))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Description) ? null : src.Description))
                    .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());

                configuration.CreateMap<MemberOfHouseholdModel, HouseholdAddHouseholdMemberCommand>()
                    .ForMember(dest => dest.HouseholdIdentifier, opt => opt.MapFrom(src => src.HouseholdIdentifier))
                    .ForMember(dest => dest.MailAddress, opt => opt.MapFrom(src => src.MailAddress))
                    .ForMember(dest => dest.TranslationInfoIdentifier, opt => opt.Ignore())
                    .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());

                configuration.CreateMap<MemberOfHouseholdModel, HouseholdRemoveHouseholdMemberCommand>()
                    .ConvertUsing(src => ToHouseholdRemoveHouseholdMemberCommand(src));

                configuration.CreateMap<HouseholdMemberIdentificationView, HouseholdMemberModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.HouseholdMemberIdentifier))
                    .ForMember(dest => dest.Name, opt => opt.Ignore())
                    .ForMember(dest => dest.MailAddress, opt => opt.MapFrom(src => src.MailAddress))
                    .ForMember(dest => dest.ActivationCode, opt => opt.Ignore())
                    .ForMember(dest => dest.IsActivated, opt => opt.Ignore())
                    .ForMember(dest => dest.ActivatedTime, opt => opt.Ignore())
                    .ForMember(dest => dest.Membership, opt => opt.Ignore())
                    .ForMember(dest => dest.MembershipExpireTime, opt => opt.Ignore())
                    .ForMember(dest => dest.CanRenewMembership, opt => opt.Ignore())
                    .ForMember(dest => dest.CanUpgradeMembership, opt => opt.Ignore())
                    .ForMember(dest => dest.PrivacyPolicy, opt => opt.Ignore())
                    .ForMember(dest => dest.HasAcceptedPrivacyPolicy, opt => opt.Ignore())
                    .ForMember(dest => dest.PrivacyPolicyAcceptedTime, opt => opt.Ignore())
                    .ForMember(dest => dest.HasReachedHouseholdLimit, opt => opt.Ignore())
                    .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
                    .ForMember(dest => dest.UpgradeableMemberships, opt => opt.Ignore())
                    .ForMember(dest => dest.Households, opt => opt.Ignore());

                configuration.CreateMap<HouseholdMemberIdentificationView, MemberOfHouseholdModel>()
                    .ForMember(dest => dest.HouseholdMemberIdentifier, opt => opt.MapFrom(src => src.HouseholdMemberIdentifier))
                    .ForMember(dest => dest.HouseholdIdentifier, opt => opt.Ignore())
                    .ForMember(dest => dest.MailAddress, opt => opt.MapFrom(src => src.MailAddress))
                    .ForMember(dest => dest.Removable, opt => opt.Ignore());

                configuration.CreateMap<HouseholdMemberView, HouseholdMemberModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.HouseholdMemberIdentifier))
                    .ForMember(dest => dest.Name, opt => opt.Ignore())
                    .ForMember(dest => dest.MailAddress, opt => opt.MapFrom(src => src.MailAddress))
                    .ForMember(dest => dest.ActivationCode, opt => opt.Ignore())
                    .ForMember(dest => dest.IsActivated, opt => opt.Ignore())
                    .ForMember(dest => dest.ActivatedTime, opt => opt.MapFrom(src => src.ActivationTime))
                    .ForMember(dest => dest.Membership, opt => opt.MapFrom(src => src.Membership))
                    .ForMember(dest => dest.MembershipExpireTime, opt => opt.MapFrom(src => src.MembershipExpireTime))
                    .ForMember(dest => dest.CanRenewMembership, opt => opt.MapFrom(src => src.CanRenewMembership))
                    .ForMember(dest => dest.CanUpgradeMembership, opt => opt.MapFrom(src => src.CanUpgradeMembership))
                    .ForMember(dest => dest.PrivacyPolicy, opt => opt.Ignore())
                    .ForMember(dest => dest.HasAcceptedPrivacyPolicy, opt => opt.Ignore())
                    .ForMember(dest => dest.PrivacyPolicyAcceptedTime, opt => opt.MapFrom(src => src.PrivacyPolicyAcceptedTime))
                    .ForMember(dest => dest.HasReachedHouseholdLimit, opt => opt.MapFrom(src => src.HasReachedHouseholdLimit))
                    .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                    .ForMember(dest => dest.UpgradeableMemberships, opt =>
                    {
                        opt.Condition(src => src.UpgradeableMemberships != null);
                        opt.MapFrom(src => src.UpgradeableMemberships);
                    })
                    .ForMember(dest => dest.Households, opt =>
                    {
                        opt.Condition(src => src.Households != null);
                        opt.MapFrom(src => src.Households);
                    });

                configuration.CreateMap<HouseholdMemberModel, HouseholdMemberActivateCommand>()
                    .ConvertUsing(src => ToHouseholdMemberActivateCommand(src));

                configuration.CreateMap<PrivacyPolicyModel, HouseholdMemberAcceptPrivacyPolicyCommand>()
                    .ConvertUsing(src => ToHouseholdMemberAcceptPrivacyPolicyCommand(src));

                configuration.CreateMap<MembershipModel, HouseholdMemberUpgradeMembershipCommand>()
                    .ForMember(dest => dest.Membership, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.DataProviderIdentifier, opt =>
                    {
                        opt.Condition(src => src.PaymentHandlerIdentifier.HasValue);
                        opt.MapFrom(src => src.PaymentHandlerIdentifier.Value);
                    })
                    .ForMember(dest => dest.PaymentTime, opt =>
                    {
                        opt.Condition(src => src.PaymentTime.HasValue);
                        opt.MapFrom(src => src.PaymentTime.Value);
                    })
                    .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => src.PaymentReference))
                    .ForMember(dest => dest.PaymentReceipt, opt => opt.MapFrom(src => src.PaymentReceipt))
                    .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());

                configuration.CreateMap<BooleanResult, bool>()
                    .ConvertUsing(src => src.Result);

                configuration.CreateMap<DataProviderView, PaymentHandlerModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.DataProviderIdentifier))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.DataSourceStatement, opt => opt.MapFrom(src => src.DataSourceStatement))
                    .ForMember(dest => dest.ActionName, opt => opt.Ignore())
                    .ForMember(dest => dest.ImagePath, opt => opt.Ignore());

                configuration.CreateMap<DataProviderView, DataProviderModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.DataProviderIdentifier))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.DataSourceStatement, opt => opt.MapFrom(src => src.DataSourceStatement));

                configuration.CreateMap<StaticTextView, PrivacyPolicyModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.StaticTextIdentifier))
                    .ForMember(dest => dest.Header, opt => opt.MapFrom(src => src.SubjectTranslation))
                    .ForMember(dest => dest.Body, opt => opt.MapFrom(src => ToBody(src)))
                    .ForMember(dest => dest.IsAccepted, opt => opt.Ignore())
                    .ForMember(dest => dest.AcceptedTime, opt => opt.Ignore());
            });
            mapperConfiguration.AssertConfigurationIsValid();

            _mapper = mapperConfiguration.CreateMapper();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert a source object to a target object.
        /// </summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <param name="source">Source object which should be converted to a target object.</param>
        /// <returns>Target object.</returns>
        public TTarget Convert<TSource, TTarget>(TSource source)
        {
            if (Equals(source, null))
            {
                throw new ArgumentNullException(nameof(source));
            }
            try
            {
                return _mapper.Map<TSource, TTarget>(source);
            }
            catch (AutoMapperMappingException mappingException)
            {
                if (mappingException.InnerException is ReduceFoodWasteExceptionBase)
                {
                    throw mappingException.InnerException;
                }
                throw;
            }
        }

        private static HouseholdRemoveHouseholdMemberCommand ToHouseholdRemoveHouseholdMemberCommand(MemberOfHouseholdModel source)
        {
            if (source.Removable == false)
            {
                throw new ReduceFoodWasteSystemException(Texts.CannotRemoveYourselfAsHouseholdMember);
            }

            return new HouseholdRemoveHouseholdMemberCommand
            {
                HouseholdIdentifier = source.HouseholdIdentifier,
                MailAddress = source.MailAddress
            };
        }

        private static HouseholdMemberActivateCommand ToHouseholdMemberActivateCommand(HouseholdMemberModel source)
        {
            if (string.IsNullOrWhiteSpace(source.ActivationCode))
            {
                throw new ReduceFoodWasteSystemException(Texts.ActivationCodeMustBeGiven);
            }

            return new HouseholdMemberActivateCommand
            {
                ActivationCode = source.ActivationCode
            };
        }

        private static HouseholdMemberAcceptPrivacyPolicyCommand ToHouseholdMemberAcceptPrivacyPolicyCommand(PrivacyPolicyModel source)
        {
            if (source.IsAccepted == false)
            {
                throw new ReduceFoodWasteSystemException(Texts.PrivacyPoliciesHasNotBeenAccepted);
            }

            return new HouseholdMemberAcceptPrivacyPolicyCommand();
        }

        private static string ToBody(StaticTextView source)
        {
            var body = source.BodyTranslation;
            if (string.IsNullOrEmpty(body))
            {
                return body;
            }

            body = body.Replace("<html>", string.Empty);
            body = body.Replace("</html>", string.Empty);
            return body;
        }

        #endregion
    }
}