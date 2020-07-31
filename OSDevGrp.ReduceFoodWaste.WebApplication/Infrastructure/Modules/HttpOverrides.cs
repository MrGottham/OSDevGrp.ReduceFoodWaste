using System;
using System.Linq;
using System.Web;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Modules
{
    public sealed class HttpOverrides : IHttpModule
    {
        #region Methods

        public void Init(HttpApplication httpApplication)
        {
            if (httpApplication == null)
            {
                throw new ArgumentNullException(nameof(httpApplication));
            }

            httpApplication.BeginRequest += OnBeginRequest;
        }

        public void Dispose()
        {
        }

        private static void OnBeginRequest(object sender, EventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            if (e == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            OnBeginRequest(sender as HttpApplication);
        }

        private static void OnBeginRequest(HttpApplication httpApplication)
        {
            if (httpApplication == null)
            {
                throw new ArgumentNullException(nameof(httpApplication));
            }

            HttpRequest httpRequest = httpApplication.Context.Request;

            string forwardedFor = GetForwardedFor(httpRequest);
            if (string.IsNullOrWhiteSpace(forwardedFor) == false)
            {
                httpRequest.ServerVariables["REMOTE_ADDR"] = forwardedFor;
                httpRequest.ServerVariables["REMOTE_HOST"] = forwardedFor;
            }

            string forwardedProto = GetForwardedProto(httpRequest);
            if (string.IsNullOrWhiteSpace(forwardedProto) == false && forwardedProto == "https")
            {
                httpApplication.Context.Request.ServerVariables["HTTPS"] = "on";
                httpApplication.Context.Request.ServerVariables["SERVER_PORT"] = "443";
                httpApplication.Context.Request.ServerVariables["SERVER_PORT_SECURE"] = "1";
            }
        }

        private static string GetForwardedFor(HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            string headerValue = GetRequestHeader(httpRequest, "X-Forwarded-For");
            return string.IsNullOrWhiteSpace(headerValue) ? null : headerValue.Split(',').FirstOrDefault();
        }

        private static string GetForwardedProto(HttpRequest httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            return GetRequestHeader(httpRequest, "X-Forwarded-Proto");
        }

        private static string GetRequestHeader(HttpRequest httpRequest, string name)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return httpRequest.Headers[name];
        }

        #endregion
    }
}