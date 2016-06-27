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
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CookieConsentAttribute : ActionFilterAttribute
    {
        #region Private constants

        private const string ConsentCookieName = "CookieConsent";

        #endregion

        #region Properties

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

            var viewBag = filterContext.Controller.ViewBag;
            viewBag.AskCookieConsent = true;
            viewBag.HasCookieConsent = false;

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
                    viewBag.AskCookieConsent = false;
                    if (string.Compare(dnt, "0", StringComparison.Ordinal) == 0)
                    {
                        viewBag.HasCookieConsent = true;
                    }
                }
                else
                {
                    if (IsSearchCrawler(request.Headers.Get("User-Agent")))
                    {
                        // Don't ask consent from search engines, also don't set cookies.
                        viewBag.AskCookieConsent = false;
                    }
                    else
                    {
                        // First request on the site and no DNT header.
                        consentCookie = new HttpCookie(ConsentCookieName)
                        {
                            Value = "asked",
                            Expires = DateTime.Now.AddDays(1)
                        };
                        filterContext.HttpContext.Response.Cookies.Add(consentCookie);
                    }
                }
            }
            else
            {
                // We received a consent cookie.
                viewBag.AskCookieConsent = false;
                if (string.Compare(consentCookie.Value, "asked", StringComparison.Ordinal) == 0)
                {
                    // Consent is implicitly given.
                    consentCookie.Value = "true";
                    consentCookie.Expires = DateTime.Now.AddDays(1);
                    filterContext.HttpContext.Response.Cookies.Set(consentCookie);
                    viewBag.HasCookieConsent = true;
                }
                else if (string.Compare(consentCookie.Value, "true", StringComparison.Ordinal) == 0)
                {
                    viewBag.HasCookieConsent = true;
                }
                else
                {
                    // Assume consent denied.
                    viewBag.HasCookieConsent = false;
                }
            }
            
            base.OnActionExecuting(filterContext);
        }

        public static bool AskCookieConsent(ViewContext viewContext)
        {
            return true;

            //if (viewContext == null)
            //{
            //    throw new ArgumentNullException("viewContext");
            //}
            
            //if (viewContext.ViewBag == null)
            //{
            //    return false;
            //}
            //return viewContext.ViewBag.AskCookieConsent ?? false;
        }

        public static bool HasCookieConsent(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            if (viewContext.ViewBag == null)
            {
                return false;
            }
            return viewContext.ViewBag.HasCookieConsent ?? false;
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