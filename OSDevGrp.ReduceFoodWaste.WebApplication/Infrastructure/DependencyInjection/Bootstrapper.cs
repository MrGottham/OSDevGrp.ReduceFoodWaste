using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.DependencyInjection
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialize()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            RegisterInfrastructureTypes(container);

            return container;
        }

        private static void RegisterInfrastructureTypes(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            container.RegisterType<IClaimValueProvider, ClaimValueProvider>();
        }
    }
}