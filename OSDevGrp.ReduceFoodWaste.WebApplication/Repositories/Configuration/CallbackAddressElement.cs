using System.Configuration;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Callback address configuration element.
    /// </summary>
    public class CallbackAddressElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets the callback address for the site.
        /// </summary>
        [ConfigurationProperty("value", DefaultValue = "http://localhost", IsRequired = true)]
        [RegexStringValidator(@"^(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?$")]
        public string Value => (string) this["value"];

        #endregion
    }
}