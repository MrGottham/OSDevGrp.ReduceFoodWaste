using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories
{
    /// <summary>
    /// Tests the repository which can access household data.
    /// </summary>
    [TestFixture]
    public class HouseholdDataRepositoryTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataRepository()
        {
            var householdDataRepository = new HouseholdDataRepository();
            Assert.That(householdDataRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tests that IsHouseholdMember throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsHouseholdMemberCreatedThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.IsHouseholdMemberCreated(null).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Creates a repository which can access household data for unit testing.
        /// </summary>
        /// <returns>Repository which can access household data for unit testing.</returns>
        private IHouseholdDataRepository CreateHouseholdDataRepository()
        {
            return new HouseholdDataRepository();
        }
    }
}
