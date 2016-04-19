using System;
using System.Security.Principal;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories
{
    /// <summary>
    /// Tests the provider which can creates credentials.
    /// </summary>
    [TestFixture]
    public class CredentialsProviderTests : TestBase
    {
        #region Private variables

        private IClaimValueProvider _claimValueProviderMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
        }

        /// <summary>
        /// Tests that the constructor initialize the provider which can creates credentials.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeCredentialsProvider()
        {
            var credentialsProvider = new CredentialsProvider(_claimValueProviderMock);
            Assert.That(credentialsProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can get values from claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new CredentialsProvider(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that CreateUserNamePasswordCredential throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatCreateUserNamePasswordCredentialThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var credentialsProvider = CreateCredentialsProvider();
            Assert.That(credentialsProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => credentialsProvider.CreateUserNamePasswordCredential(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that CreateUserNamePasswordCredential calls GetMailAddress on the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatCreateUserNamePasswordCredentialCallsGetMailAddressOnClaimValueProvider()
        {
            var credentialsProvider = CreateCredentialsProvider();
            Assert.That(credentialsProvider, Is.Not.Null);

            var identityMock = MockRepository.GenerateMock<IIdentity>();
            Assert.That(identityMock, Is.Not.Null);

            credentialsProvider.CreateUserNamePasswordCredential(identityMock);

            _claimValueProviderMock.AssertWasCalled(m => m.GetMailAddress(Arg<IIdentity>.Is.Equal(identityMock)));
        }

        /// <summary>
        /// Tests that CreateUserNamePasswordCredential calls GetUserNameIdentifier on the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatCreateUserNamePasswordCredentialCallsGetUserNameIdentifierOnClaimValueProvider()
        {
            var credentialsProvider = CreateCredentialsProvider();
            Assert.That(credentialsProvider, Is.Not.Null);

            var identityMock = MockRepository.GenerateMock<IIdentity>();
            Assert.That(identityMock, Is.Not.Null);

            credentialsProvider.CreateUserNamePasswordCredential(identityMock);

            _claimValueProviderMock.AssertWasCalled(m => m.GetUserNameIdentifier(Arg<IIdentity>.Is.Equal(identityMock)));
        }

        /// <summary>
        /// Tests that CreateUserNamePasswordCredential throws an ReduceFoodWasteRepositoryException when a mail address cannot be resolved through the identity.
        /// </summary>
        [Test]
        public void TestThatCreateUserNamePasswordCredentialThrowsReduceFoodWasteRepositoryExceptionWhenMailAddressForIdentityIsNull()
        {
            var credentialsProvider = CreateCredentialsProvider(hasMailAddress: false);
            Assert.That(credentialsProvider, Is.Not.Null);

            var exception = Assert.Throws<ReduceFoodWasteRepositoryException>(() => credentialsProvider.CreateUserNamePasswordCredential(MockRepository.GenerateMock<IIdentity>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.CannotResolveMailAddressFromIdentity));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that CreateUserNamePasswordCredential throws an ReduceFoodWasteRepositoryException when a user name identifier cannot be resolved through the identity.
        /// </summary>
        [Test]
        public void TestThatCreateUserNamePasswordCredentialThrowsReduceFoodWasteRepositoryExceptionWhenUserNameIdentifierForIdentityIsNull()
        {
            var credentialsProvider = CreateCredentialsProvider(hasUserNameIdentifier: false);
            Assert.That(credentialsProvider, Is.Not.Null);

            var exception = Assert.Throws<ReduceFoodWasteRepositoryException>(() => credentialsProvider.CreateUserNamePasswordCredential(MockRepository.GenerateMock<IIdentity>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.CannotResolveUserNameIdentifierFromIdentity));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that CreateUserNamePasswordCredential creates a user name and password credential containing mail address and user name identifier.
        /// </summary>
        [Test]
        public void TestThatCreateUserNamePasswordCredentialCreatesUserNamePasswordCredentialContainingMailAddressAndUserNameIdentifier()
        {
            var mailAddress = Fixture.Create<string>();
            var userNameIdentifier = Fixture.Create<string>();

            var credentialsProvider = CreateCredentialsProvider(mailAddress: mailAddress, userNameIdentifier: userNameIdentifier);
            Assert.That(credentialsProvider, Is.Not.Null);

            var userNamePasswordCredential = credentialsProvider.CreateUserNamePasswordCredential(MockRepository.GenerateMock<IIdentity>());
            Assert.That(userNamePasswordCredential, Is.Not.Null);
            Assert.That(userNamePasswordCredential.UserName, Is.Not.Null);
            Assert.That(userNamePasswordCredential.UserName, Is.Not.Empty);
            Assert.That(userNamePasswordCredential.UserName, Is.EqualTo(mailAddress));
            Assert.That(userNamePasswordCredential.Password, Is.Not.Null);
            Assert.That(userNamePasswordCredential.Password, Is.Not.Empty);
            Assert.That(userNamePasswordCredential.Password, Is.EqualTo(userNameIdentifier));
        }

        /// <summary>
        /// Creates a provider which can creates credentials for unit testing.
        /// </summary>
        /// <param name="hasMailAddress">Sets whether a mail address can be resolved from the identity.</param>
        /// <param name="mailAddress">Sets the mail address which should be resolved from the identity.</param>
        /// <param name="hasUserNameIdentifier">Sets whether a user name identifier can be resolved from the identity.</param>
        /// <param name="userNameIdentifier">Sets the user name identifier which should be resolved from the identity.</param>
        /// <returns>Provider which can creates credentials for unit testing.</returns>
        private ICredentialsProvider CreateCredentialsProvider(bool hasMailAddress = true, string mailAddress = null, bool hasUserNameIdentifier = true, string userNameIdentifier = null)
        {
            _claimValueProviderMock.Stub(m => m.GetMailAddress(Arg<IIdentity>.Is.Anything))
                .Return(hasMailAddress ? mailAddress ?? Fixture.Create<string>() : null)
                .Repeat.Any();
            _claimValueProviderMock.Stub(m => m.GetUserNameIdentifier(Arg<IIdentity>.Is.Anything))
                .Return(hasUserNameIdentifier ? userNameIdentifier ?? Fixture.Create<string>() : null)
                .Repeat.Any();

            return new CredentialsProvider(_claimValueProviderMock);
        }
    }
}
