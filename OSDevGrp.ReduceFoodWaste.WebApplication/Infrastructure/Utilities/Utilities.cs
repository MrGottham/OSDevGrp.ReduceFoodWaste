using System;
using System.Web;

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
    }
}