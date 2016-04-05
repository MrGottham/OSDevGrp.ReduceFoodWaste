using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Runtime.Serialization.Json;

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
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs eventArgs)
        {
            var formsAuthenticationCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (formsAuthenticationCookie == null)
            {
                return;
            }

            var formsAuthenticationTicket = FormsAuthentication.Decrypt(formsAuthenticationCookie.Value);
            if (string.IsNullOrEmpty(formsAuthenticationTicket.UserData) || formsAuthenticationTicket.UserData == "OAuth")
            {
                return;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var compressedMemoryStream = new MemoryStream(Convert.FromBase64String(formsAuthenticationTicket.UserData)))
                {
                    using (var deflateStream = new DeflateStream(compressedMemoryStream, CompressionMode.Decompress))
                    {
                        deflateStream.CopyTo(memoryStream);
                        deflateStream.Close();
                    }
                    compressedMemoryStream.Close();
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                var serializer = new DataContractJsonSerializer(typeof (IEnumerable<Claim>));
                var claims = (IEnumerable<Claim>) serializer.ReadObject(memoryStream);

                HttpContext.Current.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Forms"));

                memoryStream.Close();
            }
        }
    }
}