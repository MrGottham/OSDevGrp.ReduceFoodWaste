using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories.Configuration
{
    /// <summary>
    /// Tests the configuration for the site.
    /// </summary>
    [TestFixture]
    public class SiteConfigurationTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the configuration for site.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeSiteConfiguration()
        {
            ISiteConfiguration siteConfiguration = SiteConfiguration.Create();
            Assert.That(siteConfiguration, Is.Not.Null);
            Assert.That(siteConfiguration.CallbackAddress, Is.Not.Null);
            Assert.That(siteConfiguration.CallbackAddress.AbsoluteUri, Is.Not.Null);
            Assert.That(siteConfiguration.CallbackAddress.AbsoluteUri, Is.Not.Empty);
            Assert.That(siteConfiguration.CallbackAddress.AbsoluteUri, Is.EqualTo("http://localhost:62912/"));
        }
    }
}
