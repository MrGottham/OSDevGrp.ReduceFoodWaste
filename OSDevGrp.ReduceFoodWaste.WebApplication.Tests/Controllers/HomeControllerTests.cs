using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Cookies;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;
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

        private IClaimValueProvider _claimValueProviderMock;
        private ICookieHelper _cookieHelperMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            _cookieHelperMock = MockRepository.GenerateMock<ICookieHelper>();
        }

        /// <summary>
        /// Tests that the constructor initialize the Home controller.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHomeController()
        {
            var homeController = new HomeController(_claimValueProviderMock, _cookieHelperMock);
            Assert.That(homeController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can get values from claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HomeController(null, _cookieHelperMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the helper functionality for cookies is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCookieHelperIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HomeController(_claimValueProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cookieHelper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the getter for TopImages returns top images.
        /// </summary>
        [Test]
        public void TestThatTopImagesGetterReturnsTopImages()
        {
            Assert.That(HomeController.TopImages, Is.Not.Null);
            Assert.That(HomeController.TopImages, Is.Not.Empty);

            var topImageCollection = HomeController.TopImages.ToList();
            Assert.That(topImageCollection, Is.Not.Null);
            Assert.That(topImageCollection, Is.Not.Empty);
            Assert.That(topImageCollection.Count, Is.EqualTo(5));
            for (var i = 0; i < topImageCollection.Count; i++)
            {
                Assert.That(topImageCollection.ElementAt(i), Is.Not.Null);
                Assert.That(topImageCollection.ElementAt(i), Is.Not.Empty);
                Assert.That(topImageCollection.ElementAt(i), Is.EqualTo(string.Format("~/Images/FoodWaste0{0}.png", i + 1)));
            }
        }

        /// <summary>
        /// Tests that the getter for TopImage returns a top image.
        /// </summary>
        [Test]
        public void TestThatTopImageGetterReturnsTopImage()
        {
            Assert.That(HomeController.TopImage, Is.Not.Null);
            Assert.That(HomeController.TopImage, Is.Not.Empty);
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
        /// Tests that Index returns a redirect to the dashboard when the controller does have a user with an authenticated identity who are a validated household member.
        /// </summary>
        [Test]
        public void TestThatIndexReturnsRedirectToDashboardWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreValidatedHouseholdMember()
        {
            var homeController = CreateHomeController(isValidatedHouseholdMember: true);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            var result = homeController.Index();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);

            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("action"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Dashboard"));
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("controller"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo("Dashboard"));
        }

        /// <summary>
        /// Tests that Index returns a redirect to the creation of a household member when the controller does have a user with an authenticated identity who are not created as a household member.
        /// </summary>
        [Test]
        public void TestThatIndexRedirectToCreateHouseholdMemberWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreNotCreatedHouseholdMember()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            var result = homeController.Index();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);

            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("action"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Create"));
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("controller"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo("HouseholdMember"));
        }

        /// <summary>
        /// Tests that Index returns a redirect to the preparation of a household member when the controller does have a user with an authenticated identity who are created as a household member but not activated.
        /// </summary>
        [Test]
        public void TestThatIndexReturnsRedirectToPrepareHouseholdMemberWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreCreatedHouseholdMemberButNotActivated()
        {
            var homeController = CreateHomeController(isCreatedHouseholdMember: true, hasAcceptedPrivacyPolicies: true);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            var result = homeController.Index();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);

            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("action"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Prepare"));
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("controller"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo("HouseholdMember"));
        }

        /// <summary>
        /// Tests that Index returns a redirect to the preparation of a household member when the controller does have a user with an authenticated identity who are created as a household member but don't have accepted privacy policies.
        /// </summary>
        [Test]
        public void TestThatIndexReturnsRedirectToPrepareHouseholdMemberWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreCreatedHouseholdMemberButNotAcceptedPrivacyPolicies()
        {
            var homeController = CreateHomeController(isCreatedHouseholdMember: true, isActivatedHouseholdMember: true);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            var result = homeController.Index();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);

            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("action"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Prepare"));
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("controller"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo("HouseholdMember"));
        }

        /// <summary>
        /// Tests that Index throws NotSupportedException when the controller does have a user with an authenticated identity who are a created and activated household member with accepted privacy policies but not validated.
        /// </summary>
        [Test]
        public void TestThatIndexThrowsNotSupportedExceptionWhenControllerDoesHaveUserWithAuthenticatedIdentityWhoAreCreatedAndActivatedHouseholdMemberWithAcceptedPrivacyPoliciesButNotValidated()
        {
            var homeController = CreateHomeController(isCreatedHouseholdMember: true, isActivatedHouseholdMember: true, hasAcceptedPrivacyPolicies: true);
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.User, Is.Not.Null);
            Assert.That(homeController.User.Identity, Is.Not.Null);
            Assert.That(homeController.User.Identity.IsAuthenticated, Is.True);

            Assert.Throws<NotSupportedException>(() => homeController.Index());
        }

        /// <summary>
        /// Tests that AllowCookies throws an ArgumentNullException when the return url is null, empty or whitespaces.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatAllowCookiesThrowsArgumentNullExceptionWhenReturnUrlIsNullEmptyOrWhitespaces(string invalidValue)
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => homeController.AllowCookies(invalidValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("returnUrl"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AllowCookies calls SetCookieConsent on the helper functionality for cookies.
        /// </summary>
        [Test]
        public void TestThatAllowCookiesCallsSetCookieConsentOnCookieHelper()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);
            Assert.That(homeController.HttpContext, Is.Not.Null);
            Assert.That(homeController.HttpContext.Response, Is.Not.Null);

            homeController.AllowCookies(Fixture.Create<string>());

            _cookieHelperMock.SetCookieConsent(Arg<HttpResponseBase>.Is.Equal(homeController.HttpContext.Response), Arg<bool>.Is.Equal(true));
        }

        /// <summary>
        /// Tests that AllowCookies returns an redirect result to the return url.
        /// </summary>
        [Test]
        public void TestThatAllowCookiesReturnsRedirectResultToReturnUrl()
        {
            var homeController = CreateHomeController();
            Assert.That(homeController, Is.Not.Null);

            var returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            var result = homeController.AllowCookies(returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectResult>());

            var redirectResult = (RedirectResult)result;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Empty);
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
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

            var homeController = new HomeController(_claimValueProviderMock, _cookieHelperMock);
            homeController.ControllerContext = ControllerTestHelper.CreateControllerContext(homeController, hasUser, hasIdentity, isAuthenticated);
            return homeController;
        }
    }
}
