using System.Collections.Generic;
using System.Web;
using Microsoft.Web.WebPages.OAuth;
using OSDevGrp.ReduceFoodWaste.WebApplication.Security.Authentication;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            var microsoftScopedClient = new MicrosoftScopedClient("000000004818F150", "akGH0H6GMGveP3WnubHp5rKAMObqFEKJ", new List<string> {"wl.basic"});
            var microsoftExtraData = new Dictionary<string, object>
            {
                {"Icon", VirtualPathUtility.ToAbsolute("~/Images/microsoft.png")}
            };
            OAuthWebSecurity.RegisterClient(microsoftScopedClient, "Microsoft", microsoftExtraData);

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
