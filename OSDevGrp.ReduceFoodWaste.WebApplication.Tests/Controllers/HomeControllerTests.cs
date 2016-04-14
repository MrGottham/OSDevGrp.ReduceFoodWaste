using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;
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

        private IClaimValueProvider _claimValueProviderMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
        }

        /// <summary>
        /// Tests that the constructor initialize the Home controller.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHomeController()
        {
            var homeController = new HomeController(_claimValueProviderMock);
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

        /// <summary>
        /// Tests that Index does not call IsValidatedHouseholdMember on the provider which can get values from claims when the controller does not have a user.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsValidatedHouseholdMemberOnClaimValueProviderWhenControllerDoesNotHaveUser()
        {
            var homeController = CreateHomeController(false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index returns a ViewResult with the welcome message when the controller does not have a user.
        /// </summary>
        [Test]
        public void TestThatIndexReturnsViewResultWithWelcomeMessageWhenControllerDoesNotHaveUser()
        {
            var homeController = CreateHomeController(false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Null);

            var result = homeController.Index();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Empty);
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["Message"], Is.Not.Null);
            Assert.That(viewResult.ViewData["Message"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["Message"], Is.EqualTo(string.Format(Texts.WelcomeTo, Texts.ReduceFoodWasteProject)));
        }

        /// <summary>
        /// Tests that Index does not call IsValidatedHouseholdMember on the provider which can get values from claims when the controller does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsValidatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithoutIdentity()
        {
            var homeController = CreateHomeController(true, false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index returns a ViewResult with the welcome message when the controller does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatIndexReturnsViewResultWithWelcomeMessageWhenControllerDoesHaveUserWithoutIdentity()
        {
            var homeController = CreateHomeController(true, false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Null);

            var result = homeController.Index();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Empty);
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["Message"], Is.Not.Null);
            Assert.That(viewResult.ViewData["Message"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["Message"], Is.EqualTo(string.Format(Texts.WelcomeTo, Texts.ReduceFoodWasteProject)));
        }

        /// <summary>
        /// Tests that Index calls IsValidatedHouseholdMember on the provider which can get values from claims when the controller does have a user with an identity.
        /// </summary>
        [Test]
        public void TestThatIndexCallsIsValidatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithIdentity()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        }

        /// <summary>
        /// Creates a home controller for unit testing.
        /// </summary>
        /// <param name="hasUser">Sets whether the controller should have an user.</param>
        /// <param name="hasIdentity">Sets whether the controller should have an user with an identity.</param>
        /// <param name="isValidatedHouseholdMember">Sets whether the user is a validated household member.</param>
        /// <returns>Home controller for unit testing.</returns>
        private HomeController CreateHomeController(bool hasUser = true, bool hasIdentity = true, bool isValidatedHouseholdMember = false)
        {
            _claimValueProviderMock.Stub(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything))
                .Return(isValidatedHouseholdMember)
                .Repeat.Any();

            var userMock = MockRepository.GenerateMock<IPrincipal>();
            userMock.Stub(m => m.Identity)
                .Return(hasIdentity ? MockRepository.GenerateMock<IIdentity>() : null)
                .Repeat.Any();

            var httpContextMock = MockRepository.GenerateMock<HttpContextBase>();
            httpContextMock.Stub(m => m.User)
                .Return(hasUser ? userMock : null)
                .Repeat.Any();

            var homeController = new HomeController(_claimValueProviderMock);
            homeController.ControllerContext = new ControllerContext(httpContextMock, new RouteData(), homeController);
            return homeController;
        }
    }
}
