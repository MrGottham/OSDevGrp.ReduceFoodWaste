using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberModel()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
        }
    }
}
