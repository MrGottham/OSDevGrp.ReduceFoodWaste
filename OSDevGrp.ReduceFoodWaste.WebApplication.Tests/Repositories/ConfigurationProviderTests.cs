using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories
{
    /// <summary>
    /// Tests the configuration provider.
    /// </summary>
    [TestFixture]
    public class ConfigurationProviderTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the configuration provider.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeConfigurationProvider()
        {
            var configurationProvider = new ConfigurationProvider();
            Assert.That(configurationProvider, Is.Not.Null);
        }
    }
}
