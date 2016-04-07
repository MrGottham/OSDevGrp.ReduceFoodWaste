using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication
{
    public class FacebookScopedClient : OAuth2Client
    {
        #region Private constants

        private const string Facebook = "facebook";
        private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/auth";
        private const string TokenEndpoint = "https://accounts.google.com/o/oauth2/token";

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
                throw new ArgumentNullException("appId");
            }
            if (string.IsNullOrEmpty(appSecret))
            {
                throw new ArgumentNullException("appSecret");
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
                throw new ArgumentNullException("returnUrl");
            }

            var queryData = HttpUtility.ParseQueryString(string.Empty);
            queryData.Add("response_type", "code");
            queryData.Add("client_id", _appId);
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
            requestData.Add("client_id", _appId);
            requestData.Add("client_secret", _appSecret);

            var webRequest = (HttpWebRequest)WebRequest.Create(TokenEndpoint);
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
            if (state == null || !state.Contains(string.Format("__provider__={0}", Facebook)))
            {
                return;
            }

            var requestData = HttpUtility.ParseQueryString(state);
            requestData.Add(httpContext.Request.QueryString);
            requestData.Remove("state");

            httpContext.RewritePath(string.Format("{0}?{1}", httpContext.Request.Path, requestData));
        }

        #endregion
    }
}