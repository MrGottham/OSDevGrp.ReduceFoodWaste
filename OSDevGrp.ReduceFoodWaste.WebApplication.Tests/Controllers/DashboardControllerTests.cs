using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

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
            var identityMock = MockRepository.GenerateMock<IIdentity>();
            identityMock.Stub(m => m.IsAuthenticated)
                .Return(true)
                .Repeat.Any();

            var userMock = MockRepository.GenerateMock<IPrincipal>();
            userMock.Stub(m => m.Identity)
                .Return(identityMock)
                .Repeat.Any();

            var httpContextMock = MockRepository.GenerateMock<HttpContextBase>();
            httpContextMock.Stub(m => m.User)
                .Return(userMock)
                .Repeat.Any();

            var dashboardController = new DashboardController();
            dashboardController.ControllerContext = new ControllerContext(httpContextMock, new RouteData(), dashboardController);
            return dashboardController;
        }
    }
}
