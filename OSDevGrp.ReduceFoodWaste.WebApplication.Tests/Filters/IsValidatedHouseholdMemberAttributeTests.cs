using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Filters
{
    /// <summary>
    /// Tests the attribute which can insure that the user is a validated household member.
    /// </summary>
    [TestFixture]
    public class IsValidatedHouseholdMemberAttributeTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize an attribute which can insure that the user is a validated household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeIsValidatedHouseholdMemberAttribute()
        {
            var isValidatedHouseholdMemberAttribute = new IsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);
        }
    }
}
