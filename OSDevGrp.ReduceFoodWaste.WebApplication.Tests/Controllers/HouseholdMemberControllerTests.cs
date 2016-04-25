using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Tests the controller for a household member.
    /// </summary>
    public class HouseholdMemberControllerTests : TestBase
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepository;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _householdDataRepository = MockRepository.GenerateMock<IHouseholdDataRepository>();
        }

        /// <summary>
        /// Tests that the constructor initialize the controller for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberController()
        {
            var householdMemberController = new HouseholdMemberController(_householdDataRepository);
            Assert.That(householdMemberController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberController(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Create throws NotImplementedException.
        /// </summary>
        [Test]
        public void TestThatDashboardThrowsNotImplementedException()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.Throws<NotImplementedException>(() => householdMemberController.Create());
        }

        /// <summary>
        /// Tests that Prepare throws NotImplementedException.
        /// </summary>
        [Test]
        public void TestThatPrepareThrowsNotImplementedException()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.Throws<NotImplementedException>(() => householdMemberController.Prepare());
        }

        /// <summary>
        /// Creates a controller for a household member for unit testing.
        /// </summary>
        /// <returns>Controller for a household member for unit testing.</returns>
        private HouseholdMemberController CreateHouseholdMemberController()
        {
            var householdMemberController = new HouseholdMemberController(_householdDataRepository);
            householdMemberController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdMemberController);
            return householdMemberController;
        }
    }
}
