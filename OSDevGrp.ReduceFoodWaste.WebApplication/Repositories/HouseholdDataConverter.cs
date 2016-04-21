using System;
using AutoMapper;
using OSDevGrp.ReduceFoodWaste.WebApplication.HouseholdDataService;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;

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
                    .ForMember(dest => dest.IsAccepted, opt => opt.Ignore());
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
            return _mapper.Map<TSource, TTarget>(source);
        }

        #endregion
    }
}