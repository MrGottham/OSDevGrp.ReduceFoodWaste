using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories.Configuration
{
    /// <summary>
    /// Tests the configuration for memberships.
    /// </summary>
    [TestFixture]
    public class MembershipConfigurationTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the configuration for memberships-
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeMembershipConfiguration()
        {
            var membershipConfiguration = new MembershipConfiguration();
            Assert.That(membershipConfiguration, Is.Not.Null);
        }
    }
}
