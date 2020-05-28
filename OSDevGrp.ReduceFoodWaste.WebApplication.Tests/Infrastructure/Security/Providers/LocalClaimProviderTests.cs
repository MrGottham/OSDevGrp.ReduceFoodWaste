using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Infrastructure.Security.Providers
{
    /// <summary>
    /// Tests the provider which can append local claims to a claims identity.
    /// </summary>
    [TestFixture]
    public class LocalClaimProviderTests : TestBase
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
        }

        /// <summary>
        /// Tests that the constructor initialize a provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeLocalClaimProvider()
        {
            ILocalClaimProvider localClaimProvider = new LocalClaimProvider(_householdDataRepositoryMock);
            Assert.That(localClaimProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCredentialsProviderIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new LocalClaimProvider(null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await localClaimProvider.AddLocalClaimsAsync(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync calls IsHouseholdMemberCreatedAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncCallsIsHouseholdMemberCreatedAsyncOnHouseholdDataRepository()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            _householdDataRepositoryMock.AssertWasCalled(m => m.IsHouseholdMemberCreatedAsync(Arg<IIdentity>.Is.Equal(claimsIdentity)));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when IsHouseholdMemberCreatedAsync on the repository which can access household data throws an ReduceFoodWasteExceptionBase.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenIsHouseholdMemberCreatedAsyncOnHouseholdDataRepositoryThrowsReduceFoodWasteExceptionBase()
        {
            ReduceFoodWasteBusinessException exceptionToThrow = Fixture.Create<ReduceFoodWasteBusinessException>();

            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreatedAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            ReduceFoodWasteBusinessException result = Assert.ThrowsAsync<ReduceFoodWasteBusinessException>(async () => await localClaimProvider.AddLocalClaimsAsync(claimsIdentity));

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when IsHouseholdMemberCreatedAsync on the repository which can access household data throws an Exception.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenIsHouseholdMemberCreatedAsyncOnHouseholdDataRepositoryThrowsException()
        {
            Exception exceptionToThrow = Fixture.Create<Exception>();

            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreatedAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            ReduceFoodWasteSystemException result = Assert.ThrowsAsync<ReduceFoodWasteSystemException>(async () => await localClaimProvider.AddLocalClaimsAsync(claimsIdentity));

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ReduceFoodWasteSystemException>());
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
            Assert.That(result.Message, Is.EqualTo(exceptionToThrow.Message));
            Assert.That(result.InnerException, Is.Not.Null);
            Assert.That(result.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync adds claim for the household member has been created when a household member has been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncAddsClaimForCreatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember);
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(Convert.ToString(true)));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(ClaimValueTypes.Boolean));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been created when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForCreatedHouseholdMemberWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been activated when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForActivatedHouseholdMemberWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has accepted privacy policies when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForPrivacyPoliciesAcceptedWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been validated when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not call IsHouseholdMemberActivatedAsync on the repository which can access household data when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotCallIsHouseholdMemberActivatedAsyncOnHouseholdDataRepositoryWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.IsHouseholdMemberActivatedAsync(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not call HasHouseholdMemberAcceptedPrivacyPolicyAsync on the repository which can access household data when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotCallHasHouseholdMemberAcceptedPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.HasHouseholdMemberAcceptedPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync calls IsHouseholdMemberActivatedAsync on the repository which can access household data when a household member has been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncCallsIsHouseholdMemberActivatedAsyncOnHouseholdDataRepositoryWhenHouseholdMemberForIdentityHasBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            _householdDataRepositoryMock.AssertWasCalled(m => m.IsHouseholdMemberActivatedAsync(Arg<IIdentity>.Is.Equal(claimsIdentity)));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when IsHouseholdMemberActivatedAsync on the repository which can access household data throws an ReduceFoodWasteExceptionBase.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenIsHouseholdMemberActivatedAsyncOnHouseholdDataRepositoryThrowsReduceFoodWasteExceptionBase()
        {
            ReduceFoodWasteBusinessException exceptionToThrow = Fixture.Create<ReduceFoodWasteBusinessException>();

            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivatedAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            ReduceFoodWasteBusinessException result = Assert.ThrowsAsync<ReduceFoodWasteBusinessException>(async () => await localClaimProvider.AddLocalClaimsAsync(claimsIdentity));

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when IsHouseholdMemberActivatedAsync on the repository which can access household data throws an Exception.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenIsHouseholdMemberActivatedAsyncOnHouseholdDataRepositoryThrowsException()
        {
            Exception exceptionToThrow = Fixture.Create<Exception>();

            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivatedAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            ReduceFoodWasteSystemException result = Assert.ThrowsAsync<ReduceFoodWasteSystemException>(async () => await localClaimProvider.AddLocalClaimsAsync(claimsIdentity));

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ReduceFoodWasteSystemException>());
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
            Assert.That(result.Message, Is.EqualTo(exceptionToThrow.Message));
            Assert.That(result.InnerException, Is.Not.Null);
            Assert.That(result.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync calls HasHouseholdMemberAcceptedPrivacyPolicyAsync on the repository which can access household data when a household member has been created for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncCallsHasHouseholdMemberAcceptedPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberForIdentityHasBeenCreated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            _householdDataRepositoryMock.AssertWasCalled(m => m.HasHouseholdMemberAcceptedPrivacyPolicyAsync(Arg<IIdentity>.Is.Equal(claimsIdentity)));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when HasHouseholdMemberAcceptedPrivacyPolicyAsync on the repository which can access household data throws an ReduceFoodWasteExceptionBase.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenHasHouseholdMemberAcceptedPrivacyPolicyAsyncOnHouseholdDataRepositoryThrowsReduceFoodWasteExceptionBase()
        {
            ReduceFoodWasteBusinessException exceptionToThrow = Fixture.Create<ReduceFoodWasteBusinessException>();

            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(hasHouseholdMemberAcceptedPrivacyPolicyAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            ReduceFoodWasteBusinessException result = Assert.ThrowsAsync<ReduceFoodWasteBusinessException>(async () => await localClaimProvider.AddLocalClaimsAsync(claimsIdentity));

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when HasHouseholdMemberAcceptedPrivacyPolicyAsync on the repository which can access household data throws an Exception.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenHasHouseholdMemberAcceptedPrivacyPolicyAsyncOnHouseholdDataRepositoryThrowsException()
        {
            Exception exceptionToThrow = Fixture.Create<Exception>();

            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(hasHouseholdMemberAcceptedPrivacyPolicyAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            ReduceFoodWasteSystemException result = Assert.ThrowsAsync<ReduceFoodWasteSystemException>(async () => await localClaimProvider.AddLocalClaimsAsync(claimsIdentity));

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ReduceFoodWasteSystemException>());
            Assert.That(result.Message, Is.Not.Null);
            Assert.That(result.Message, Is.Not.Empty);
            Assert.That(result.Message, Is.EqualTo(exceptionToThrow.Message));
            Assert.That(result.InnerException, Is.Not.Null);
            Assert.That(result.InnerException, Is.EqualTo(exceptionToThrow));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync adds claim for the household member has been activated when a household member has been created and activated for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncAddsClaimForActivatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedAndActivated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember);
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(Convert.ToString(true)));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(ClaimValueTypes.Boolean));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been activated when a household member has been created but not activated for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForActivatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedButNotActivated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync adds claim for the household member has accepted privacy policies when a household member has been created and accepted privacy policies for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncAddsClaimForPrivacyPoliciesAcceptedWhenHouseholdMemberForIdentityHasBeenCreatedAndAcceptedPrivacyPolicies()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted);
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(Convert.ToString(true)));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(ClaimValueTypes.Boolean));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has accepted privacy policies when a household member has been created but not accepted privacy policies for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForPrivacyPoliciesAcceptedWhenHouseholdMemberForIdentityHasBeenCreatedButNotAcceptedPrivacyPolicies()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(hasHouseholdMemberAcceptedPrivacyPolicy: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync adds claim for the household member has been validated when a household member has been created, has been activated and accepted privacy policies for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncAddsClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedHasBeenActivatedAndAcceptedPrivacyPolicies()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(Convert.ToString(true)));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(ClaimValueTypes.Boolean));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been validated when a household member has been created but not activated and not accepted privacy policies for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedButNotActivatedAndNotAcceptedPrivacyPolicies()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivated: false, hasHouseholdMemberAcceptedPrivacyPolicy: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been validated when a household member has been created and accepted privacy policies but not activated for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedHasAcceptedPrivacyPoliciesButNotActivated()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been validated when a household member has been created have been activated but not accepted privacy policies for the identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimsAsyncDoesNotAddClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedHaveBeenActivatedButNotAcceptedPrivacyPolicies()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider(hasHouseholdMemberAcceptedPrivacyPolicy: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            await localClaimProvider.AddLocalClaimsAsync(claimsIdentity);

            Claim claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimAsync throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimAsyncThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await localClaimProvider.AddLocalClaimAsync(null, new Claim(Fixture.Create<string>(), Fixture.Create<string>())));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimAsync throws an ArgumentNullException when the claim to add is null.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimAsyncThrowsArgumentNullExceptionWhenClaimToAddIsNull()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            ArgumentNullException exception = Assert.ThrowsAsync<ArgumentNullException>(async () => await localClaimProvider.AddLocalClaimAsync(new ClaimsIdentity(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimToAdd"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimAsync adds a local claim to the claims identity.
        /// </summary>
        [Test]
        public async Task TestThatAddLocalClaimAsyncAddsLocalClaimToClaimsIdentity()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            Claim claimToAdd = new Claim(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(claimToAdd, Is.Not.Null);
            Assert.That(claimToAdd.Type, Is.Not.Null);
            Assert.That(claimToAdd.Type, Is.Not.Empty);
            Assert.That(claimToAdd.Value, Is.Not.Null);
            Assert.That(claimToAdd.Value, Is.Not.Empty);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(claimToAdd.Type), Is.Null);

            await localClaimProvider.AddLocalClaimAsync(claimsIdentity, claimToAdd);

            Claim claim = claimsIdentity.FindFirst(claimToAdd.Type);
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Empty);
            Assert.That(claim.Type, Is.EqualTo(claimToAdd.Type));
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(claimToAdd.Value));
        }

        /// <summary>
        /// Tests that GenerateCreatedHouseholdMemberClaim generates a claim which indicates that a claim identity has been created as a household member.
        /// </summary>
        [Test]
        public void TestThatGenerateCreatedHouseholdMemberClaimGeneratesClaim()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            Claim claim = localClaimProvider.GenerateCreatedHouseholdMemberClaim();
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Empty);
            Assert.That(claim.Type, Is.EqualTo(LocalClaimTypes.CreatedHouseholdMember));
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(Convert.ToString(true)));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(ClaimValueTypes.Boolean));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
        }

        /// <summary>
        /// Tests that GenerateActivatedHouseholdMemberClaim generates a claim which indicates that a claim identity is an activated household member.
        /// </summary>
        [Test]
        public void TestThatGenerateActivatedHouseholdMemberClaimGeneratesClaim()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            Claim claim = localClaimProvider.GenerateActivatedHouseholdMemberClaim();
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Empty);
            Assert.That(claim.Type, Is.EqualTo(LocalClaimTypes.ActivatedHouseholdMember));
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(Convert.ToString(true)));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(ClaimValueTypes.Boolean));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
        }

        /// <summary>
        /// Tests that GeneratePrivacyPoliciesAcceptedClaim generates a claim which indicates that a claim identity has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatGeneratePrivacyPoliciesAcceptedClaimGeneratesClaim()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            Claim claim = localClaimProvider.GeneratePrivacyPoliciesAcceptedClaim();
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Empty);
            Assert.That(claim.Type, Is.EqualTo(LocalClaimTypes.PrivacyPoliciesAccepted));
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(Convert.ToString(true)));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(ClaimValueTypes.Boolean));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
        }

        /// <summary>
        /// Tests that GeneratePrivacyPoliciesAcceptedClaim generates a claim which indicates that a claim identity has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatGenerateValidatedHouseholdMemberClaimGeneratesClaim()
        {
            ILocalClaimProvider localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            Claim claim = localClaimProvider.GenerateValidatedHouseholdMemberClaim();
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Empty);
            Assert.That(claim.Type, Is.EqualTo(LocalClaimTypes.ValidatedHouseholdMember));
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(Convert.ToString(true)));
            Assert.That(claim.ValueType, Is.Not.Null);
            Assert.That(claim.ValueType, Is.Not.Empty);
            Assert.That(claim.ValueType, Is.EqualTo(ClaimValueTypes.Boolean));
            Assert.That(claim.Issuer, Is.Not.Null);
            Assert.That(claim.Issuer, Is.Not.Empty);
            Assert.That(claim.Issuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
            Assert.That(claim.OriginalIssuer, Is.Not.Null);
            Assert.That(claim.OriginalIssuer, Is.Not.Empty);
            Assert.That(claim.OriginalIssuer, Is.EqualTo(LocalClaimProvider.LocalClaimIssuer));
        }

        /// <summary>
        /// Creates a provider which can append local claims to a claims identity for unit testing.
        /// </summary>
        /// <param name="isHouseholdMemberCreated">Sets whether a household member are created for the identity.</param>
        /// <param name="isHouseholdMemberActivated">Sets whether the household member for the given identity has been activated.</param>
        /// <param name="hasHouseholdMemberAcceptedPrivacyPolicy">Sets whether the household member for the given identity has accepted the privacy policies.</param>
        /// <param name="isHouseholdMemberCreatedAsyncException">Sets the exception which should be thrown by IsHouseholdMemberCreatedAsync.</param>
        /// <param name="isHouseholdMemberActivatedAsyncException">Sets the exception which should be thrown by IsHouseholdMemberActivatedAsync.</param>
        /// <param name="hasHouseholdMemberAcceptedPrivacyPolicyAsyncException">Sets the exception which should be thrown by HasHouseholdMemberAcceptedPrivacyPolicyAsync.</param>
        /// <returns>Provider which can append local claims to a claims identity for unit testing.</returns>
        private ILocalClaimProvider CreateLocalClaimProvider(bool isHouseholdMemberCreated = true, bool isHouseholdMemberActivated = true, bool hasHouseholdMemberAcceptedPrivacyPolicy = true, Exception isHouseholdMemberCreatedAsyncException = null, Exception isHouseholdMemberActivatedAsyncException = null, Exception hasHouseholdMemberAcceptedPrivacyPolicyAsyncException = null)
        {
            _householdDataRepositoryMock.Stub(m => m.IsHouseholdMemberCreatedAsync(Arg<IIdentity>.Is.Anything))
                .Return(Task.Run(() =>
                {
                    if (isHouseholdMemberCreatedAsyncException != null)
                    {
                        throw isHouseholdMemberCreatedAsyncException;
                    }

                    return isHouseholdMemberCreated;
                }))
                .Repeat.Any();

            _householdDataRepositoryMock.Stub(m => m.IsHouseholdMemberActivatedAsync(Arg<IIdentity>.Is.Anything))
                .Return(Task.Run(() =>
                {
                    if (isHouseholdMemberActivatedAsyncException != null)
                    {
                        throw isHouseholdMemberActivatedAsyncException;
                    }

                    return isHouseholdMemberActivated;
                }))
                .Repeat.Any();

            _householdDataRepositoryMock.Stub(m => m.HasHouseholdMemberAcceptedPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything))
                .Return(Task.Run(() =>
                {
                    if (hasHouseholdMemberAcceptedPrivacyPolicyAsyncException != null)
                    {
                        throw hasHouseholdMemberAcceptedPrivacyPolicyAsyncException;
                    }

                    return hasHouseholdMemberAcceptedPrivacyPolicy;
                }))
                .Repeat.Any();

            return new LocalClaimProvider(_householdDataRepositoryMock);
        }
    }
}