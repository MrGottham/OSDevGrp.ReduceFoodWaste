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
            var microsoftScopedClient = new MicrosoftScopedClient("000000004818F090", "Ht2m6n7P83LT4rgfXwgLbIGqQ3SKoqSB", new List<string> {"wl.basic", "wl.emails"});
            var microsoftExtraData = new Dictionary<string, object>
            {
                {"Icon", VirtualPathUtility.ToAbsolute("~/Images/microsoft.png")}
            };
            OAuthWebSecurity.RegisterClient(microsoftScopedClient, "Microsoft", microsoftExtraData);

            var googlePlusScopedClient = new GooglePlusScopedClient("913030417905-08vqe17eck6s2nf0jl56ls91plqlvaou.apps.googleusercontent.com", "2mHZVafOFxKkTiCRghfpeiqi");
            var googlePlusExtraData = new Dictionary<string, object>
            {
                {"Icon", VirtualPathUtility.ToAbsolute("~/Images/google.png")}
            };
            OAuthWebSecurity.RegisterClient(googlePlusScopedClient, "Google", googlePlusExtraData);

            var facebookClient = new FacebookScopedClient("1065427026850786", "ad34aaf2adb862bdd7d690a8ee355a29");
            var facebookExtraData = new Dictionary<string, object>
            {
                {"Icon", VirtualPathUtility.ToAbsolute("~/Images/facebook.png")}
            };
            OAuthWebSecurity.RegisterClient(facebookClient, "Facebook", facebookExtraData);
        }
    }
}
