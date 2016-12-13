using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

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
            var membershipConfigurationMock = MockRepository.GenerateMock<IMembershipConfiguration>();
            Assert.That(membershipConfigurationMock, Is.Not.Null);

            var configurationProvider = new ConfigurationProvider(membershipConfigurationMock);
            Assert.That(configurationProvider, Is.Not.Null);
            Assert.That(configurationProvider.MembershipConfiguration, Is.Not.Null);
            Assert.That(configurationProvider.MembershipConfiguration, Is.EqualTo(membershipConfigurationMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when configuration of memberships is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenMembershipConfigurationIsNull()
        {
            var exeption = Assert.Throws<ArgumentNullException>(() => new ConfigurationProvider(null));
            Assert.That(exeption, Is.Not.Null);
            Assert.That(exeption.ParamName, Is.Not.Null);
            Assert.That(exeption.ParamName, Is.Not.Empty);
            Assert.That(exeption.ParamName, Is.EqualTo("membershipConfiguration"));
            Assert.That(exeption.InnerException, Is.Null);
        }
    }
}
