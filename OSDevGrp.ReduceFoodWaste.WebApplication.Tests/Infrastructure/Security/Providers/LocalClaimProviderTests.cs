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
            var localClaimProvider = new LocalClaimProvider(_householdDataRepositoryMock);
            Assert.That(localClaimProvider, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCredentialsProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new LocalClaimProvider(null));
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
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localClaimProvider.AddLocalClaimsAsync(null));
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
        public void TestThatAddLocalClaimsAsyncCallsIsHouseholdMemberCreatedAsyncOnHouseholdDataRepository()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);
            
            task.Wait();

            _householdDataRepositoryMock.AssertWasCalled(m => m.IsHouseholdMemberCreatedAsync(Arg<IIdentity>.Is.Equal(claimsIdentity)));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when IsHouseholdMemberCreatedAsync on the repository which can access household data thows an ReduceFoodWasteExceptionBase.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenIsHouseholdMemberCreatedAsyncOnHouseholdDataRepositoryThrowsReduceFoodWasteExceptionBase()
        {
            var exceptionToThrow = Fixture.Create<ReduceFoodWasteBusinessException>();

            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreatedAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            Assert.Throws<AggregateException>(task.Wait).Handle(exception =>
            {
                Assert.That(exception, Is.Not.Null);
                Assert.That(exception, Is.EqualTo(exceptionToThrow));
                return true;
            });
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when IsHouseholdMemberCreatedAsync on the repository which can access household data thows an Exception.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenIsHouseholdMemberCreatedAsyncOnHouseholdDataRepositoryThrowsException()
        {
            var exceptionToThrow = Fixture.Create<Exception>();

            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreatedAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            Assert.Throws<AggregateException>(task.Wait).Handle(exception =>
            {
                Assert.That(exception, Is.Not.Null);
                Assert.That(exception, Is.TypeOf<ReduceFoodWasteSystemException>());
                Assert.That(exception.Message, Is.Not.Null);
                Assert.That(exception.Message, Is.Not.Empty);
                Assert.That(exception.Message, Is.EqualTo(exceptionToThrow.Message));
                Assert.That(exception.InnerException, Is.Not.Null);
                Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
                return true;
            });
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync adds claim for the household member has been created when a household member has been created for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncAddsClaimForCreatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember);
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
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForCreatedHouseholdMemberWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.CreatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been activated when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForActivatedHouseholdMemberWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has accepted privacy policies when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForPrivacyPoliciesAcceptedWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been validated when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not call IsHouseholdMemberActivatedAsync on the repository which can access household data when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncDoesNotCallIsHouseholdMemberActivatedAsyncOnHouseholdDataRepositoryWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.IsHouseholdMemberActivatedAsync(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not call HasHouseholdMemberAcceptedPrivacyPolicyAsync on the repository which can access household data when a household member has not been created for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncDoesNotCallHasHouseholdMemberAcceptedPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberForIdentityHasNotBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberCreated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.HasHouseholdMemberAcceptedPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync calls IsHouseholdMemberActivatedAsync on the repository which can access household data when a household member has been created for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncCallsIsHouseholdMemberActivatedAsyncOnHouseholdDataRepositoryWhenHouseholdMemberForIdentityHasBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            _householdDataRepositoryMock.AssertWasCalled(m => m.IsHouseholdMemberActivatedAsync(Arg<IIdentity>.Is.Equal(claimsIdentity)));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when IsHouseholdMemberActivatedAsync on the repository which can access household data thows an ReduceFoodWasteExceptionBase.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenIsHouseholdMemberActivatedAsyncOnHouseholdDataRepositoryThrowsReduceFoodWasteExceptionBase()
        {
            var exceptionToThrow = Fixture.Create<ReduceFoodWasteBusinessException>();

            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivatedAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            Assert.Throws<AggregateException>(task.Wait).Handle(exception =>
            {
                Assert.That(exception, Is.Not.Null);
                Assert.That(exception, Is.EqualTo(exceptionToThrow));
                return true;
            });
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when IsHouseholdMemberActivatedAsync on the repository which can access household data thows an Exception.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenIsHouseholdMemberActivatedAsyncOnHouseholdDataRepositoryThrowsException()
        {
            var exceptionToThrow = Fixture.Create<Exception>();

            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivatedAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            Assert.Throws<AggregateException>(task.Wait).Handle(exception =>
            {
                Assert.That(exception, Is.Not.Null);
                Assert.That(exception, Is.TypeOf<ReduceFoodWasteSystemException>());
                Assert.That(exception.Message, Is.Not.Null);
                Assert.That(exception.Message, Is.Not.Empty);
                Assert.That(exception.Message, Is.EqualTo(exceptionToThrow.Message));
                Assert.That(exception.InnerException, Is.Not.Null);
                Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
                return true;
            });
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync calls HasHouseholdMemberAcceptedPrivacyPolicyAsync on the repository which can access household data when a household member has been created for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncCallsHasHouseholdMemberAcceptedPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberForIdentityHasBeenCreated()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            _householdDataRepositoryMock.AssertWasCalled(m => m.HasHouseholdMemberAcceptedPrivacyPolicyAsync(Arg<IIdentity>.Is.Equal(claimsIdentity)));
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when HasHouseholdMemberAcceptedPrivacyPolicyAsync on the repository which can access household data thows an ReduceFoodWasteExceptionBase.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenHasHouseholdMemberAcceptedPrivacyPolicyAsyncOnHouseholdDataRepositoryThrowsReduceFoodWasteExceptionBase()
        {
            var exceptionToThrow = Fixture.Create<ReduceFoodWasteBusinessException>();

            var localClaimProvider = CreateLocalClaimProvider(hasHouseholdMemberAcceptedPrivacyPolicyAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            Assert.Throws<AggregateException>(task.Wait).Handle(exception =>
            {
                Assert.That(exception, Is.Not.Null);
                Assert.That(exception, Is.EqualTo(exceptionToThrow));
                return true;
            });
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync fails when HasHouseholdMemberAcceptedPrivacyPolicyAsync on the repository which can access household data thows an Exception.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncFailsWhenHasHouseholdMemberAcceptedPrivacyPolicyAsyncOnHouseholdDataRepositoryThrowsException()
        {
            var exceptionToThrow = Fixture.Create<Exception>();

            var localClaimProvider = CreateLocalClaimProvider(hasHouseholdMemberAcceptedPrivacyPolicyAsyncException: exceptionToThrow);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            Assert.Throws<AggregateException>(task.Wait).Handle(exception =>
            {
                Assert.That(exception, Is.Not.Null);
                Assert.That(exception, Is.TypeOf<ReduceFoodWasteSystemException>());
                Assert.That(exception.Message, Is.Not.Null);
                Assert.That(exception.Message, Is.Not.Empty);
                Assert.That(exception.Message, Is.EqualTo(exceptionToThrow.Message));
                Assert.That(exception.InnerException, Is.Not.Null);
                Assert.That(exception.InnerException, Is.EqualTo(exceptionToThrow));
                return true;
            });
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync adds claim for the household member has been activated when a household member has been created and activated for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncAddsClaimForActivatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedAndActivated()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember);
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
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForActivatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedButNotActivated()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.ActivatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync adds claim for the household member has accepted privacy policies when a household member has been created and accepted privacy policies for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncAddsClaimForPrivacyPoliciesAcceptedWhenHouseholdMemberForIdentityHasBeenCreatedAndAcceptedPrivacyPolicies()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted);
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
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForPrivacyPoliciesAcceptedWhenHouseholdMemberForIdentityHasBeenCreatedButNotAcceptedPrivacyPolicies()
        {
            var localClaimProvider = CreateLocalClaimProvider(hasHouseholdMemberAcceptedPrivacyPolicy: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();
            var claim = claimsIdentity.FindFirst(LocalClaimTypes.PrivacyPoliciesAccepted);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync adds claim for the household member has been validated when a household member has been created, has been activated and accepted privacy policies for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncAddsClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedHasBeenActivatedAndAcceptedPrivacyPolicies()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
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
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedButNotActivatedAndNotAcceptedPrivacyPolicies()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivated: false, hasHouseholdMemberAcceptedPrivacyPolicy: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been validated when a household member has been created and accepted privacy policies but not activated for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedHasAcceptedPrivacyPoliciesButNotActivated()
        {
            var localClaimProvider = CreateLocalClaimProvider(isHouseholdMemberActivated: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimsAsync does not add claim for the household member has been validated when a household member has been created have been activated but not accepted privacy policies for the identity.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimsAsyncDoesNotAddClaimForValidatedHouseholdMemberWhenHouseholdMemberForIdentityHasBeenCreatedHaveBeenActivatedButNotAcceptedPrivacyPolicies()
        {
            var localClaimProvider = CreateLocalClaimProvider(hasHouseholdMemberAcceptedPrivacyPolicy: false);
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember), Is.Null);

            var task = localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(LocalClaimTypes.ValidatedHouseholdMember);
            Assert.That(claim, Is.Null);
        }

        /// <summary>
        /// Tests that AddLocalClaimAsync throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatAddLocalClaimAsyncThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localClaimProvider.AddLocalClaimAsync(null, new Claim(Fixture.Create<string>(), Fixture.Create<string>())));
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
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => localClaimProvider.AddLocalClaimAsync(new ClaimsIdentity(), null));
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
        public void TestThatAddLocalClaimAsyncAddsLocalClaimToClaimsIdentity()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claimToAdd = new Claim(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(claimToAdd, Is.Not.Null);
            Assert.That(claimToAdd.Type, Is.Not.Null);
            Assert.That(claimToAdd.Type, Is.Not.Empty);
            Assert.That(claimToAdd.Value, Is.Not.Null);
            Assert.That(claimToAdd.Value, Is.Not.Empty);

            var claimsIdentity = new ClaimsIdentity();
            Assert.That(claimsIdentity, Is.Not.Null);
            Assert.That(claimsIdentity.FindFirst(claimToAdd.Type), Is.Null);

            var task = localClaimProvider.AddLocalClaimAsync(claimsIdentity, claimToAdd);
            Assert.That(task, Is.Not.Null);

            task.Wait();

            var claim = claimsIdentity.FindFirst(claimToAdd.Type);
            Assert.That(claim, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Null);
            Assert.That(claim.Type, Is.Not.Empty);
            Assert.That(claim.Type, Is.EqualTo(claimToAdd.Type));
            Assert.That(claim.Value, Is.Not.Null);
            Assert.That(claim.Value, Is.Not.Empty);
            Assert.That(claim.Value, Is.EqualTo(claimToAdd.Value));
        }

        /// <summary>
        /// Tests that GenerateCreatedHouseholdMemberClaim generates a claim which indicates that a cliam identity has been created as a household member.
        /// </summary>
        [Test]
        public void TestThatGenerateCreatedHouseholdMemberClaimGeneratesClaim()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claim = localClaimProvider.GenerateCreatedHouseholdMemberClaim();
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
        /// Tests that GenerateActivatedHouseholdMemberClaim generates a claim which indicates that a cliam identity is an activated household member.
        /// </summary>
        [Test]
        public void TestThatGenerateActivatedHouseholdMemberClaimGeneratesClaim()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claim = localClaimProvider.GenerateActivatedHouseholdMemberClaim();
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
        /// Tests that GeneratePrivacyPoliciesAcceptedClaim generates a claim which indicates that a cliam identity has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatGeneratePrivacyPoliciesAcceptedClaimGeneratesClaim()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claim = localClaimProvider.GeneratePrivacyPoliciesAcceptedClaim();
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
        /// Tests that GeneratePrivacyPoliciesAcceptedClaim generates a claim which indicates that a cliam identity has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatGenerateValidatedHouseholdMemberClaimGeneratesClaim()
        {
            var localClaimProvider = CreateLocalClaimProvider();
            Assert.That(localClaimProvider, Is.Not.Null);

            var claim = localClaimProvider.GenerateValidatedHouseholdMemberClaim();
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
        /// <param name="isHouseholdMemberCreatedAsyncException">Sets the exception which should be throwed by IsHouseholdMemberCreatedAsync.</param>
        /// <param name="isHouseholdMemberActivatedAsyncException">Sets the exception which should be throwed by IsHouseholdMemberActivatedAsync.</param>
        /// <param name="hasHouseholdMemberAcceptedPrivacyPolicyAsyncException">Sets the exception which should be throwed by HasHouseholdMemberAcceptedPrivacyPolicyAsync.</param>
        /// <returns>Provider which can append local claims to a claims identity for unit testing.</returns>
        private ILocalClaimProvider CreateLocalClaimProvider(bool isHouseholdMemberCreated = true, bool isHouseholdMemberActivated = true, bool hasHouseholdMemberAcceptedPrivacyPolicy = true, Exception isHouseholdMemberCreatedAsyncException = null, Exception isHouseholdMemberActivatedAsyncException = null, Exception hasHouseholdMemberAcceptedPrivacyPolicyAsyncException = null)
        {
            Func<bool> isHouseholdMemberCreatedFunc = () =>
            {
                if (isHouseholdMemberCreatedAsyncException != null)
                {
                    throw isHouseholdMemberCreatedAsyncException;
                }
                return isHouseholdMemberCreated;
            };
            _householdDataRepositoryMock.Stub(m => m.IsHouseholdMemberCreatedAsync(Arg<IIdentity>.Is.Anything))
                .Return(Task.Run(isHouseholdMemberCreatedFunc))
                .Repeat.Any();

            Func<bool> isHouseholdMemberActivatedFunc = () =>
            {
                if (isHouseholdMemberActivatedAsyncException != null)
                {
                    throw isHouseholdMemberActivatedAsyncException;
                }
                return isHouseholdMemberActivated;
            };
            _householdDataRepositoryMock.Stub(m => m.IsHouseholdMemberActivatedAsync(Arg<IIdentity>.Is.Anything))
                .Return(Task.Run(isHouseholdMemberActivatedFunc))
                .Repeat.Any();

            Func<bool> hasHouseholdMemberAcceptedPrivacyPolicyFunc = () =>
            {
                if (hasHouseholdMemberAcceptedPrivacyPolicyAsyncException != null)
                {
                    throw hasHouseholdMemberAcceptedPrivacyPolicyAsyncException;
                }
                return hasHouseholdMemberAcceptedPrivacyPolicy;
            };
            _householdDataRepositoryMock.Stub(m => m.HasHouseholdMemberAcceptedPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything))
                .Return(Task.Run(hasHouseholdMemberAcceptedPrivacyPolicyFunc))
                .Repeat.Any();

            return new LocalClaimProvider(_householdDataRepositoryMock);
        }
    }
}
