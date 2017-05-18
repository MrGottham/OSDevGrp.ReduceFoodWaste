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
            var siteConfigurationMock = MockRepository.GenerateMock<ISiteConfiguration>();
            Assert.That(siteConfigurationMock, Is.Not.Null);

            var membershipConfigurationMock = MockRepository.GenerateMock<IMembershipConfiguration>();
            Assert.That(membershipConfigurationMock, Is.Not.Null);

            var paymentConfigurationMock = MockRepository.GenerateMock<IPaymentConfiguration>();
            Assert.That(paymentConfigurationMock, Is.Not.Null);

            var configurationProvider = new ConfigurationProvider(siteConfigurationMock, membershipConfigurationMock, paymentConfigurationMock);
            Assert.That(configurationProvider, Is.Not.Null);
            Assert.That(configurationProvider.SiteConfiguration, Is.Not.Null);
            Assert.That(configurationProvider.SiteConfiguration, Is.EqualTo(siteConfigurationMock));
            Assert.That(configurationProvider.MembershipConfiguration, Is.Not.Null);
            Assert.That(configurationProvider.MembershipConfiguration, Is.EqualTo(membershipConfigurationMock));
            Assert.That(configurationProvider.PaymentConfiguration, Is.Not.Null);
            Assert.That(configurationProvider.PaymentConfiguration, Is.EqualTo(paymentConfigurationMock));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the configuration for the site is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenSiteConfigurationIsNull()
        {
            var membershipConfigurationMock = MockRepository.GenerateMock<IMembershipConfiguration>();
            Assert.That(membershipConfigurationMock, Is.Not.Null);

            var paymentConfigurationMock = MockRepository.GenerateMock<IPaymentConfiguration>();
            Assert.That(paymentConfigurationMock, Is.Not.Null);

            // ReSharper disable ObjectCreationAsStatement
            var exeption = Assert.Throws<ArgumentNullException>(() => new ConfigurationProvider(null, membershipConfigurationMock, paymentConfigurationMock));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exeption, Is.Not.Null);
            Assert.That(exeption.ParamName, Is.Not.Null);
            Assert.That(exeption.ParamName, Is.Not.Empty);
            Assert.That(exeption.ParamName, Is.EqualTo("siteConfiguration"));
            Assert.That(exeption.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when configuration of memberships is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenMembershipConfigurationIsNull()
        {
            var siteConfigurationMock = MockRepository.GenerateMock<ISiteConfiguration>();
            Assert.That(siteConfigurationMock, Is.Not.Null);

            var paymentConfigurationMock = MockRepository.GenerateMock<IPaymentConfiguration>();
            Assert.That(paymentConfigurationMock, Is.Not.Null);

            // ReSharper disable ObjectCreationAsStatement
            var exeption = Assert.Throws<ArgumentNullException>(() => new ConfigurationProvider(siteConfigurationMock, null, paymentConfigurationMock));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exeption, Is.Not.Null);
            Assert.That(exeption.ParamName, Is.Not.Null);
            Assert.That(exeption.ParamName, Is.Not.Empty);
            Assert.That(exeption.ParamName, Is.EqualTo("membershipConfiguration"));
            Assert.That(exeption.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when configuration for payments is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenPaymentConfigurationIsNull()
        {
            var siteConfigurationMock = MockRepository.GenerateMock<ISiteConfiguration>();
            Assert.That(siteConfigurationMock, Is.Not.Null);

            var membershipConfigurationMock = MockRepository.GenerateMock<IMembershipConfiguration>();
            Assert.That(membershipConfigurationMock, Is.Not.Null);

            // ReSharper disable ObjectCreationAsStatement
            var exeption = Assert.Throws<ArgumentNullException>(() => new ConfigurationProvider(siteConfigurationMock, membershipConfigurationMock, null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exeption, Is.Not.Null);
            Assert.That(exeption.ParamName, Is.Not.Null);
            Assert.That(exeption.ParamName, Is.Not.Empty);
            Assert.That(exeption.ParamName, Is.EqualTo("paymentConfiguration"));
            Assert.That(exeption.InnerException, Is.Null);
        }
    }
}
