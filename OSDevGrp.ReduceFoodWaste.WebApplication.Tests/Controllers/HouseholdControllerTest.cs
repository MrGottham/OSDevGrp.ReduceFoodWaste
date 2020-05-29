using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Tests the controller for a household.
    /// </summary>
    [TestFixture]
    public class HouseholdControllerTest : TestBase
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
        }

        /// <summary>
        /// Tests that the constructor initialize the controller for a household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdController()
        {
            HouseholdController householdController = new HouseholdController(_householdDataRepositoryMock);
            Assert.That(householdController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new HouseholdController(null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Manage with an identification for a given household which are equal to null and without a status message returns a RedirectToRouteResult for the dashboard.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatManageWithHouseholdIdentifierEqualToNullAndWithoutStatusMessageReturnsRedirectToRouteResultForDashboard(string statusMessage)
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            // ReSharper disable ExpressionIsAlwaysNull
            ActionResult result = householdController.Manage(householdIdentifier: householdIdentifier, statusMessage: statusMessage);
            // ReSharper restore ExpressionIsAlwaysNull
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

        /// <summary>
        /// Tests that Manage with an identification for a given household which are equal to null and with a status message returns a RedirectToRouteResult for the dashboard.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierEqualToNullAndWithStatusMessageReturnsRedirectToRouteResultForDashboard()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            // ReSharper disable ExpressionIsAlwaysNull
            ActionResult result = householdController.Manage(householdIdentifier: householdIdentifier, statusMessage: statusMessage);
            // ReSharper restore ExpressionIsAlwaysNull
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

        /// <summary>
        /// Tests that Manage with an identification for a given household which are equal to null and without an error message returns a RedirectToRouteResult for the dashboard.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatManageWithHouseholdIdentifierEqualToNullAndWithoutErrorMessageReturnsRedirectToRouteResultForDashboard(string errorMessage)
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            // ReSharper disable ExpressionIsAlwaysNull
            ActionResult result = householdController.Manage(householdIdentifier: householdIdentifier, errorMessage: errorMessage);
            // ReSharper restore ExpressionIsAlwaysNull
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

        /// <summary>
        /// Tests that Manage with an identification for a given household which are equal to null and with an error message returns a RedirectToRouteResult for the dashboard.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierEqualToNullAndWithErrorMessageReturnsRedirectToRouteResultForDashboard()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            // ReSharper disable ExpressionIsAlwaysNull
            ActionResult result = householdController.Manage(householdIdentifier: householdIdentifier, errorMessage: errorMessage);
            // ReSharper restore ExpressionIsAlwaysNull
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

        /// <summary>
        /// Tests that Manage with an identification for a given household which are not equal to null and without a status message returns a ViewResult with a model for manage the given household.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatManageWithHouseholdIdentifierNotEqualToNullAndWithoutStatusMessageReturnsViewResultWithModelForManageHousehold(string statusMessage)
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            ActionResult result = householdController.Manage(householdIdentifier: householdIdentifier, statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            HouseholdModel householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
        }

        /// <summary>
        /// Tests that Manage with an identification for a given household which are not equal to null and with a status message returns a ViewResult with a model for manage the given household.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierNotEqualToNullAndWithStatusMessageReturnsViewResultWithModelForManageHousehold()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            ActionResult result = householdController.Manage(householdIdentifier: householdIdentifier, statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.Not.Null);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.EqualTo(statusMessage));
            Assert.That(viewResult.ViewData["EditMode"], Is.Null);
            Assert.That(viewResult.ViewData["AddingHouseholdMemberMode"], Is.Null);

            HouseholdModel householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
        }

        /// <summary>
        /// Tests that Manage with an identification for a given household which are not equal to null and without an error message returns a ViewResult with a model for manage the given household.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatManageWithHouseholdIdentifierNotEqualToNullAndWithoutErrorMessageReturnsViewResultWithModelForManageHousehold(string errorMessage)
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            ActionResult result = householdController.Manage(householdIdentifier: householdIdentifier, errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            HouseholdModel householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
        }

        /// <summary>
        /// Tests that Manage with an identification for a given household which are not equal to null and with an error message returns a ViewResult with a model for manage the given household.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierNotEqualToNullAndWithErrorMessageReturnsViewResultWithModelForManageHousehold()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            ActionResult result = householdController.Manage(householdIdentifier: householdIdentifier, errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.EqualTo(errorMessage));
            Assert.That(viewResult.ViewData["EditMode"], Is.Null);
            Assert.That(viewResult.ViewData["AddingHouseholdMemberMode"], Is.Null);

            HouseholdModel householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
        }

        /// <summary>
        /// Tests that HouseholdInformation with an identification for a given household which are equal to null does not call GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatHouseholdInformationWithHouseholdIdentifierEqualToNullDoesNotCallGetHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            // ReSharper disable ExpressionIsAlwaysNull
            householdController.HouseholdInformation(householdIdentifier);
            // ReSharper restore ExpressionIsAlwaysNull

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that HouseholdInformation with an identification for a given household which are equal to null returns a PartialViewResult without a model.
        /// </summary>
        [Test]
        public void TestThatHouseholdInformationWithHouseholdIdentifierEqualToNullReturnsPartialViewResultWithoutModel()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            // ReSharper disable ExpressionIsAlwaysNull
            ActionResult result = householdController.HouseholdInformation(householdIdentifier);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            PartialViewResult partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("_Empty"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Empty);
            Assert.That(partialViewResult.Model, Is.Null);
        }

        /// <summary>
        /// Tests that HouseholdInformation with an identification for a given household which are not equal to null calls GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatHouseholdInformationWithHouseholdIdentifierNotEqualToNullCallsGetHouseholdAsyncOnHouseholdDataRepository()
        {
            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            HouseholdController householdController = CreateHouseholdController(householdGetterCallback: arguments =>
            {
                Assert.That(arguments, Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.TypeOf<HouseholdModel>());

                HouseholdModel householdModel = (HouseholdModel) arguments.ElementAt(1);
                Assert.That(householdModel, Is.Not.Null);
                Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
            });
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdController.HouseholdInformation(householdIdentifier);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Equal(householdController.User.Identity), Arg<HouseholdModel>.Is.NotNull, Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that HouseholdInformation with an identification for a given household which are not equal to null returns a PartialViewResult with a model for the given household.
        /// </summary>
        [Test]
        public void TestThatHouseholdInformationWithHouseholdIdentifierNotEqualToNullReturnsPartialViewResultWithHouseholdModel()
        {
            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            HouseholdController householdController = CreateHouseholdController(household: householdModel);
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            ActionResult result = householdController.HouseholdInformation(householdIdentifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            PartialViewResult partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("_HouseholdInformation"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Not.Empty);
            Assert.That(partialViewResult.ViewData["EditMode"], Is.Not.Null);
            Assert.That(partialViewResult.ViewData["EditMode"], Is.False);
            Assert.That(partialViewResult.Model, Is.Not.Null);
            Assert.That(partialViewResult.Model, Is.EqualTo(householdModel));
        }

        /// <summary>
        /// Tests that Edit with an identification for a given household which are equal to null does not call GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatEditWithHouseholdIdentifierEqualToNullDoesNotCallGetHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            // ReSharper disable ExpressionIsAlwaysNull
            householdController.Edit(householdIdentifier);
            // ReSharper restore ExpressionIsAlwaysNull

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that Edit with an identification for a given household which are equal to null returns a PartialViewResult without a model.
        /// </summary>
        [Test]
        public void TestThatEditWithHouseholdIdentifierEqualToNullReturnsPartialViewResultWithoutModel()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            // ReSharper disable ExpressionIsAlwaysNull
            ActionResult result = householdController.Edit(householdIdentifier);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            PartialViewResult partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("_Empty"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Empty);
            Assert.That(partialViewResult.Model, Is.Null);
        }

        /// <summary>
        /// Tests that Edit with an identification for a given household which are not equal to null calls GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatEditWithHouseholdIdentifierNotEqualToNullCallsGetHouseholdAsyncOnHouseholdDataRepository()
        {
            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            HouseholdController householdController = CreateHouseholdController(householdGetterCallback: arguments =>
            {
                Assert.That(arguments, Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.TypeOf<HouseholdModel>());

                HouseholdModel householdModel = (HouseholdModel) arguments.ElementAt(1);
                Assert.That(householdModel, Is.Not.Null);
                Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
            });
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdController.Edit(householdIdentifier);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Equal(householdController.User.Identity), Arg<HouseholdModel>.Is.NotNull, Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Edit with an identification for a given household which are not equal to null returns a PartialViewResult with a model for the given household.
        /// </summary>
        [Test]
        public void TestThatEditWithHouseholdIdentifierNotEqualToNullReturnsPartialViewResultWithHouseholdModel()
        {
            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            HouseholdController householdController = CreateHouseholdController(household: householdModel);
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            ActionResult result = householdController.Edit(householdIdentifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            PartialViewResult partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("Edit"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Not.Empty);
            Assert.That(partialViewResult.ViewData["EditMode"], Is.Not.Null);
            Assert.That(partialViewResult.ViewData["EditMode"], Is.True);
            Assert.That(partialViewResult.Model, Is.Not.Null);
            Assert.That(partialViewResult.Model, Is.EqualTo(householdModel));
        }

        /// <summary>
        /// Tests that Edit with a household model for updating throws an ArgumentNullException when the household model for updating is null.
        /// </summary>
        [Test]
        public void TestThatEditWithHouseholdModelThrowsArgumentNullExceptionWhenHouseholdModelIsNull()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            const HouseholdModel householdModel = null;

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdController.Edit(householdModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Edit with an invalid household model for updating calls GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatEditWithInvalidHouseholdModelCallsGetHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdModel invalidHouseholdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();

            HouseholdController householdController = CreateHouseholdController(householdGetterCallback: arguments =>
            {
                Assert.That(arguments, Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.TypeOf<HouseholdModel>());

                HouseholdModel householdModel = (HouseholdModel) arguments.ElementAt(1);
                Assert.That(householdModel, Is.Not.Null);
                Assert.That(householdModel.Identifier, Is.EqualTo(invalidHouseholdModel.Identifier));
            });
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdController.ModelState.IsValid, Is.False);

            householdController.Edit(invalidHouseholdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Equal(householdController.User.Identity), Arg<HouseholdModel>.Is.NotNull, Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Edit with an invalid household model for updating does not call UpdateHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatEditWithInvalidHouseholdModelDoesNotCallUpdateHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            householdController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdController.ModelState.IsValid, Is.False);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();

            householdController.Edit(householdModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.UpdateHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that Edit with an invalid household model for updating returns a ViewResult with a model for the given household.
        /// </summary>
        [Test]
        public void TestThatEditWithInvalidHouseholdModelReturnsViewResultWithHouseholdModel()
        {
            List<MemberOfHouseholdModel> householdMemberModelCollection = new List<MemberOfHouseholdModel>(Random.Next(5, 10));
            while (householdMemberModelCollection.Count < householdMemberModelCollection.Capacity)
            {
                MemberOfHouseholdModel memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                    .With(m => m.HouseholdMemberIdentifier, Guid.NewGuid())
                    .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                    .With(m => m.MailAddress, Fixture.Create<string>())
                    .With(m => m.Removable, Fixture.Create<bool>())
                    .Create();
                householdMemberModelCollection.Add(memberOfHouseholdModel);
            }
            HouseholdModel reloadedHouseholdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, householdMemberModelCollection)
                .Create();
            Assert.That(reloadedHouseholdModel, Is.Not.Null);
            Assert.That(reloadedHouseholdModel.HouseholdMembers, Is.Not.Null);
            Assert.That(reloadedHouseholdModel.HouseholdMembers, Is.Not.Empty);

            HouseholdController householdController = CreateHouseholdController(household: reloadedHouseholdModel);
            Assert.That(householdController, Is.Not.Null);

            householdController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdController.ModelState.IsValid, Is.False);

            HouseholdModel invalidHouseholdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(invalidHouseholdModel, Is.Not.Null);
            Assert.That(invalidHouseholdModel.HouseholdMembers, Is.Null);

            ActionResult result = householdController.Edit(invalidHouseholdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["EditMode"], Is.Not.Null);
            Assert.That(viewResult.ViewData["EditMode"], Is.True);
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(invalidHouseholdModel));

            HouseholdModel updatedInvalidHouseholdModel = (HouseholdModel) viewResult.Model;
            Assert.That(updatedInvalidHouseholdModel, Is.Not.Null);
            Assert.That(updatedInvalidHouseholdModel.HouseholdMembers, Is.Not.Null);
            Assert.That(updatedInvalidHouseholdModel.HouseholdMembers, Is.Not.Empty);
            Assert.That(updatedInvalidHouseholdModel.HouseholdMembers, Is.EqualTo(householdMemberModelCollection));
        }

        /// <summary>
        /// Tests that Edit with a valid household model for updating does not call GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatEditWithValidHouseholdModelDoesNotCallGetHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();

            householdController.Edit(householdModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that Edit with a valid household model for updating calls UpdateHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatEditWithValidHouseholdModelCallsUpdateHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();

            householdController.Edit(householdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.UpdateHouseholdAsync(Arg<IIdentity>.Is.Equal(householdController.User.Identity), Arg<HouseholdModel>.Is.Equal(householdModel)));
        }

        /// <summary>
        /// Tests that Edit with a valid household model for updating returns a RedirectToRouteResult to manage the updated household.
        /// </summary>
        [Test]
        public void TestThatEditWithValidHouseholdModelReturnsRedirectToRouteResultToManage()
        {
            Guid updatedHouseholdIdentifier = Guid.NewGuid();
            HouseholdModel updatedHouseholdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.Identifier, updatedHouseholdIdentifier)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(updatedHouseholdModel, Is.Not.Null);
            Assert.That(updatedHouseholdModel.Identifier, Is.Not.EqualTo(default(Guid)));

            HouseholdController householdController = CreateHouseholdController(updatedHousehold: updatedHouseholdModel);
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();

            ActionResult result = householdController.Edit(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.Count, Is.EqualTo(3));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.EqualTo("householdIdentifier"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.EqualTo(updatedHouseholdIdentifier));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.EqualTo("action"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.EqualTo("Manage"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.EqualTo("controller"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.EqualTo("Household"));
        }

        /// <summary>
        /// Tests that AddHouseholdMember with an identification for a given household which are equal to null returns a PartialViewResult without a model.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithHouseholdIdentifierEqualToNullReturnsPartialViewResultWithoutModel()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            // ReSharper disable ExpressionIsAlwaysNull
            ActionResult result = householdController.AddHouseholdMember(householdIdentifier);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            PartialViewResult partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("_Empty"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Empty);
            Assert.That(partialViewResult.Model, Is.Null);
        }

        /// <summary>
        /// Tests that AddHouseholdMember with an identification for a given household which are not equal to null returns a PartialViewResult with a model for adding a household member.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithHouseholdIdentifierNotEqualToNullReturnsPartialViewResultWithMemberOfHouseholdModel()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            ActionResult result = householdController.AddHouseholdMember(householdIdentifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            PartialViewResult partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("_AddHouseholdMember"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Empty);
            Assert.That(partialViewResult.Model, Is.Not.Null);
            Assert.That(partialViewResult.Model, Is.TypeOf<MemberOfHouseholdModel>());

            MemberOfHouseholdModel memberOfHouseholdModel = (MemberOfHouseholdModel) partialViewResult.Model;
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.HouseholdMemberIdentifier, Is.EqualTo(default(Guid)));
            Assert.That(memberOfHouseholdModel.HouseholdIdentifier, Is.EqualTo(householdIdentifier));
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Null);
            Assert.That(memberOfHouseholdModel.Removable, Is.False);
        }

        /// <summary>
        /// Tests that AddHouseholdMember with a household member model for adding a household member throws an ArgumentNullException when the household member model for adding a household member is null.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithMemberOfHouseholdModelThrowsArgumentNullExceptionWhenMemberOfHouseholdModelIsNull()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            const MemberOfHouseholdModel memberOfHouseholdModel = null;

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdController.AddHouseholdMember(memberOfHouseholdModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("memberOfHouseholdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddHouseholdMember with an invalid household member model for adding a household member calls GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithInvalidMemberOfHouseholdModelCallsGetHouseholdAsyncOnHouseholdDataRepository()
        {
            MemberOfHouseholdModel invalidMemberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, (string) null)
                .Create();

            HouseholdController householdController = CreateHouseholdController(householdGetterCallback: arguments =>
            {
                Assert.That(arguments, Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.TypeOf<HouseholdModel>());

                HouseholdModel householdModel = (HouseholdModel) arguments.ElementAt(1);
                Assert.That(householdModel, Is.Not.Null);
                Assert.That(householdModel.Identifier, Is.EqualTo(invalidMemberOfHouseholdModel.HouseholdIdentifier));
            });
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdController.ModelState.IsValid, Is.False);

            householdController.AddHouseholdMember(invalidMemberOfHouseholdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Equal(householdController.User.Identity), Arg<HouseholdModel>.Is.NotNull, Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that AddHouseholdMember with an invalid household member model for adding a household member does not call AddHouseholdMemberToHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithInvalidMemberOfHouseholdModelDoesNotCallAddHouseholdMemberToHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            MemberOfHouseholdModel invalidMemberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, (string) null)
                .Create();

            householdController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdController.ModelState.IsValid, Is.False);

            householdController.AddHouseholdMember(invalidMemberOfHouseholdModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.AddHouseholdMemberToHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<MemberOfHouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that AddHouseholdMember with an invalid household member model for adding a household member returns a ViewResult with a model for the given household.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithInvalidMemberOfHouseholdModelViewResultWithHouseholdModel()
        {
            HouseholdModel reloadedHouseholdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();

            HouseholdController householdController = CreateHouseholdController(household: reloadedHouseholdModel);
            Assert.That(householdController, Is.Not.Null);

            householdController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdController.ModelState.IsValid, Is.False);

            MemberOfHouseholdModel invalidMemberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, (string) null)
                .Create();

            ActionResult result = householdController.AddHouseholdMember(invalidMemberOfHouseholdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["AddingHouseholdMemberMode"], Is.Not.Null);
            Assert.That(viewResult.ViewData["AddingHouseholdMemberMode"], Is.True);
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(reloadedHouseholdModel));

            HouseholdModel householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.HouseholdMembers, Is.Not.Null);
            Assert.That(householdModel.HouseholdMembers, Is.Not.Empty);
            Assert.That(householdModel.HouseholdMembers.Last(), Is.EqualTo(invalidMemberOfHouseholdModel));
        }

        /// <summary>
        /// Tests that AddHouseholdMember with a valid household member model for adding a household member does not call GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithValidMemberOfHouseholdModelDoesNotCallGetHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            MemberOfHouseholdModel validMemberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, (string) null)
                .Create();

            householdController.AddHouseholdMember(validMemberOfHouseholdModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that AddHouseholdMember with a valid household member model for adding a household member calls AddHouseholdMemberToHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithValidMemberOfHouseholdModelCallsAddHouseholdMemberToHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            MemberOfHouseholdModel validMemberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, (string) null)
                .Create();

            householdController.AddHouseholdMember(validMemberOfHouseholdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.AddHouseholdMemberToHouseholdAsync(Arg<IIdentity>.Is.Equal(householdController.User.Identity), Arg<MemberOfHouseholdModel>.Is.Equal(validMemberOfHouseholdModel), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that AddHouseholdMember with a valid household member model for adding a household member returns a RedirectToRouteResult to manage the household.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberWithValidMemberOfHouseholdModelReturnsRedirectToRouteResultToManage()
        {
            MemberOfHouseholdModel validMemberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, (string) null)
                .Create();

            HouseholdController householdController = CreateHouseholdController(addedMemberOfHousehold: validMemberOfHouseholdModel);
            Assert.That(householdController, Is.Not.Null);

            ActionResult result = householdController.AddHouseholdMember(validMemberOfHouseholdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.Count, Is.EqualTo(3));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.EqualTo("householdIdentifier"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.EqualTo(validMemberOfHouseholdModel.HouseholdIdentifier));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.EqualTo("action"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.EqualTo("Manage"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.EqualTo("controller"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.EqualTo("Household"));
        }

        /// <summary>
        /// Tests that RemoveHouseholdMember with a household member model for removing a household member throws an ArgumentNullException when the household member model for removing a household member is null.
        /// </summary>
        [Test]
        public void TestThatRemoveHouseholdMemberWithMemberOfHouseholdModelThrowsArgumentNullExceptionWhenMemberOfHouseholdModelIsNull()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            const MemberOfHouseholdModel memberOfHouseholdModel = null;

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdController.RemoveHouseholdMember(memberOfHouseholdModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("memberOfHouseholdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RemoveHouseholdMember with a valid household member model for removing a household member calls RemoveHouseholdMemberFromHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatRemoveHouseholdMemberWithValidMemberOfHouseholdModelCallsRemoveHouseholdMemberFromHouseholdAsyncOnHouseholdDataRepository()
        {
            HouseholdController householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            MemberOfHouseholdModel validMemberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, (string) null)
                .Create();

            householdController.RemoveHouseholdMember(validMemberOfHouseholdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.RemoveHouseholdMemberFromHouseholdAsync(Arg<IIdentity>.Is.Equal(householdController.User.Identity), Arg<MemberOfHouseholdModel>.Is.Equal(validMemberOfHouseholdModel)));
        }

        /// <summary>
        /// Tests that RemoveHouseholdMember with a valid household member model for removing a household member returns a RedirectToRouteResult to manage the household.
        /// </summary>
        [Test]
        public void TestThatRemoveHouseholdMemberWithValidMemberOfHouseholdModelReturnsRedirectToRouteResultToManage()
        {
            MemberOfHouseholdModel validMemberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, (string) null)
                .Create();

            HouseholdController householdController = CreateHouseholdController(removedMemberOfHousehold: validMemberOfHouseholdModel);
            Assert.That(householdController, Is.Not.Null);

            ActionResult result = householdController.RemoveHouseholdMember(validMemberOfHouseholdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.Count, Is.EqualTo(3));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.EqualTo("householdIdentifier"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.EqualTo(validMemberOfHouseholdModel.HouseholdIdentifier));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.EqualTo("action"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.EqualTo("Manage"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Key, Is.EqualTo("controller"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(2).Value, Is.EqualTo("Household"));
        }

        /// <summary>
        /// Creates a controller for a household for unit testing.
        /// </summary>
        /// <param name="household">Sets the model for the household which should be used.</param>
        /// <param name="householdGetterCallback">Sets a callback action which are executed when we get the household to use.</param>
        /// <param name="updatedHousehold">Sets the model for the updated household.</param>
        /// <param name="addedMemberOfHousehold">Sets the model for the added household member.</param>
        /// <param name="removedMemberOfHousehold">Sets the model for the removed household member.</param>
        /// <returns>Controller for a household for unit testing.</returns>
        private HouseholdController CreateHouseholdController(HouseholdModel household = null, Action<object[]> householdGetterCallback = null, HouseholdModel updatedHousehold = null, MemberOfHouseholdModel addedMemberOfHousehold = null, MemberOfHouseholdModel removedMemberOfHousehold = null)
        {
            _householdDataRepositoryMock.Stub(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e =>
                {
                    if (householdGetterCallback == null)
                    {
                        return;
                    }
                    try
                    {
                        householdGetterCallback.Invoke(e.Arguments);
                    }
                    catch (TargetInvocationException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            throw ex.InnerException;
                        }
                        throw;
                    }
                })
                .Return(Task.FromResult(household ?? Fixture.Build<HouseholdModel>()
                    .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                    .Create()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.UpdateHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything))
                .Return(Task.FromResult(updatedHousehold ?? Fixture.Build<HouseholdModel>()
                    .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                    .Create()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.AddHouseholdMemberToHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<MemberOfHouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(() => addedMemberOfHousehold ?? Fixture.Build<MemberOfHouseholdModel>().With(m => m.HouseholdIdentifier, Guid.NewGuid()).With(m => m.MailAddress, Fixture.Create<string>()).Create()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.RemoveHouseholdMemberFromHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<MemberOfHouseholdModel>.Is.Anything))
                .Return(Task.Run(() => removedMemberOfHousehold ?? Fixture.Build<MemberOfHouseholdModel>().With(m => m.HouseholdIdentifier, Guid.NewGuid()).With(m => m.MailAddress, Fixture.Create<string>()).Create()))
                .Repeat.Any();

            HouseholdController householdController = new HouseholdController(_householdDataRepositoryMock);
            householdController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdController);
            return householdController;
        }
    }
}