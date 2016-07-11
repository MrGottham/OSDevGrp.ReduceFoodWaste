using System;
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
                    .ForMember(dest => dest.PrivacyPolicy, opt => opt.Ignore());

                configuration.CreateMap<HouseholdModel, HouseholdAddCommand>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.Description) ? null : src.Description))
                    .ForMember(dest => dest.TranslationInfoIdentifier, opt => opt.Ignore())
                    .ForMember(dest => dest.ExtensionData, opt => opt.Ignore());

                configuration.CreateMap<HouseholdMemberView, HouseholdMemberModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.HouseholdMemberIdentifier))
                    .ForMember(dest => dest.Name, opt => opt.Ignore())
                    .ForMember(dest => dest.MailAddress, opt => opt.MapFrom(src => src.MailAddress))
                    .ForMember(dest => dest.ActivationCode, opt => opt.Ignore())
                    .ForMember(dest => dest.IsActivated, opt => opt.Ignore())
                    .ForMember(dest => dest.ActivatedTime, opt => opt.MapFrom(src => src.ActivationTime))
                    .ForMember(dest => dest.Membership, opt => opt.MapFrom(src => src.Membership))
                    .ForMember(dest => dest.MembershipExpireTime, opt => opt.MapFrom(src => src.MembershipExpireTime))
                    .ForMember(dest => dest.PrivacyPolicy, opt => opt.Ignore())
                    .ForMember(dest => dest.HasAcceptedPrivacyPolicy, opt => opt.Ignore())
                    .ForMember(dest => dest.PrivacyPolicyAcceptedTime, opt => opt.MapFrom(src => src.PrivacyPolicyAcceptedTime))
                    .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => src.CreationTime))
                    .ForMember(dest => dest.Households, opt =>
                    {
                        opt.Condition(src => src.Households != null);
                        opt.MapFrom(src => src.Households);
                    });

                configuration.CreateMap<HouseholdMemberModel, HouseholdMemberActivateCommand>()
                    .ConvertUsing(src =>
                    {
                        if (string.IsNullOrWhiteSpace(src.ActivationCode))
                        {
                            throw new ReduceFoodWasteSystemException(Texts.ActivationCodeMustBeGiven);
                        }
                        return new HouseholdMemberActivateCommand
                        {
                            ActivationCode = src.ActivationCode
                        };
                    });

                configuration.CreateMap<PrivacyPolicyModel, HouseholdMemberAcceptPrivacyPolicyCommand>()
                    .ConvertUsing(src =>
                    {
                        if (src.IsAccepted == false)
                        {
                            throw new ReduceFoodWasteSystemException(Texts.PrivacyPoliciesHasNotBeenAccepted);
                        }
                        return new HouseholdMemberAcceptPrivacyPolicyCommand();
                    });

                configuration.CreateMap<BooleanResult, bool>()
                    .ConvertUsing(src => src.Result);

                configuration.CreateMap<StaticTextView, PrivacyPolicyModel>()
                    .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => src.StaticTextIdentifier))
                    .ForMember(dest => dest.Header, opt => opt.MapFrom(src => src.SubjectTranslation))
                    .ForMember(dest => dest.Body, opt => opt.ResolveUsing(src =>
                    {
                        var body = src.BodyTranslation;
                        if (string.IsNullOrEmpty(body))
                        {
                            return body;
                        }
                        body = body.Replace("<html>", string.Empty);
                        body = body.Replace("</html>", string.Empty);
                        return body;
                    }))
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
                throw new ArgumentNullException("source");
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

        #endregion
    }
}