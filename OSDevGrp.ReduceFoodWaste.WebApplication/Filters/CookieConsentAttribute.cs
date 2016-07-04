using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Filters
{
    /// <summary>
    /// ASP.NET MVC FilterAttribute for implementing the European cookie-law.
    /// https://www.macaw.nl/artikelen/implementing-european-cookie-law-compliance-in-asp-net-mvc
    /// https://gist.github.com/Maarten88/5778402
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CookieConsentAttribute : ActionFilterAttribute
    {
        #region Private constants

        public const string ConsentCookieName = "CookieConsent";
        private const string ConsentContextName = "CookieConsentInfo";

        #endregion

        #region Helper class

        private class CookieConsentInfo
        {
            #region Constructor

            public CookieConsentInfo()
            {
                NeedToAskConsent = true;
                HasConsent = false;
            }

            #endregion

            #region Properties

            public bool NeedToAskConsent { get; set; }
            public bool HasConsent { get; set; }

            #endregion
        }

        #endregion

        #region Constructor

        public CookieConsentAttribute()
        {
            ImplicitlyAllowCookies = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Some government relax their interpretation of the law somewhat:
        /// After the first page with the message, clicking anything other than the cookie refusal link may be interpreted as implicitly allowing cookies.
        /// </summary>
        public bool ImplicitlyAllowCookies { get; private set; }

        private static IEnumerable<string> Crawlers
        {
            get
            {
                return new List<string>
                {
                    "Baiduspider",
                    "Googlebot",
                    "YandexBot",
                    "YandexImages",
                    "bingbot",
                    "msnbot",
                    "Vagabondo",
                    "SeznamBot",
                    "ia_archiver",
                    "AcoonBot",
                    "Yahoo! Slurp",
                    "AhrefsBot"
                };
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

            var cookieConsentInfo = new CookieConsentInfo();

            var request = filterContext.HttpContext.Request;

            // Check if the user has a consent cookie.
            var consentCookie = request.Cookies[ConsentCookieName];
            if (consentCookie == null)
            {
                // No consent cookie. 
                // We first check the Do Not Track header value, this can have the value "0" or "1".
                var dnt = request.Headers.Get("DNT");

                // If we receive a DNT header, we accept its value and do not ask the user anymore.
                if (string.IsNullOrEmpty(dnt) == false)
                {
                    cookieConsentInfo.NeedToAskConsent = false;
                    if (string.Compare(dnt, "0", StringComparison.Ordinal) == 0)
                    {
                        cookieConsentInfo.HasConsent = true;
                    }
                }
                else
                {
                    if (IsSearchCrawler(request.Headers.Get("User-Agent")))
                    {
                        // Don't ask consent from search engines, also don't set cookies.
                        cookieConsentInfo.NeedToAskConsent = false;
                    }
                    else
                    {
                        // First request on the site and no DNT header.
                        consentCookie = new HttpCookie(ConsentCookieName)
                        {
                            Value = "asked",
                            Expires = DateTime.Now
                        };
                        filterContext.HttpContext.Response.Cookies.Add(consentCookie);
                    }
                }
            }
            else
            {
                // We received a consent cookie.
                cookieConsentInfo.NeedToAskConsent = false;
                if (ImplicitlyAllowCookies && string.Compare(consentCookie.Value, "asked", StringComparison.Ordinal) == 0)
                {
                    // Consent is implicitly given.
                    consentCookie.Value = "true";
                    consentCookie.Expires = DateTime.Now.AddDays(1);
                    filterContext.HttpContext.Response.Cookies.Set(consentCookie);
                    
                    cookieConsentInfo.HasConsent = true;
                }
                else if (string.Compare(consentCookie.Value, "true", StringComparison.Ordinal) == 0)
                {
                    cookieConsentInfo.HasConsent = true;
                }
                else
                {
                    // Assume consent denied.
                    cookieConsentInfo.HasConsent = false;
                }
            }

            HttpContext.Current.Items[ConsentContextName] = cookieConsentInfo;
            
            base.OnActionExecuting(filterContext);
        }

        public static bool AskCookieConsent(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            var cookieConsentInfo = viewContext.HttpContext.Items[ConsentContextName] as CookieConsentInfo ?? new CookieConsentInfo();
            return cookieConsentInfo.NeedToAskConsent;
        }

        public static bool HasCookieConsent(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            var cookieConsentInfo = viewContext.HttpContext.Items[ConsentContextName] as CookieConsentInfo ?? new CookieConsentInfo();
            return cookieConsentInfo.HasConsent;
        }

        public static void SetCookieConsent(HttpResponseBase httpResponse, bool consent)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse");
            }

            var consentCookie = new HttpCookie(ConsentCookieName)
            {
                Value = consent ? "true" : "false",
                Expires = DateTime.Now.AddDays(1)
            };
            httpResponse.Cookies.Set(consentCookie);
        }

        private static bool IsSearchCrawler(string userAgent)
        {
            return string.IsNullOrWhiteSpace(userAgent) == false && Crawlers.Any(userAgent.Contains);
        }

        #endregion
    }
}