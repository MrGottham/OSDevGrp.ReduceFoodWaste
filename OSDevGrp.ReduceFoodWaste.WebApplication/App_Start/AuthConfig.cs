using System.Collections.Generic;
using System.Web;
using Microsoft.Web.WebPages.OAuth;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            var microsoftScopedClient = new MicrosoftScopedClient("000000004818F090", "Ht2m6n7P83LT4rgfXwgLbIGqQ3SKoqSB", new List<string> {"wl.basic", "wl.emails"});
            var microsoftExtraData = new Dictionary<string, object>
            {
                {"Icon", VirtualPathUtility.ToAbsolute("~/Images/microsoft.png")}
            };
            OAuthWebSecurity.RegisterClient(microsoftScopedClient, "Microsoft", microsoftExtraData);

            var googlePlusClient = new GooglePlusClient("913030417905-08vqe17eck6s2nf0jl56ls91plqlvaou.apps.googleusercontent.com", "2mHZVafOFxKkTiCRghfpeiqi");
            var googleExtraData = new Dictionary<string, object>
            {
                {"Icon", VirtualPathUtility.ToAbsolute("~/Images/google.png")}
            };
            OAuthWebSecurity.RegisterClient(googlePlusClient, "Google", googleExtraData);

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");
        }
    }
}
