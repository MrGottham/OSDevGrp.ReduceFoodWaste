using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories
{
    /// <summary>
    /// Tests the repository which can access household data.
    /// </summary>
    [TestFixture]
    public class HouseholdDataRepositoryTests : TestBase
    {
        #region Private variables

        private ICredentialsProvider _credentialsProviderMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _credentialsProviderMock = MockRepository.GenerateMock<ICredentialsProvider>();
        }

        /// <summary>
        /// Tests that the constructor initialize a repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataRepository()
        {
            var householdDataRepository = new HouseholdDataRepository(_credentialsProviderMock);
            Assert.That(householdDataRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can creates credentials is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCredentialsProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdDataRepository(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("credentialsProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsHouseholdMemberCreatedAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsHouseholdMemberCreatedAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.IsHouseholdMemberCreatedAsync(null));
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
            return new HouseholdDataRepository(_credentialsProviderMock);
        }
    }
}
