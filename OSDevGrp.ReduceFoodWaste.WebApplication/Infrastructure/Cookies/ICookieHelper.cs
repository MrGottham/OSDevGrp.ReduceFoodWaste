using System.Web;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Cookies
{
    /// <summary>
    /// Interface for helper functionality to cookies.
    /// </summary>
    public interface ICookieHelper
    {
        /// <summary>
        /// Set the value for acceptance of the cookie consent.
        /// </summary>
        /// <param name="httpResponse">HTTP response.</param>
        /// <param name="consent">Value for acceptance of the cookie consent.</param>
        void SetCookieConsent(HttpResponseBase httpResponse, bool consent);
    }
}
