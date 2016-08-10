using System.Web.Mvc;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Tests the controller for a household members dashboard.
    /// </summary>
    [TestFixture]
    public class DashboardControllerTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the controller for a household members dashboard.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDashboardController()
        {
            var dashboardController = new DashboardController();
            Assert.That(dashboardController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Dashboard returns a ViewResult with a model for a dashboard.
        /// </summary>
        [Test]
        public void TestThatDashboardReturnsViewResultWithDashboardModel()
        {
            var dashboardController = CreateDashboardController();
            Assert.That(dashboardController, Is.Not.Null);

            var result = dashboardController.Dashboard();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Empty);
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<DashboardModel>());

            var dashboardModel = (DashboardModel) viewResult.Model;
            Assert.That(dashboardModel, Is.Not.Null);
            Assert.That(dashboardModel.HouseholdMember, Is.Null);
            Assert.That(dashboardModel.Households, Is.Null);
        }

        /// <summary>
        /// Creates a controller for a household members dashboard for unit testing.
        /// </summary>
        /// <returns>Controller for a household members dashboard for unit testing.</returns>
        private static DashboardController CreateDashboardController()
        {
            var dashboardController = new DashboardController();
            dashboardController.ControllerContext = ControllerTestHelper.CreateControllerContext(dashboardController);
            return dashboardController;
        }
    }
}
