using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a household.
    /// </summary>
    [TestFixture]
    public class HouseholdModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdModel()
        {
            var householdModel = new HouseholdModel();
            Assert.That(householdModel, Is.Not.Null);
        }
    }
}
