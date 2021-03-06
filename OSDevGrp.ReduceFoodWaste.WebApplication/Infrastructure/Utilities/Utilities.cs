﻿using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// Removes HTML tags from a given string.
        /// </summary>
        /// <param name="value">The string to which to remove HTML tags.</param>
        /// <returns>The string without HTML tags.</returns>
        public string StripHtml(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            Regex regularExpression = new Regex("<.*?>", RegexOptions.Compiled);
            return regularExpression.Replace(value, string.Empty);
        }

        /// <summary>
        /// Replaces a given value with another value in a given url encoded string.
        /// </summary>
        /// <param name="url">The url encoded string where the source value should be replaced with the target value.</param>
        /// <param name="sourceValue">The source value which should be replaced.</param>
        /// <param name="targetValue">The target value which the source value should be replaced with.</param>
        /// <returns>Url encoded string where the source value has been replaced with the target value.</returns>
        public string UrlReplace(string url, string sourceValue, string targetValue)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }
            if (string.IsNullOrWhiteSpace(sourceValue))
            {
                throw new ArgumentNullException(nameof(sourceValue));
            }
            if (string.IsNullOrWhiteSpace(targetValue))
            {
                throw new ArgumentNullException(nameof(targetValue));
            }

            int pos = url.IndexOf("?", StringComparison.Ordinal);
            if (pos < 0)
            {
                return url;
            }

            string newUrl = url.Substring(0, pos + 1);

            NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(url.Substring(pos));
            foreach (string key in nameValueCollection.AllKeys)
            {
                if (string.Compare(key, nameValueCollection.AllKeys.First(), StringComparison.Ordinal) != 0)
                {
                    newUrl = newUrl + '&';
                }
                newUrl = newUrl + $"{key}={HttpUtility.UrlEncode(nameValueCollection[key].Replace(sourceValue, targetValue), Encoding.UTF8)}";
            }

            return newUrl;
        }

        /// <summary>
        /// Converts a given action to an URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper which can be used to generate URLs by using routing.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValueDictionary">The dictionary which contains the parameters for the action.</param>
        /// <returns>URL for the given action.</returns>
        public string ActionToUrl(UrlHelper urlHelper, string actionName, string controllerName, RouteValueDictionary routeValueDictionary)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }
            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException(nameof(actionName));
            }
            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new ArgumentNullException(nameof(controllerName));
            }
            return urlHelper.Action(actionName, controllerName, routeValueDictionary);
        }
    }
}