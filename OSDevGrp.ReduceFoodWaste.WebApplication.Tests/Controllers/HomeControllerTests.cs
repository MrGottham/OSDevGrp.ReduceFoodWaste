using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Tests the Home controller.
    /// </summary>
    [TestFixture]
    public class HomeControllerTests : TestBase
    {
        #region Private variables

        private IClaimValueProvider _claimValueProvider;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [OneTimeSetUp]
        public void TestInitialize()
        {
            _claimValueProvider = MockRepository.GenerateMock<IClaimValueProvider>();
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can get values from claims is null.
        /// </summary>
        [Test]
        public void TestTestConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HomeController(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Creates a home controller for unit testing.
        /// </summary>
        /// <returns>Home controller for unit testing.</returns>
        private HomeController CreateSut()
        {
            return new HomeController(_claimValueProvider);
        }
    }
}
