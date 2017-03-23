using System.Web.Mvc;
using System.Web.Routing;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities
{
    /// <summary>
    /// Interface for utilities which support the infrastructure.
    /// </summary>
    public interface IUtilities
    {
        /// <summary>
        /// Removes HTML tags from a given string.
        /// </summary>
        /// <param name="value">The string to which to remove HTML tags.</param>
        /// <returns>The string without HTML tags.</returns>
        string StripHtml(string value);

        /// <summary>
        /// Replaces a given value with another value in a given url encoded string.
        /// </summary>
        /// <param name="url">The url encoded string where the source value should be replaced with the target value.</param>
        /// <param name="sourceValue">The source value which should be replaced.</param>
        /// <param name="targetValue">The target value which the source value should be replaced with.</param>
        /// <returns>Url encoded string where the source value has been replaced with the target value.</returns>
        string UrlReplace(string url, string sourceValue, string targetValue);

        /// <summary>
        /// Converts a given action to an URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper which can be used to generate URLs by using routing.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValueDictionary">The dictionary which contains the parameters for the action.</param>
        /// <returns>URL for the given action.</returns>
        string ActionToUrl(UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValueDictionary);
    }
}
