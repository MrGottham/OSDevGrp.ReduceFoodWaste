using System;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Tests the controller for a household members dashboard.
    /// </summary>
    [TestFixture]
    public class DashboardControllerTests : TestBase
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
        /// Tests that the constructor initialize the controller for a household members dashboard.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDashboardController()
        {
            var dashboardController = new DashboardController(_householdDataRepositoryMock);
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
        /// Tests that HouseholdMemberInformation calls GetHouseholdMemberAsync on the repository which can access household data..
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberInformationCallsGetHouseholdMemberAsyncOnHouseholdDataRepository()
        {
            var dashboardController = CreateDashboardController();
            Assert.That(dashboardController, Is.Not.Null);
            Assert.That(dashboardController.User, Is.Not.Null);
            Assert.That(dashboardController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            dashboardController.HouseholdMemberInformation();

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Equal(dashboardController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Creates a controller for a household members dashboard for unit testing.
        /// </summary>
        /// <param name="householdMember">Sets the household member which should be used for the dashboard.</param>
        /// <returns>Controller for a household members dashboard for unit testing.</returns>
        private DashboardController CreateDashboardController(HouseholdMemberModel householdMember = null)
        {
            Func<HouseholdMemberModel> householdMemberGetter = () =>
            {
                if (householdMember != null)
                {
                    return householdMember;
                }
                return new HouseholdMemberModel
                {
                    Households = Fixture.CreateMany<HouseholdModel>(Random.Next(1, 5)).ToList()
                };
            };

            _householdDataRepositoryMock.Stub(
                m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(householdMemberGetter))
                .Repeat.Any();

            var dashboardController = new DashboardController(_householdDataRepositoryMock);
            dashboardController.ControllerContext = ControllerTestHelper.CreateControllerContext(dashboardController);
            return dashboardController;
        }
    }
}
