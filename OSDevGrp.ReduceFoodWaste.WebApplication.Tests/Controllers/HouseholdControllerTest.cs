using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using Ploeh.AutoFixture;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

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
            var householdController = new HouseholdController(_householdDataRepositoryMock);
            Assert.That(householdController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdController(null));
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
            var householdController = CreateHouseholdController();
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
            var result = householdController.Manage(householdIdentifier: householdIdentifier, statusMessage: statusMessage);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            // ReSharper disable ExpressionIsAlwaysNull
            var result = householdController.Manage(householdIdentifier: householdIdentifier, statusMessage: statusMessage);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
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
            var householdController = CreateHouseholdController();
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
            var result = householdController.Manage(householdIdentifier: householdIdentifier, errorMessage: errorMessage);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            // ReSharper disable ExpressionIsAlwaysNull
            var result = householdController.Manage(householdIdentifier: householdIdentifier, errorMessage: errorMessage);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            var result = householdController.Manage(householdIdentifier: householdIdentifier, statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            var householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
        }

        /// <summary>
        /// Tests that Manage with an identification for a given household which are not equal to null and with a status message returns a ViewResult with a model for manage the given household.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierNotEqualToNullAndWithStatusMessageReturnsViewResultWithModelForManageHousehold()
        {
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            var result = householdController.Manage(householdIdentifier: householdIdentifier, statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
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

            var householdModel = (HouseholdModel) viewResult.Model;
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            var result = householdController.Manage(householdIdentifier: householdIdentifier, errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            var householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
        }

        /// <summary>
        /// Tests that Manage with an identification for a given household which are not equal to null and with an error message returns a ViewResult with a model for manage the given household.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierNotEqualToNullAndWithErrorMessageReturnsViewResultWithModelForManageHousehold()
        {
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            var result = householdController.Manage(householdIdentifier: householdIdentifier, errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
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

            var householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
        }

        /// <summary>
        /// Tests that HouseholdInformation with an identification for a given household which are equal to null does not call GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatHouseholdInformationWithHouseholdIdentifierEqualToNullDoesNotCallGetHouseholdAsyncOnHouseholdDataRepository()
        {
            var householdController = CreateHouseholdController();
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            // ReSharper disable ExpressionIsAlwaysNull
            var result = householdController.HouseholdInformation(householdIdentifier);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            var partialViewResult = (PartialViewResult) result;
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

            Action<object[]> householdGetterCallback = arguments =>
            {
                Assert.That(arguments, Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.TypeOf<HouseholdModel>());

                var householdModel = (HouseholdModel) arguments.ElementAt(1);
                Assert.That(householdModel, Is.Not.Null);
                Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
            };

            var householdController = CreateHouseholdController(householdGetterCallback: householdGetterCallback);
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
            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            var householdController = CreateHouseholdController(household: householdModel);
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var result = householdController.HouseholdInformation(householdIdentifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            var partialViewResult = (PartialViewResult) result;
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
            var householdController = CreateHouseholdController();
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            // ReSharper disable ExpressionIsAlwaysNull
            var result = householdController.Edit(householdIdentifier);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            var partialViewResult = (PartialViewResult) result;
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

            Action<object[]> householdGetterCallback = arguments =>
            {
                Assert.That(arguments, Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.TypeOf<HouseholdModel>());

                var householdModel = (HouseholdModel) arguments.ElementAt(1);
                Assert.That(householdModel, Is.Not.Null);
                Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier.Value));
            };

            var householdController = CreateHouseholdController(householdGetterCallback: householdGetterCallback);
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
            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            var householdController = CreateHouseholdController(household: householdModel);
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var result = householdController.Edit(householdIdentifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            var partialViewResult = (PartialViewResult) result;
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            const HouseholdModel householdModel = null;

            var exception = Assert.Throws<ArgumentNullException>(() => householdController.Edit(householdModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Edit with an invalid household model for updating does not call UpdateHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatEditWithInvalidHouseholdModelDoesNotCallUpdateHouseholdAsyncOnHouseholdDataRepository()
        {
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            householdController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdController.ModelState.IsValid, Is.False);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            householdController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdController.ModelState.IsValid, Is.False);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();

            var result = householdController.Edit(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["EditMode"], Is.Not.Null);
            Assert.That(viewResult.ViewData["EditMode"], Is.True);
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdModel));
        }

        /// <summary>
        /// Tests that Edit with a valid household model for updating calls UpdateHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatEditWithValidHouseholdModelCallsUpdateHouseholdAsyncOnHouseholdDataRepository()
        {
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
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
            var updatedHouseholdIdentifier = Guid.NewGuid();
            var updatedHouseholdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.Identifier, updatedHouseholdIdentifier)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(updatedHouseholdModel, Is.Not.Null);
            Assert.That(updatedHouseholdModel.Identifier, Is.Not.EqualTo(default(Guid)));

            var householdController = CreateHouseholdController(updatedHousehold: updatedHouseholdModel);
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();

            var result = householdController.Edit(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = null;
            // ReSharper disable ExpressionIsAlwaysNull
            Assert.That(householdIdentifier, Is.Null);
            // ReSharper restore ExpressionIsAlwaysNull
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.False);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            // ReSharper disable ExpressionIsAlwaysNull
            var result = householdController.AddHouseholdMember(householdIdentifier);
            // ReSharper restore ExpressionIsAlwaysNull
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            var partialViewResult = (PartialViewResult) result;
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
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Guid? householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse

            var result = householdController.AddHouseholdMember(householdIdentifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            var partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("_AddHouseholdMember"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Empty);
            Assert.That(partialViewResult.Model, Is.Not.Null);
            Assert.That(partialViewResult.Model, Is.TypeOf<MemberOfHouseholdModel>());

            var memberOfHouseholdModel = (MemberOfHouseholdModel) partialViewResult.Model;
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.HouseholdMemberIdentifier, Is.EqualTo(default(Guid)));
            Assert.That(memberOfHouseholdModel.HouseholdIdentifier, Is.EqualTo(householdIdentifier));
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Null);
            Assert.That(memberOfHouseholdModel.Removable, Is.False);
        }

        /// <summary>
        /// Creates a controller for a household for unit testing.
        /// </summary>
        /// <param name="household">Sets the model for the household which should be used.</param>
        /// <param name="householdGetterCallback">Sets a callback action which are executed when we get the household to use.</param>
        /// <param name="updatedHousehold">Sets the model for the updated household.</param>
        /// <returns>Controller for a household for unit testing.</returns>
        private HouseholdController CreateHouseholdController(HouseholdModel household = null, Action<object[]> householdGetterCallback = null, HouseholdModel updatedHousehold = null)
        {
            Func<HouseholdModel> householdGetter = () =>
            {
                if (household != null)
                {
                    return household;
                }
                return Fixture.Build<HouseholdModel>()
                    .With(m => m.HouseholdMembers, null)
                    .Create();
            };
            Func<HouseholdModel> householdUpdater = () =>
            {
                if (updatedHousehold != null)
                {
                    return updatedHousehold;
                }
                return Fixture.Build<HouseholdModel>()
                    .With(m => m.HouseholdMembers, null)
                    .Create();
            };

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
                .Return(Task.Run(householdGetter))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.UpdateHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything))
                .Return(Task.Run(householdUpdater))
                .Repeat.Any();

            var householdController = new HouseholdController(_householdDataRepositoryMock);
            householdController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdController);
            return householdController;
        }
    }
}
