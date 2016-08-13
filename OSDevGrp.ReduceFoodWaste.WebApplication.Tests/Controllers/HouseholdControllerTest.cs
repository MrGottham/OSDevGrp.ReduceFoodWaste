using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
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
        /// Tests that Manage with identification for the household throws an NotImplementedException.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierThrowsNotImplementedException()
        {
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            Assert.Throws<NotImplementedException>(() => householdController.Manage(Guid.NewGuid()));
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
