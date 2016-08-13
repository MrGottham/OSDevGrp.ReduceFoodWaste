using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;

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
        /// Tests that Manage with identification for a given household returns a ViewResult with a model for manage the given household.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierReturnsViewResultWithModelForManageHousehold()
        {
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            var householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.EqualTo(default(Guid)));

            var result = householdController.Manage(householdIdentifier);
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
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier));
        }

        /// <summary>
        /// Creates a controller for a household for unit testing.
        /// </summary>
        /// <returns>Controller for a household for unit testing.</returns>
        private HouseholdController CreateHouseholdController()
        {
            var householdController = new HouseholdController(_householdDataRepositoryMock);
            householdController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdController);
            return householdController;
        }
    }
}
