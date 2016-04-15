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
        /// Tests that IsValidatedHouseholdMember when called with a identity throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsValidatedHouseholdMemberWhenCalledWithIdentityThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsValidatedHouseholdMember((IIdentity) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsValidatedHouseholdMember when called with a claims identity throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatIsValidatedHouseholdMemberWhenCalledWithClaimsIdentityThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsValidatedHouseholdMember(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsValidatedHouseholdMember when called with a claims identity returns false when the claim collection is empty.
        /// </summary>
        [Test]
        public void TestThatIsValidatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsFalseWhenClaimCollectionIsEmpty()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>(0);
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsValidatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsValidatedHouseholdMember when called with a claims identity returns false when a claim for validated household member does not exist.
        /// </summary>
        [Test]
        public void TestThatIsValidatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsFalseWhenClaimForValidatedHouseholdMemberDoesNotExist()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsValidatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsValidatedHouseholdMember when called with a claims identity returns the value from the claim for validated household member when the claim does exist.
        /// </summary>
        [Test]
        [TestCase(true, LocalClaimProvider.LocalClaimIssuer, LocalClaimProvider.LocalClaimIssuer, true)]
        [TestCase(false, LocalClaimProvider.LocalClaimIssuer, LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(true, "XYZ", LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(false, "XYZ", LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(true, LocalClaimProvider.LocalClaimIssuer, "XYZ", false)]
        [TestCase(false, LocalClaimProvider.LocalClaimIssuer, "XYZ", false)]
        [TestCase(true, null, null, false)]
        [TestCase(false, null, null, false)]
        [TestCase(true, "", "", false)]
        [TestCase(false, "", "", false)]
        [TestCase(true, "XYZ", "XYZ", false)]
        [TestCase(false, "XYZ", "XYZ", false)]
        public void TestThatIsValidatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsValueFromClaimForValidatedHouseholdMemberWhenClaimDoesExist(bool claimValue, string claimIssuer, string claimOriginalIssuer, bool expectedValue)
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(LocalClaimTypes.ValidatedHouseholdMember, Convert.ToString(claimValue), ClaimValueTypes.Boolean, claimIssuer, claimOriginalIssuer)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsValidatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that IsValidatedHouseholdMember when called with a identity throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsCreatedHouseholdMemberWhenCalledWithIdentityThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsCreatedHouseholdMember((IIdentity) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsCreatedHouseholdMember when called with a claims identity throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatIsCreatedHouseholdMemberWhenCalledWithClaimsIdentityThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsCreatedHouseholdMember(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsCreatedHouseholdMember when called with a claims identity returns false when the claim collection is empty.
        /// </summary>
        [Test]
        public void TestThatIsCreatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsFalseWhenClaimCollectionIsEmpty()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>(0);
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsCreatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsCreatedHouseholdMember when called with a claims identity returns false when a claim for created household member does not exist.
        /// </summary>
        [Test]
        public void TestThatIsCreatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsFalseWhenClaimForCreatedHouseholdMemberDoesNotExist()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsCreatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsCreatedHouseholdMember when called with a claims identity returns the value from the claim for created household member when the claim does exist.
        /// </summary>
        [Test]
        [TestCase(true, LocalClaimProvider.LocalClaimIssuer, LocalClaimProvider.LocalClaimIssuer, true)]
        [TestCase(false, LocalClaimProvider.LocalClaimIssuer, LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(true, "XYZ", LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(false, "XYZ", LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(true, LocalClaimProvider.LocalClaimIssuer, "XYZ", false)]
        [TestCase(false, LocalClaimProvider.LocalClaimIssuer, "XYZ", false)]
        [TestCase(true, null, null, false)]
        [TestCase(false, null, null, false)]
        [TestCase(true, "", "", false)]
        [TestCase(false, "", "", false)]
        [TestCase(true, "XYZ", "XYZ", false)]
        [TestCase(false, "XYZ", "XYZ", false)]
        public void TestThatIsCreatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsValueFromClaimForCreatedHouseholdMemberWhenClaimDoesExist(bool claimValue, string claimIssuer, string claimOriginalIssuer, bool expectedValue)
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(LocalClaimTypes.CreatedHouseholdMember, Convert.ToString(claimValue), ClaimValueTypes.Boolean, claimIssuer, claimOriginalIssuer)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsCreatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that IsActivatedHouseholdMember when called with a identity throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsActivatedHouseholdMemberWhenCalledWithIdentityThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception =Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsActivatedHouseholdMember((IIdentity) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsActivatedHouseholdMember when called with a claims identity throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatIsActivatedHouseholdMemberWhenCalledWithClaimsIdentityThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsActivatedHouseholdMember(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsActivatedHouseholdMember when called with a claims identity returns false when the claim collection is empty.
        /// </summary>
        [Test]
        public void TestThatIsActivatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsFalseWhenClaimCollectionIsEmpty()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>(0);
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsActivatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsActivatedHouseholdMember when called with a claims identity returns false when a claim for activated household member does not exist.
        /// </summary>
        [Test]
        public void TestThatIsActivatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsFalseWhenClaimForActivatedHouseholdMemberDoesNotExist()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsActivatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsActivatedHouseholdMember when called with a claims identity returns the value from the claim for activated household member when the claim does exist.
        /// </summary>
        [Test]
        [TestCase(true, LocalClaimProvider.LocalClaimIssuer, LocalClaimProvider.LocalClaimIssuer, true)]
        [TestCase(false, LocalClaimProvider.LocalClaimIssuer, LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(true, "XYZ", LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(false, "XYZ", LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(true, LocalClaimProvider.LocalClaimIssuer, "XYZ", false)]
        [TestCase(false, LocalClaimProvider.LocalClaimIssuer, "XYZ", false)]
        [TestCase(true, null, null, false)]
        [TestCase(false, null, null, false)]
        [TestCase(true, "", "", false)]
        [TestCase(false, "", "", false)]
        [TestCase(true, "XYZ", "XYZ", false)]
        [TestCase(false, "XYZ", "XYZ", false)]
        public void TestThatIsActivatedHouseholdMemberWhenCalledWithClaimsIdentityReturnsValueFromClaimForActivatedHouseholdMemberWhenClaimDoesExist(bool claimValue, string claimIssuer, string claimOriginalIssuer, bool expectedValue)
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(LocalClaimTypes.ActivatedHouseholdMember, Convert.ToString(claimValue), ClaimValueTypes.Boolean, claimIssuer, claimOriginalIssuer)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsActivatedHouseholdMember(claimsIdentity);
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Tests that IsPrivacyPoliciesAccepted when called with a identity throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPoliciesAcceptedWhenCalledWithIdentityThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsPrivacyPoliciesAccepted((IIdentity) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsPrivacyPoliciesAccepted when called with a claims identity throws an ArgumentNullException when the claims identity is null.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPoliciesAcceptedWhenCalledWithClaimsIdentityThrowsArgumentNullExceptionWhenClaimsIdentityIsNull()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => claimValueProvider.IsPrivacyPoliciesAccepted(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimsIdentity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsPrivacyPoliciesAccepted when called with a claims identity returns false when the claim collection is empty.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPoliciesAcceptedWhenCalledWithClaimsIdentityReturnsFalseWhenClaimCollectionIsEmpty()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>(0);
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsPrivacyPoliciesAccepted(claimsIdentity);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsPrivacyPoliciesAccepted when called with a claims identity returns false when a claim for privacy policies accepted does not exist.
        /// </summary>
        [Test]
        public void TestThatIsPrivacyPoliciesAcceptedWhenCalledWithClaimsIdentityReturnsFalseWhenClaimForPrivacyPoliciesAcceptedDoesNotExist()
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsPrivacyPoliciesAccepted(claimsIdentity);
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that IsPrivacyPoliciesAccepted when called with a claims identity returns the value from the claim for privacy policies accepted when the claim does exist.
        /// </summary>
        [Test]
        [TestCase(true, LocalClaimProvider.LocalClaimIssuer, LocalClaimProvider.LocalClaimIssuer, true)]
        [TestCase(false, LocalClaimProvider.LocalClaimIssuer, LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(true, "XYZ", LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(false, "XYZ", LocalClaimProvider.LocalClaimIssuer, false)]
        [TestCase(true, LocalClaimProvider.LocalClaimIssuer, "XYZ", false)]
        [TestCase(false, LocalClaimProvider.LocalClaimIssuer, "XYZ", false)]
        [TestCase(true, null, null, false)]
        [TestCase(false, null, null, false)]
        [TestCase(true, "", "", false)]
        [TestCase(false, "", "", false)]
        [TestCase(true, "XYZ", "XYZ", false)]
        [TestCase(false, "XYZ", "XYZ", false)]
        public void TestThatIsPrivacyPoliciesAcceptedWhenCalledWithClaimsIdentityReturnsValueFromClaimForPrivacyPoliciesAcceptedWhenClaimDoesExist(bool claimValue, string claimIssuer, string claimOriginalIssuer, bool expectedValue)
        {
            var claimValueProvider = new ClaimValueProvider();
            Assert.That(claimValueProvider, Is.Not.Null);

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, Fixture.Create<string>(), ClaimValueTypes.String),
                new Claim(LocalClaimTypes.PrivacyPoliciesAccepted, Convert.ToString(claimValue), ClaimValueTypes.Boolean, claimIssuer, claimOriginalIssuer)
            };
            var claimsIdentity = new ClaimsIdentity(claimCollection);

            var result = claimValueProvider.IsPrivacyPoliciesAccepted(claimsIdentity);
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
