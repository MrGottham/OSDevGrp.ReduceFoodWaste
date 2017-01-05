namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities
{
    /// <summary>
    /// Interface for utilities which support the infrastructure.
    /// </summary>
    public interface IUtilities
    {
        /// <summary>
        /// Converts a virtual path to an application absolute path.
        /// </summary>
        /// <param name="virutalPath">The virtual path to convert to an application-relative path.</param>
        /// <returns>The absolute path representation of the specified virtual path.</returns>
        string ToAbsolutePath(string virutalPath);

        /// <summary>
        /// Converts a string to an HTML-encoded string.
        /// </summary>
        /// <param name="value">The string to encode.</param>
        /// <returns>An encoded string.</returns>
        string HtmlEncode(string value);

        /// <summary>
        /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string.
        /// </summary>
        /// <param name="value">The string to decode.</param>
        /// <returns>The string to decode.</returns>
        string HtmlDecode(string value);
    }
}
