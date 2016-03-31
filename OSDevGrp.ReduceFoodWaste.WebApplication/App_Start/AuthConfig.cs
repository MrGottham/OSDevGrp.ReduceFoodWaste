using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Helpers;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using Microsoft.Web.WebPages.OAuth;
using Newtonsoft.Json;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public static class AuthConfig
    {
        private class MicrosoftScopedClient : MicrosoftClient
        {
            #region Private variables

            private readonly IEnumerable<string> _scopes;

            #endregion

            #region Constructors

            public MicrosoftScopedClient(string appId, string appSecret, IEnumerable<string> scopes) 
                : base(appId, appSecret)
            {
                if (scopes == null)
                {
                    throw new ArgumentNullException("scopes");
                }
                _scopes = scopes;
            }

            #endregion

            #region Methods

            public override void RequestAuthentication(HttpContextBase context, Uri returnUrl)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (returnUrl == null)
                {
                    throw new ArgumentNullException("returnUrl");
                }

                if (_scopes.Any() == false)
                {
                    base.RequestAuthentication(context, returnUrl);
                    return;
                }

                var serviceLoginUrl = base.GetServiceLoginUrl(returnUrl);

                var elements = serviceLoginUrl.ToString().Split(new[] {'&'});
                var scopeElement = elements.SingleOrDefault(m => m.StartsWith("scope="));
                if (scopeElement == null)
                {
                    context.Response.Redirect(string.Format("{0}&{1}", serviceLoginUrl.ToString(), GenerateScopes(string.Empty)));
                    return;
                }

                var s = serviceLoginUrl.ToString().Replace(scopeElement, GenerateScopes(scopeElement.Substring(scopeElement.IndexOf('=') + 1)));
                context.Response.Redirect(s);
            }

            public override AuthenticationResult VerifyAuthentication(HttpContextBase context, Uri returnPageUrl)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (returnPageUrl == null)
                {
                    throw new ArgumentNullException("returnPageUrl");
                }

                var result = base.VerifyAuthentication(context, returnPageUrl);
                if (result == null || result.ExtraData == null || result.ExtraData.ContainsKey("accesstoken") == false)
                {
                    return result;
                }

                var request = WebRequest.Create(string.Format("https://apis.live.net/v5.0/me?access_token={0}", result.ExtraData["accesstoken"]));
                using (var response = request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var streamReader = new StreamReader(responseStream))
                        {
                            var data = streamReader.ReadToEnd();

                            streamReader.Close();
                        }
                        responseStream.Close();
                    }
                    response.Close();
                }

//                var x = JsonConvert.DeserializeObject<ExtendedMicrosoftClientUserData>();

                return result;

            }

            private string GenerateScopes(string existingScopes)
            {
                if (existingScopes == null)
                {
                    throw new ArgumentNullException("existingScopes");
                }
                var scopeBuilder = new StringBuilder(HttpUtility.UrlDecode(existingScopes));
                foreach (var scope in _scopes.Where(m => string.IsNullOrWhiteSpace(m) == false))
                {
                    if (existingScopes.Contains(scope))
                    {
                        continue;
                    }
                    if (scopeBuilder.Length == 0)
                    {
                        scopeBuilder.Append(scope);
                        continue;
                    }
                    scopeBuilder.AppendFormat(" {0}", scope);
                }
                return string.Format("scope={0}", scopeBuilder.ToString());
            }

            protected class ExtendedMicrosoftClientUserData
            {
                public string FirstName { get; set; }
                public string Gender { get; set; }
                public string Id { get; set; }
                public string LastName { get; set; }
                public Uri Link { get; set; }
                public string Name { get; set; }
                public Emails Emails { get; set; }
            }

            protected class Emails
            {
                public string Preferred { get; set; }
                public string Account { get; set; }
                public string Personal { get; set; }
                public string Business { get; set; }
            }
            #endregion
        }
        
        public static void RegisterAuth()
        {
            // http://stackoverflow.com/questions/17168353/getting-email-from-oauth-authentication-microsoft

            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            var microsoftScopedClient = new MicrosoftScopedClient("000000004818F150", "akGH0H6GMGveP3WnubHp5rKAMObqFEKJ", new List<string> {"wl.basic"});
            var microsoftExtraData = new Dictionary<string, object>
            {
                {"Icon", "../Images/microsoft.png"}
            };
            OAuthWebSecurity.RegisterClient(microsoftScopedClient, "Microsoft", microsoftExtraData);
            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "000000004818F150",
            //    clientSecret: "akGH0H6GMGveP3WnubHp5rKAMObqFEKJ",
            //    displayName: "Microsoft",
            //    extraData: microsoftExtraData);

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
