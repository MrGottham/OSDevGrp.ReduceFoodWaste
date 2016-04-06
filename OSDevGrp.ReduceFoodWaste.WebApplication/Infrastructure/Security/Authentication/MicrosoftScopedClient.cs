using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication
{
    public class MicrosoftScopedClient : MicrosoftClient
    {
        private class ExtendedMicrosoftClientUserData
        {
            public Emails Emails { get; set; }
        }

        private class Emails
        {
            public string Preferred { get; set; }
            public string Account { get; set; }
            public string Personal { get; set; }
            public string Business { get; set; }
        }

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

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            if (returnUrl == null)
            {
                throw new ArgumentNullException("returnUrl");
            }

            if (_scopes.Any() == false)
            {
                return base.GetServiceLoginUrl(returnUrl);
            }

            var serviceLoginUrl = base.GetServiceLoginUrl(returnUrl);

            var elements = serviceLoginUrl.ToString().Split(new[] {'&'});
            var scopeElement = elements.SingleOrDefault(m => m.StartsWith("scope="));
            if (scopeElement == null)
            {
                return new Uri(string.Format("{0}&{1}", serviceLoginUrl.ToString(), GenerateScopes(string.Empty)));
            }
            return new Uri(serviceLoginUrl.ToString().Replace(scopeElement, GenerateScopes(scopeElement.Substring(scopeElement.IndexOf('=') + 1))));
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException("accessToken");
            }

            var userData = new Dictionary<string, string>(base.GetUserData(accessToken));

            ExtendedMicrosoftClientUserData extendedMicrosoftClientUserData;
            var request = WebRequest.Create(string.Format("https://apis.live.net/v5.0/me?access_token={0}", accessToken));
            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        extendedMicrosoftClientUserData = JsonConvert.DeserializeObject<ExtendedMicrosoftClientUserData>(streamReader.ReadToEnd());
                        streamReader.Close();
                    }
                    responseStream.Close();
                }
                response.Close();
            }

            if (extendedMicrosoftClientUserData == null || extendedMicrosoftClientUserData.Emails == null)
            {
                return userData;
            }
            if (string.IsNullOrWhiteSpace(extendedMicrosoftClientUserData.Emails.Preferred) == false)
            {
                userData.Add("emails.preferred", extendedMicrosoftClientUserData.Emails.Preferred);
            }
            if (string.IsNullOrWhiteSpace(extendedMicrosoftClientUserData.Emails.Account) == false)
            {
                userData.Add("emails.account", extendedMicrosoftClientUserData.Emails.Account);
            }
            if (string.IsNullOrWhiteSpace(extendedMicrosoftClientUserData.Emails.Personal) == false)
            {
                userData.Add("emails.personal", extendedMicrosoftClientUserData.Emails.Personal);
            }
            if (string.IsNullOrWhiteSpace(extendedMicrosoftClientUserData.Emails.Business) == false)
            {
                userData.Add("emails.business", extendedMicrosoftClientUserData.Emails.Business);
            }
            return userData;
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

        #endregion
    }
}