using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a membership.
    /// </summary>
    [TestFixture]
    public class MembershipModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a membership.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeMembershipModel()
        {
            var membershipModel = new MembershipModel();
            Assert.That(membershipModel, Is.Not.Null);
        }
    }
}
