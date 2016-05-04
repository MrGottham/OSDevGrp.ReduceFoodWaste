using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class CultureAttribute : ActionFilterAttribute
    {
        #region Private constants

        private const string CookieLanguageEntry = "language";

        #endregion

        #region Properties

        private static string CookieName
        {
            get
            {
                return "_Culture";
            }
        }

        #endregion

        #region Methods

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            // Gets the name for the saved or default culture.
            var cultureName = GetSavedCultureOrDefault(filterContext.RequestContext.HttpContext.Request);
            
            // Set culture on the current thread.
            SetCultureOnThread(cultureName);

            // Process as usual.
            base.OnActionExecuting(filterContext);
        }

        public static void SavePrefferedCulture(HttpResponseBase httpResponse, string language, int expireDays = 1)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse");
            }
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException("language");
            }

            var cookie = new HttpCookie(CookieName)
            {
                Expires = DateTime.Now.AddDays(expireDays)
            };
            cookie.Values[CookieLanguageEntry] = language;
            httpResponse.Cookies.Add(cookie);
        }

        internal static string GetSavedCultureOrDefault(HttpRequestBase httpRequest)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException("httpRequest");
            }

            try
            {
                var cookie = httpRequest.Cookies[CookieName];
                return cookie != null ? cookie.Values[CookieLanguageEntry] : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        internal static void SetCultureOnThread(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
            {
                return;
            }
            var cultureInfo = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        #endregion
    }
}