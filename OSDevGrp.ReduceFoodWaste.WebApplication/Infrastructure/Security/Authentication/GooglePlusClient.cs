using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication
{
    public class GooglePlusClient : OAuth2Client
    {

        #region Private constants

        private const string Google = "google";
        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/auth";
        private const string TokenEndpoint = "https://accounts.google.com/o/oauth2/token";

        #endregion

        #region Private variables

        private readonly string _clientId;
        private readonly string _clientSecret;

        #endregion

        #region Constructor

        public GooglePlusClient(string clientId, string clientSecret) : 
            base(Google)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException("clientId");
            }
            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException("clientSecret");
            }
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        #endregion

        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            if (returnUrl == null)
            {
                throw new ArgumentNullException("returnUrl");
            }

            var queryData = HttpUtility.ParseQueryString(string.Empty);
            queryData.Add("response_type", "code");
            queryData.Add("client_id", _clientId);
            queryData.Add("scope", "email");
            queryData.Add("redirect_uri", GetRedirectUri(returnUrl));
            if (string.IsNullOrEmpty(returnUrl.Query) == false)
            {
                queryData.Add("state", returnUrl.Query.Substring(1));
            }

            return new Uri(string.Format("{0}?{1}", AuthorizationEndpoint, queryData));
        }

        protected override IDictionary<string, string> GetUserData(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException("accessToken");
            }

            var request = WebRequest.Create(string.Format("https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}", Uri.EscapeDataString(accessToken)));
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
                throw new ArgumentNullException("returnUrl");
            }
            if (string.IsNullOrEmpty(authorizationCode))
            {
                throw new ArgumentNullException("authorizationCode");
            }

            var requestData = HttpUtility.ParseQueryString(string.Empty);
            requestData.Add("grant_type", "authorization_code");
            requestData.Add("code", authorizationCode);
            requestData.Add("redirect_uri", GetRedirectUri(returnUrl));
            requestData.Add("client_id", _clientId);
            requestData.Add("client_secret", _clientSecret);

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
                    var json = JObject.Parse(streamReader.ReadToEnd());
                    return json.Value<string>("access_token");
                }
            }
        }

        private static string GetRedirectUri(Uri returnUrl)
        {
            if (returnUrl == null)
            {
                throw new ArgumentNullException("returnUrl");
            }
            return returnUrl.GetLeftPart(UriPartial.Path);
        }

        public static void RewriteRequest(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var state = HttpUtility.UrlDecode(httpContext.Request.QueryString["state"]);
            if (state == null || !state.Contains(string.Format("__provider__={0}", Google)))
            {
                return;
            }

            var requestData = HttpUtility.ParseQueryString(state);
            requestData.Add(httpContext.Request.QueryString);
            requestData.Remove("state");

            httpContext.RewritePath(string.Format("{0}?{1}", httpContext.Request.Path, requestData));
        }
    }
}