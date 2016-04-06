using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.DependencyInjection;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Bootstrapper.Initialize();
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs eventArgs)
        {
            var formsAuthenticationTicket = Request.ToFormsAuthenticationTicket();
            if (formsAuthenticationTicket == null)
            {
                return;
            }

            var claimsPrincipal = formsAuthenticationTicket.ToClaimsPrincipal();
            if (claimsPrincipal == null)
            {
                return;
            }

            HttpContext.Current.User = claimsPrincipal;
        }
    }
}