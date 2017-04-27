using System.Globalization;
using System.Linq;
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
    /// Tests the controller for the sidebar.
    /// </summary>
    [TestFixture]
    public class SidebarControllerTests : TestBase
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
        /// Tests that HouseholdIdentificationCollection calls GetHouseholdIdentificationCollectionAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatHouseholdIdentificationCollectionCallsGetHouseholdIdentificationCollectionAsyncOnHouseholdDataRepository()
        {
            var sidebarController = CreateSidebarController();
            Assert.That(sidebarController, Is.Not.Null);
            Assert.That(sidebarController.User, Is.Not.Null);
            Assert.That(sidebarController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            sidebarController.HouseholdIdentificationCollection();

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdIdentificationCollectionAsync(Arg<IIdentity>.Is.Equal(sidebarController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that HouseholdIdentificationCollection returns a PartialViewResult with a model for the household identification collection.
        /// </summary>
        [Test]
        public void TestThatHouseholdIdentificationCollectionReturnsPartialViewResultWithHouseholdIdentificationCollectionModel()
        {
            HouseholdIdentificationCollectionModel householdIdentificationCollectionModel = new HouseholdIdentificationCollectionModel(Fixture.CreateMany<HouseholdIdentificationModel>(Random.Next(3, 10)).ToList(), Fixture.Create<bool>());
            Assert.That(householdIdentificationCollectionModel, Is.Not.Null);
            Assert.That(householdIdentificationCollectionModel, Is.Not.Empty);

            var sidebarController = CreateSidebarController(householdIdentificationCollectionModel: householdIdentificationCollectionModel);
            Assert.That(sidebarController, Is.Not.Null);

            var result = sidebarController.HouseholdIdentificationCollection();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            var partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("_HouseholdIdentificationCollection"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Empty);
            Assert.That(partialViewResult.Model, Is.Not.Null);
            Assert.That(partialViewResult.Model, Is.Not.Empty);
            Assert.That(partialViewResult.Model, Is.EqualTo(householdIdentificationCollectionModel));
        }

        /// <summary>
        /// Creates a controller for the sidebar for unit testing.
        /// </summary>
        /// <param name="householdIdentificationCollectionModel">Sets the model for the collection of household identifications.</param>
        /// <returns>Controller for the sidebar for unit testing.</returns>
        private SidebarController CreateSidebarController(HouseholdIdentificationCollectionModel householdIdentificationCollectionModel = null)
        {
            HouseholdIdentificationCollectionModel householdIdentificationCollectionGetter = householdIdentificationCollectionModel ?? new HouseholdIdentificationCollectionModel(Fixture.CreateMany<HouseholdIdentificationModel>(Random.Next(3, 10)).ToList(), Fixture.Create<bool>());
            _householdDataRepositoryMock.Stub(m => m.GetHouseholdIdentificationCollectionAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(() => householdIdentificationCollectionGetter))
                .Repeat.Any();

            var sidebarController = new SidebarController(_householdDataRepositoryMock);
            sidebarController.ControllerContext = ControllerTestHelper.CreateControllerContext(sidebarController);
            return sidebarController;
        }
    }
}
