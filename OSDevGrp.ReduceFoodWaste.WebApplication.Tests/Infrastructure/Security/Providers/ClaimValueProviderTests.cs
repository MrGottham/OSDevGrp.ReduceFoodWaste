using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Infrastructure.Security.Providers
{
    /// <summary>
    /// Tests the provider which can get values from claims.
    /// </summary>
    [TestFixture]
    public class ClaimValueProviderTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeClaimValueProvider()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tests that IsAuthenticated throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsAuthenticatedThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsAuthenticated(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsAuthenticated calls IsAuthenticated on the identity.
        /// </summary>
        [Test]
        public void TestThatIsAuthenticatedCallsIsAuthenticatedOnIdentity()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var identityMock = MockRepository.GenerateMock<IIdentity>();
            identityMock.Stub(m => m.IsAuthenticated)
                .Return(Fixture.Create<bool>())
                .Repeat.Any();

            claimValueProvider.IsAuthenticated(identityMock);

            identityMock.AssertWasCalled(m => m.IsAuthenticated);
        }

        /// <summary>
        /// Tests that IsAuthenticated returns the result from IsAuthenticated on the identity.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatIsAuthenticatedReturnsResultFromIsAuthenticatedOnIdentity(bool expectedValue)
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var identityMock = MockRepository.GenerateMock<IIdentity>();
            identityMock.Stub(m => m.IsAuthenticated)
                .Return(expectedValue)
                .Repeat.Any();

            var result = claimValueProvider.IsAuthenticated(identityMock);
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that GetUserNameIdentifier when called with a identity throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatGetUserNameIdentifierWhenCalledWithIdentityThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.GetUserNameIdentifier((IIdentity) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetUserNameIdentifier when called with a claims identity throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatGetUserNameIdentifierWhenCalledWithClaimsIdentityThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.GetUserNameIdentifier(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetUserNameIdentifier when called with a claims identity returns null when the claim collection is empty.
        /// </summary>
        [Test]
        public void TestThatGetUserNameIdentifierWhenCalledWithClaimsIdentityReturnsNullWhenClaimCollectionIsEmpty()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>(0);
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.GetUserNameIdentifier(claimsIdentity);
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that GetUserNameIdentifier when called with a claims identity returns null when a claim for the user name identifier does not exist.
        /// </summary>
        [Test]
        public void TestThatGetUserNameIdentifierWhenCalledWithClaimsIdentityReturnsNullWhenClaimForUserNameIdentifierDoesNotExist()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.GetUserNameIdentifier(claimsIdentity);
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that GetUserNameIdentifier when called with a claims identity returns user name identifier when a claim for the user name identifier does exist.
        /// </summary>
        [Test]
        public void TestThatGetUserNameIdentifierWhenCalledWithClaimsIdentityReturnsUserNameIdentifierWhenClaimForUserNameIdentifierDoesExist()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var userNameIdentifier = Fixture.Create<string>();
            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userNameIdentifier, ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String),
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.GetUserNameIdentifier(claimsIdentity);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(userNameIdentifier));
        }

        /// <summary>
        /// Tests that GetMailAddress when called with a identity throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatGetMailAddressWhenCalledWithIdentityThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.GetMailAddress((IIdentity) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetMailAddress when called with a claims identity throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatGetMailAddressWhenCalledWithClaimsIdentityThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.GetMailAddress(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetMailAddress when called with a claims identity returns null when the claim collection is empty.
        /// </summary>
        [Test]
        public void TestThatGetMailAddressWhenCalledWithClaimsIdentityReturnsNullWhenClaimCollectionIsEmpty()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>(0);
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.GetMailAddress(claimsIdentity);
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that GetMailAddress when called with a claims identity returns null when a claim for the mail address does not exist.
        /// </summary>
        [Test]
        public void TestThatGetMailAddressWhenCalledWithClaimsIdentityReturnsNullWhenClaimForMailAddressDoesNotExist()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.GetMailAddress(claimsIdentity);
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Tests that GetMailAddress when called with a claims identity returns mail address when a claim for the mail address does exist.
        /// </summary>
        [Test]
        public void TestThatGetMailAddressWhenCalledWithClaimsIdentityReturnsMailAddressWhenClaimForMailAddressDoesExist()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var mailAddress = Fixture.Create<string>();
            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Email, mailAddress, ClaimValueTypes.String)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.GetMailAddress(claimsIdentity);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(mailAddress));
        }
    }
}
