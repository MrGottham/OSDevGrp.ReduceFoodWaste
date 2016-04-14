using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Infrastructure.Security.Providers
{
    /// <summary>
    /// Tests the provider which can append local claims to a claims identity.
    /// </summary>
    [TestFixture]
    public class LocalClaimProviderTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeLocalClaimProvider()
        {
            var localClaimProvider = new LocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localClaimProvider.AddLocalClaimsAsync(null).Wait());
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Creates a provider which can append local claims to a claims identity for unit testing.
        /// </summary>
        /// <returns>Provider which can append local claims to a claims identity for unit testing.</returns>
        private static ILocalClaimProvider CreateLocalClaimProvider()
        {
            return new LocalClaimProvider();
        }
    }
}
