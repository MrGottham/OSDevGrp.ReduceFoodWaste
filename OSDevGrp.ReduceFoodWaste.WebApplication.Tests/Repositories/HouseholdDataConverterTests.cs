using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories
{
    /// <summary>
    /// Tests the converter which can convert household data.
    /// </summary>
    [TestFixture]
    public class HouseholdDataConverterTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a converter which can convert household data.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataConverter()
        {
            var householdDataConverter = new HouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Convert throws an ArgumentNullException when the source object is null.
        /// </summary>
        [Test]
        public void TestThatConvertThrowsArgumentNullExceptionWhenSourceObjectIsNull()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataConverter.Convert<object, object>(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("source"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Creates a converter which can convert household data for unit testing.
        /// </summary>
        /// <returns>Converter which can convert household data for unit testing.</returns>
        private static IHouseholdDataConverter CreateHouseholdDataConverter()
        {
            return new HouseholdDataConverter();
        }
    }
}
