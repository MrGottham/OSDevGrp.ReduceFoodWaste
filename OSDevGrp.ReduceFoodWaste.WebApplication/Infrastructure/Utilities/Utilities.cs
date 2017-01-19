using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities
{
    /// <summary>
    /// Utilities which support the infrastructure.
    /// </summary>
    public class Utilities : IUtilities
    {
        /// <summary>
        /// Converts a virtual path to an application absolute path.
        /// </summary>
        /// <param name="virutalPath">The virtual path to convert to an application-relative path.</param>
        /// <returns>The absolute path representation of the specified virtual path.</returns>
        public string ToAbsolutePath(string virutalPath)
        {
            if (string.IsNullOrWhiteSpace(virutalPath))
            {
                throw new ArgumentNullException(nameof(virutalPath));
            }
            return VirtualPathUtility.ToAbsolute(virutalPath);
        }

        /// <summary>
        /// Converts a string to an HTML-encoded string.
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>An encoded string.</returns>
        public string HtmlEncode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string.
        /// </summary>
        /// <param name="value">The string to decode.</param>
        /// <returns>The string to decode.</returns>
        public string HtmlDecode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return HttpUtility.HtmlDecode(value);
        }

        /// <summary>
        /// Converts a given action to an url.
        /// </summary>
        /// <param name="requestContext">The current request context.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValueDictionary">The dictionary which contains the parameters for the action.</param>
        /// <returns>Url for the given action.</returns>
        public string ActionToUrl(RequestContext requestContext, string actionName, string controllerName, RouteValueDictionary routeValueDictionary)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException(nameof(requestContext));
            }
            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException(nameof(actionName));
            }
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new ArgumentNullException(nameof(controllerName));
            }
            var urlHelper = new UrlHelper(requestContext);
            return urlHelper.Action(actionName, controllerName, routeValueDictionary);
        }
    }
}