using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
        // http://stackoverflow.com/questions/17168353/getting-email-from-oauth-authentication-microsoft

            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            var microsoftExtraData = new Dictionary<string, object>
            {
                {"Icon", "../Images/microsoft.png"}
            };
            OAuthWebSecurity.RegisterMicrosoftClient(
                clientId: "000000004818F150",
                clientSecret: "akGH0H6GMGveP3WnubHp5rKAMObqFEKJ",
                displayName: "Microsoft",
                extraData: microsoftExtraData);

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
