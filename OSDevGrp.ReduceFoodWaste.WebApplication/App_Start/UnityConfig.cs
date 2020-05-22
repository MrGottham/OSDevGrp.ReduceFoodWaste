using System;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Cookies;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;
using Unity;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> _container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => _container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        private static void RegisterTypes(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            RegisterInfrastructureTypes(container);
            RegisterModelHelperTypes(container);
            RegisterRepositoryTypes(container);
        }

        private static void RegisterInfrastructureTypes(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.RegisterType<IUtilities, Utilities>();
            container.RegisterType<IClaimValueProvider, ClaimValueProvider>();
            container.RegisterType<ILocalClaimProvider, LocalClaimProvider>();
            container.RegisterType<ICookieHelper, CookieHelper>();
        }

        private static void RegisterModelHelperTypes(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.RegisterType<IModelHelper, ModelHelper>();
        }

        private static void RegisterRepositoryTypes(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.RegisterInstance(SiteConfiguration.Create());
            container.RegisterInstance(MembershipConfiguration.Create());
            container.RegisterInstance(PaymentConfiguration.Create());

            container.RegisterType<ICredentialsProvider, CredentialsProvider>();
            container.RegisterType<IConfigurationProvider, ConfigurationProvider>();
            container.RegisterType<IHouseholdDataConverter, HouseholdDataConverter>();
            container.RegisterType<IHouseholdDataRepository, HouseholdDataRepository>();
        }
    }
}