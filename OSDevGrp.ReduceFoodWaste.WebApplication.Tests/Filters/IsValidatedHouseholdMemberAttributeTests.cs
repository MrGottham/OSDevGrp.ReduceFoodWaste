using System;
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

        /// <summary>
        /// Tests that OnActionExecuting throws an ArgumentNullException when the filter context is null.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsArgumentNullExceptionWhenFilterContextIsNull()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => isValidatedHouseholdMemberAttribute.OnActionExecuting(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("filterContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnResultExecuting throws an ArgumentNullException when the filter context is null.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingThrowsArgumentNullExceptionWhenFilterContextIsNull()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => isValidatedHouseholdMemberAttribute.OnResultExecuting(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("filterContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Creates an attribute which can insure that the user is a validated household member for unit testing.
        /// </summary>
        /// <returns>Attribute which can insure that the user is a validated household member for unit testing</returns>
        private static IsValidatedHouseholdMemberAttribute CreateIsValidatedHouseholdMemberAttribute()
        {
            return new IsValidatedHouseholdMemberAttribute();
        }
    }
}
