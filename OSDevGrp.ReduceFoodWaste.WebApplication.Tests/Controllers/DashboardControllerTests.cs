using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
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
        /// Tests that Dashboard throws NotImplementedException.
        /// </summary>
        [Test]
        public void TestThatDashboardThrowsNotImplementedException()
        {
            var createDashboardController = CreateDashboardController();
            Assert.That(createDashboardController, Is.Not.Null);

            Assert.Throws<NotImplementedException>(() => createDashboardController.Dashboard());
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
