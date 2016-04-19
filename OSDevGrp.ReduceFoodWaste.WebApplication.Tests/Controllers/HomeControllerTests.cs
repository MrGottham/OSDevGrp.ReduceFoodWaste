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
        /// Tests that Index does not call IsAuthenticated on the provider which can get values from claims when the controller does not have a user.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsAuthenticatedOnClaimValueProviderWhenControllerDoesNotHaveUser()
        {
            var homeController = CreateHomeController(false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsAuthenticated(Arg<IIdentity>.Is.Anything));
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
        /// Tests that Index does not call IsCreatedHouseholdMember on the provider which can get values from claims when the controller does not have a user.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsCreatedHouseholdMemberOnClaimValueProviderWhenControllerDoesNotHaveUser()
        {
            var homeController = CreateHomeController(false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsCreatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index does not call IsActivatedHouseholdMember on the provider which can get values from claims when the controller does not have a user.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsActivatedHouseholdMemberOnClaimValueProviderWhenControllerDoesNotHaveUser()
        {
            var homeController = CreateHomeController(false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index does not call IsPrivacyPoliciesAccepted on the provider which can get values from claims when the controller does not have a user.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsPrivacyPoliciesAcceptedOnClaimValueProviderWhenControllerDoesNotHaveUser()
        {
            var homeController = CreateHomeController(false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Anything));
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
        /// Tests that Index does not call IsAuthenticated on the provider which can get values from claims when the controller does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsAuthenticatedOnClaimValueProviderWhenControllerDoesHaveUserWithoutIdentity()
        {
            var homeController = CreateHomeController(true, false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsAuthenticated(Arg<IIdentity>.Is.Anything));
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
        /// Tests that Index does not call IsCreatedHouseholdMember on the provider which can get values from claims when the controller does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsCreatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithoutIdentity()
        {
            var homeController = CreateHomeController(true, false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsCreatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index does not call IsActivatedHouseholdMember on the provider which can get values from claims when the controller does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsActivatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithoutIdentity()
        {
            var homeController = CreateHomeController(true, false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index does not call IsPrivacyPoliciesAccepted on the provider which can get values from claims when the controller does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsPrivacyPoliciesAcceptedOnClaimValueProviderWhenControllerDoesHaveUserWithoutIdentity()
        {
            var homeController = CreateHomeController(true, false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Anything));
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
        /// Tests that Index calls IsAuthenticated on the provider which can get values from claims when the controller does have a user with an identity.
        /// </summary>
        [Test]
        public void TestThatIndexCallsIsAuthenticatedOnClaimValueProviderWhenControllerDoesHaveUserWithIdentity()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);

            homeController.Index();

            _claimValueProviderMock.AssertWasCalled(m => m.IsAuthenticated(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        }

        /// <summary>
        /// Tests that Index does not call IsValidatedHouseholdMember on the provider which can get values from claims when the controller does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsValidatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithUnauthenticatedIdentity()
        {
            var homeController = CreateHomeController(isAuthenticated: false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.False);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index does not call IsCreatedHouseholdMember on the provider which can get values from claims when the controller does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsCreatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithUnauthenticatedIdentity()
        {
            var homeController = CreateHomeController(isAuthenticated: false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.False);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsCreatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index does not call IsActivatedHouseholdMember on the provider which can get values from claims when the controller does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsActivatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithUnauthenticatedIdentity()
        {
            var homeController = CreateHomeController(isAuthenticated: false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.False);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index does not call IsPrivacyPoliciesAccepted on the provider which can get values from claims when the controller does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexDoesNotCallIsPrivacyPoliciesAcceptedOnClaimValueProviderWhenControllerDoesHaveUserWithUnauthenticatedIdentity()
        {
            var homeController = CreateHomeController(isAuthenticated: false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.False);

            homeController.Index();

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that Index returns a ViewResult with the welcome message when the controller does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexReturnsViewResultWithWelcomeMessageWhenControllerDoesHaveUserWithUnauthenticatedIdentity()
        {
            var homeController = CreateHomeController(isAuthenticated: false);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.False);

            var result = homeController.Index();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult)result;
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
        /// Tests that Index calls IsValidatedHouseholdMember on the provider which can get values from claims when the controller does have a user with an authenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexCallsIsValidatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithAuthenticatedIdentity()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            homeController.Index();

            _claimValueProviderMock.AssertWasCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        }

        /// <summary>
        /// Tests that Index calls IsCreatedHouseholdMember on the provider which can get values from claims when the controller does have a user with an authenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexCallsIsCreatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithAuthenticatedIdentity()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            homeController.Index();

            _claimValueProviderMock.AssertWasCalled(m => m.IsCreatedHouseholdMember(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        }

        /// <summary>
        /// Tests that Index calls IsActivatedHouseholdMember on the provider which can get values from claims when the controller does have a user with an authenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexCallsIsActivatedHouseholdMemberOnClaimValueProviderWhenControllerDoesHaveUserWithAuthenticatedIdentity()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            homeController.Index();

            _claimValueProviderMock.AssertWasCalled(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        }

        /// <summary>
        /// Tests that Index calls IsPrivacyPoliciesAccepted on the provider which can get values from claims when the controller does have a user with an authenticated identity.
        /// </summary>
        [Test]
        public void TestThatIndexCallsIsPrivacyPoliciesAcceptedOnClaimValueProviderWhenControllerDoesHaveUserWithAuthenticatedIdentity()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            homeController.Index();

            _claimValueProviderMock.AssertWasCalled(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Equal(homeController.User.Identity)));
        }

        /// <summary>
        /// Tests that Index throws NotImplementedException when the controller does have a user with an authenticated identity who are a validated household member.
        /// </summary>
        [Test]
        public void TestThatIndexThrowsNotImplementedExceptionWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreValidatedHouseholdMember()
        {
            var homeController = CreateHomeController(isValidatedHouseholdMember: true);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            Assert.Throws<NotImplementedException>(() => homeController.Index());
        }

        /// <summary>
        /// Tests that Index throws NotImplementedException when the controller does have a user with an authenticated identity who are not created as a household member.
        /// </summary>
        [Test]
        public void TestThatIndexThrowsNotImplementedExceptionWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreNotCreatedHouseholdMember()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            Assert.Throws<NotImplementedException>(() => homeController.Index());
        }

        /// <summary>
        /// Tests that Index throws NotImplementedException when the controller does have a user with an authenticated identity who are created as a household member but not activated.
        /// </summary>
        [Test]
        public void TestThatIndexThrowsNotImplementedExceptionWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreCreatedHouseholdMemberButNotActivated()
        {
            var homeController = CreateHomeController(isCreatedHouseholdMember: true, hasAcceptedPrivacyPolicies: true);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            Assert.Throws<NotImplementedException>(() => homeController.Index());
        }

        /// <summary>
        /// Tests that Index throws NotImplementedException when the controller does have a user with an authenticated identity who are created as a household member but don't have accepted privacy policies.
        /// </summary>
        [Test]
        public void TestThatIndexThrowsNotImplementedExceptionWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreCreatedHouseholdMemberButNotAcceptedPrivacyPolicies()
        {
            var homeController = CreateHomeController(isCreatedHouseholdMember: true, isActivatedHouseholdMember: true);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            Assert.Throws<NotImplementedException>(() => homeController.Index());
        }

        /// <summary>
        /// Creates a home controller for unit testing.
        /// </summary>
        /// <param name="hasUser">Sets whether the controller should have an user.</param>
        /// <param name="hasIdentity">Sets whether the controller should have an user with an identity.</param>
        /// <param name="isAuthenticated">Sets whether the user is authenticated.</param>
        /// <param name="isValidatedHouseholdMember">Sets whether the user is a validated household member.</param>
        /// <param name="isCreatedHouseholdMember">Set whether the user has been created as a household member.</param>
        /// <param name="isActivatedHouseholdMember">Sets whether the user is an activated household member.</param>
        /// <param name="hasAcceptedPrivacyPolicies">Sets whether the user has accepted the privacy policies.</param>
        /// <returns>Home controller for unit testing.</returns>
        private HomeController CreateHomeController(bool hasUser = true, bool hasIdentity = true, bool isAuthenticated = true, bool isValidatedHouseholdMember = false, bool isCreatedHouseholdMember = false, bool isActivatedHouseholdMember = false, bool hasAcceptedPrivacyPolicies = false)
        {
            _claimValueProviderMock.Stub(m => m.IsAuthenticated(Arg<IIdentity>.Is.Anything))
                .Return(isAuthenticated)
                .Repeat.Any();
            _claimValueProviderMock.Stub(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything))
                .Return(isValidatedHouseholdMember)
                .Repeat.Any();
            _claimValueProviderMock.Stub(m => m.IsCreatedHouseholdMember(Arg<IIdentity>.Is.Anything))
                .Return(isCreatedHouseholdMember)
                .Repeat.Any();
            _claimValueProviderMock.Stub(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Anything))
                .Return(isActivatedHouseholdMember)
                .Repeat.Any();
            _claimValueProviderMock.Stub(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Anything))
                .Return(hasAcceptedPrivacyPolicies)
                .Repeat.Any();

            var identityMock = MockRepository.GenerateMock<IIdentity>();
            identityMock.Stub(m => m.IsAuthenticated)
                .Return(isAuthenticated)
                .Repeat.Any();

            var userMock = MockRepository.GenerateMock<IPrincipal>();
            userMock.Stub(m => m.Identity)
                .Return(hasIdentity ? identityMock : null)
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
