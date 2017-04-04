using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication
{
    public class FacebookScopedClient : OAuth2Client
    {
        #region Private constants

        private const string Facebook = "facebook";
        private const string AuthorizationEndpoint = "https://www.facebook.com/dialog/oauth";
        private const string TokenEndpoint = "https://graph.facebook.com/oauth/access_token";

        #endregion

        #region Private variables

        private readonly string _appId;
        private readonly string _appSecret;

        #endregion

        #region Constructor

        public FacebookScopedClient(string appId, string appSecret) : 
            base(Facebook)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId));
            }
            if (string.IsNullOrEmpty(appSecret))
            {
                throw new ArgumentNullException(nameof(appSecret));
            }
            _appId = appId;
            _appSecret = appSecret;
        }

        #endregion

        #region Methods

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            if (returnUrl == null)
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }

            var queryData = HttpUtility.ParseQueryString(string.Empty);
            queryData.Add("client_id", _appId);
            queryData.Add("redirect_uri", returnUrl.AbsoluteUri);
            queryData.Add("scope", "public_profile,email");
            queryData.Add("display", "page");

            return new Uri($"{AuthorizationEndpoint}?{queryData}");
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            var request = WebRequest.Create($"https://graph.facebook.com/me?access_token={Uri.EscapeDataString(accessToken)}&fields=id,name,first_name,last_name,link,gender,email");
            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var streamReader = new StreamReader(responseStream))
                    {
                        return JsonConvert.DeserializeObject<Dictionary<string, string>>(streamReader.ReadToEnd());
                    }
                }
            }
        }

        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            if (returnUrl == null)
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }
            if (string.IsNullOrEmpty(authorizationCode))
            {
                throw new ArgumentNullException(nameof(authorizationCode));
            }

            var requestData = HttpUtility.ParseQueryString(string.Empty);
            requestData.Add("client_id", _appId);
            requestData.Add("client_secret", _appSecret);
            requestData.Add("redirect_uri", returnUrl.AbsoluteUri);
            requestData.Add("code", authorizationCode);

            var webRequest = (HttpWebRequest) WebRequest.Create(TokenEndpoint);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            using (var requestStream = webRequest.GetRequestStream())
            {
                using (var streamWrtier = new StreamWriter(requestStream))
                {
                    streamWrtier.Write(requestData);
                }
            }
            using (var webResponse = webRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var nameValueCollection = JsonConvert.DeserializeObject<Dictionary<string, string>>(streamReader.ReadToEnd());
                    return nameValueCollection["access_token"];
                }
            }
        }

         #endregion
    }
}