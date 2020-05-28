using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers.HouseholdMemberController
{
    /// <summary>
    /// Tests the AddHousehold methods controller for a household member.
    /// </summary>
    public class AddHouseholdTests : TestBase
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;
        private IClaimValueProvider _claimValueProviderMock;
        private ILocalClaimProvider _localClaimProviderMock;
        private IModelHelper _modelHelperMock;
        private IUtilities _utilitiesMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            _claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            _localClaimProviderMock = MockRepository.GenerateMock<ILocalClaimProvider>();
            _modelHelperMock = MockRepository.GenerateMock<IModelHelper>();
            _utilitiesMock = MockRepository.GenerateMock<IUtilities>();
        }

        /// <summary>
        /// Tests that AddHousehold without a status message returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatAddHouseholdWithoutStatusMessageReturnsViewResultWithModelForAddingNewHousehold(string statusMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            ActionResult result = householdMemberController.AddHousehold(statusMessage: statusMessage);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result);
        }

        /// <summary>
        /// Tests that AddHousehold with a status message returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdWithStatusMessageReturnsViewResultWithModelForAddingNewHousehold()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            ActionResult result = householdMemberController.AddHousehold(statusMessage: statusMessage);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result, new Dictionary<string, string> {{"StatusMessage", statusMessage}});
        }

        /// <summary>
        /// Tests that AddHousehold without an error message returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatAddHouseholdWithoutErrorMessageReturnsViewResultWithModelForAddingNewHousehold(string errorMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            ActionResult result = householdMemberController.AddHousehold(errorMessage: errorMessage);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result);
        }

        /// <summary>
        /// Tests that AddHousehold with an error message returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdWithErrorMessageReturnsViewResultWithModelForAddingNewHousehold()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            ActionResult result = householdMemberController.AddHousehold(errorMessage: errorMessage);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result, new Dictionary<string, string> {{"ErrorMessage", errorMessage}});
        }

        /// <summary>
        /// Tests that AddHousehold with a model for adding a new household throws an ArgumentNullException when the model is null.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdWithModelForAddingNewHouseholdThrowsArgumentNullExceptionWhenHouseholdModelIsNull()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.AddHousehold(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddHousehold with an invalid model for adding a new household returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdWithInvalidModelForAddingNewHouseholdReturnsViewResultWithModelForAddingNewHousehold()
        {
            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.ModelState, Is.Not.Null);

            householdMemberController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdMemberController.ModelState.IsValid, Is.False);

            ActionResult result = householdMemberController.AddHousehold(householdModel);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result, expectedHouseholdModel: householdModel);
        }

        /// <summary>
        /// Tests that AddHousehold with an valid model for adding a new household calls CreateHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdWithValidModelForAddingNewHouseholdCallsCreateHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdMemberController.AddHousehold(householdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.CreateHouseholdAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<HouseholdModel>.Is.Equal(householdModel), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that AddHousehold with an valid model for adding a new household returns a RedirectToRouteResult for the dashboard.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdWithValidModelForAddingNewHouseholdReturnsRedirectToRouteResultForDashboard()
        {
            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ActionResult result = householdMemberController.AddHousehold(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.Count, Is.EqualTo(2));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.EqualTo("action"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.EqualTo("Dashboard"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.EqualTo("controller"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.EqualTo("Dashboard"));
        }

        private WebApplication.Controllers.HouseholdMemberController CreateHouseholdMemberController()
        {
            _householdDataRepositoryMock.Stub(m => m.CreateHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(() => Fixture.Build<HouseholdModel>().With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null).Create()))
                .Repeat.Any();

            WebApplication.Controllers.HouseholdMemberController householdMemberController = new WebApplication.Controllers.HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock, _modelHelperMock, _utilitiesMock);
            householdMemberController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdMemberController);
            return householdMemberController;
        }

        private static void AssertThatActionResultIsViewResultForAddingNewHousehold(ActionResult actionResult, IDictionary<string, string> viewData = null, HouseholdModel expectedHouseholdModel = null)
        {
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) actionResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("AddHousehold"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            if (viewData == null)
            {
                Assert.That(viewResult.ViewData, Is.Not.Null);
                Assert.That(viewResult.ViewData, Is.Empty);
            }
            else
            {
                Assert.That(viewResult.ViewData, Is.Not.Null);
                Assert.That(viewResult.ViewData, Is.Not.Empty);
                foreach (KeyValuePair<string, string> viewDataElement in viewData)
                {
                    Assert.That(viewResult.ViewData[viewDataElement.Key], Is.Not.Null);
                    Assert.That(viewResult.ViewData[viewDataElement.Key], Is.Not.Empty);
                    Assert.That(viewResult.ViewData[viewDataElement.Key], Is.EqualTo(viewDataElement.Value));
                }
            }

            HouseholdModel householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            if (expectedHouseholdModel != null)
            {
                Assert.That(householdModel, Is.EqualTo(expectedHouseholdModel));
                return;
            }
            Assert.That(householdModel.Name, Is.Null);
            Assert.That(householdModel.Description, Is.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Null);
        }
    }
}