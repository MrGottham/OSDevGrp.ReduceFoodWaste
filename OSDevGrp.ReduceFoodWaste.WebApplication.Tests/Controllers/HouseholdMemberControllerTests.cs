using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Tests the controller for a household member.
    /// </summary>
    public class HouseholdMemberControllerTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the controller for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberController()
        {
            var householdMemberController = new HouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
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
        private static HouseholdMemberController CreateHouseholdMemberController()
        {
            var householdMemberController = new HouseholdMemberController();
            householdMemberController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdMemberController);
            return householdMemberController;
        }
    }
}
