using System;
using System.Web;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Cookies
{
    /// <summary>
    /// Helper functionality to cookies.
    /// </summary>
    public class CookieHelper : ICookieHelper
    {
        /// <summary>
        /// Set the value for acceptance of the cookie consent.
        /// </summary>
        /// <param name="httpResponse">HTTP response.</param>
        /// <param name="consent">Value for acceptance of the cookie consent.</param>
        public void SetCookieConsent(HttpResponseBase httpResponse, bool consent)
        {
            if (httpResponse == null)
            {
                throw new ArgumentNullException("httpResponse");
            }
            CookieConsentAttribute.SetCookieConsent(httpResponse, consent);
        }
    }
}