using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;
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
        [SetUp]
        public void TestInitialize()
        {
            _claimValueProvider = MockRepository.GenerateMock<IClaimValueProvider>();
        }

        /// <summary>
        /// Tests that the constructor initialize the Home controller.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHomeController()
        {
            var homeController = new HomeController(_claimValueProvider);
            Assert.That(homeController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can get values from claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HomeController(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        ///// <summary>
        ///// Tests that Index calls IsAuthenticated on the provider which can get values from claims.
        ///// </summary>
        //[Test]
        //public void TestThatIndexCallsIsAuthenticatedOnClaimValueProvider()
        //{
        //    var homeController = CreateSut();
        //    Assert.That(homeController, Is.Not.Null);

        //    homeController.Index();

        //    _claimValueProvider.AssertWasCalled(m => m.IsAuthenticated(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        //}

        ///// <summary>
        ///// Tests that Index calls IsValidatedHouseholdMember on the provider which can get values from claims.
        ///// </summary>
        //[Test]
        //public void TestThatIsValidatedHouseholdMemberOnClaimValueProvider()
        //{
        //    var homeController = CreateSut();
        //    Assert.That(homeController, Is.Not.Null);

        //    homeController.Index();

        //    _claimValueProvider.AssertWasCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        //}

        ///// <summary>
        ///// Tests that Index calls GetMailAddress on the provider which can get values from claims.
        ///// </summary>
        //[Test]
        //public void TestThatGetMailAddressOnClaimValueProvider()
        //{
        //    var homeController = CreateSut();
        //    Assert.That(homeController, Is.Not.Null);

        //    homeController.Index();

        //    _claimValueProvider.AssertWasCalled(m => m.GetMailAddress(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        //}

        /// <summary>
        /// Creates a home controller for unit testing.
        /// </summary>
        /// <param name="hasUser">Sets whether the controller should have an user.</param>
        /// <param name="hasIdentity">Sets whether the controller should have an user with an identity.</param>
        /// <returns>Home controller for unit testing.</returns>
        private HomeController CreateSut(bool hasUser = true, bool hasIdentity = true)
        {
            var userMock = MockRepository.GenerateMock<IPrincipal>();
            userMock.Stub(m => m.Identity)
                .Return(hasIdentity ? MockRepository.GenerateMock<IIdentity>() : null)
                .Repeat.Any();

            var httpContextMock = MockRepository.GenerateMock<HttpContextBase>();
            httpContextMock.Stub(m => m.User)
                .Return(hasUser ? userMock : null)
                .Repeat.Any();

            var homeController = new HomeController(_claimValueProvider);
            homeController.ControllerContext = new ControllerContext(httpContextMock, new RouteData(), homeController);
            return homeController;
        }
    }
}
