using System;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading.Tasks;
using OSDevGrp.ReduceFoodWaste.WebApplication.HouseholdDataService;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Repository which can access household data.
    /// </summary>
    public class HouseholdDataRepository : IHouseholdDataRepository
    {
        #region Private constants

        private const string HouseholdDataServiceEndpointConfigurationName = "HouseholdDataService";

        #endregion

        #region Constructor

        public HouseholdDataRepository()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determinates whether a given identity has been created as a household member.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the given identity has been created as a household member otherwise false.</returns>
        public Task<bool> IsHouseholdMemberCreated(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            Func<bool> func = () =>
            {
                using (var channelFactory = new ChannelFactory<HouseholdDataServiceChannel>(HouseholdDataServiceEndpointConfigurationName))
                {
                    using (var channel = channelFactory.CreateChannel())
                    {
                        return false;
                    }
                }
            };

            return Task.Run(func);
        }

        #endregion
    }
}