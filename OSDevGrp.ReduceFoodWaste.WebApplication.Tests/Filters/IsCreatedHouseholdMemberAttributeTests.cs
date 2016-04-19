using System;
using System.Security.Principal;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Filters
{
    /// <summary>
    /// Tests the attribute which can insure that the user is a created household member.
    /// </summary>
    [TestFixture]
    public class IsCreatedHouseholdMemberAttributeTests : TestBase
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
        /// Tests that the constructor initialize an attribute which can insure that the user is a created household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeIsCreatedHouseholdMemberAttribute()
        {
            var isCreatedHouseholdMemberAttribute = new IsCreatedHouseholdMemberAttribute();
            Assert.That(isCreatedHouseholdMemberAttribute, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can get values from claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new IsCreatedHouseholdMemberAttribute(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an ArgumentNullException when the filter context is null.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsArgumentNullExceptionWhenFilterContextIsNull()
        {
            var isCreatedHouseholdMemberAttribute = CreateIsCreatedHouseholdMemberAttribute();
            Assert.That(isCreatedHouseholdMemberAttribute, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => isCreatedHouseholdMemberAttribute.OnActionExecuting(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("filterContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does not have an user.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesNotHaveUser()
        {
            var isCreatedHouseholdMemberAttribute = CreateIsCreatedHouseholdMemberAttribute();
            Assert.That(isCreatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(hasUser: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Null);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isCreatedHouseholdMemberAttribute.OnActionExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting does not call IsCreatedHouseholdMember on the provider which can get values from claims when the HTTP context on the filter context does not have an user.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingDoesNotCallIsCreatedHouseholdMemberMemberOnClaimValueProviderWhenHttpContextOnFilterContextDoesNotHaveUser()
        {
            var isCreatedHouseholdMemberAttribute = CreateIsCreatedHouseholdMemberAttribute();
            Assert.That(isCreatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(hasUser: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Null);

            Assert.Throws<UnauthorizedAccessException>(() => isCreatedHouseholdMemberAttribute.OnActionExecuting(filterContext));

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsCreatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithoutIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(hasIdentity: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Null);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting does not call IsValidatedHouseholdMember on the provider which can get values from claims when the HTTP context on the filter context does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingDoesNotCallIsValidatedHouseholdMemberOnClaimValueProviderWhenHttpContextOnFilterContextDoesHaveUserWithoutIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(hasIdentity: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Null);

            Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext));

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithUnauthenticatedIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(isAuthenticated: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.False);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting does not call IsValidatedHouseholdMember on the provider which can get values from claims when the HTTP context on the filter context does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingDoesNotCallIsValidatedHouseholdMemberOnClaimValueProviderWhenHttpContextOnFilterContextDoesHaveUserWithUnauthenticatedIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(isAuthenticated: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.False);

            Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext));

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that OnActionExecuting calls IsValidatedHouseholdMember on the provider which can get values from claims when the HTTP context on the filter context does have a user with an authenticated identity.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingCallsIsValidatedHouseholdMemberOnClaimValueProviderWhenHttpContextOnFilterContextDoesHaveUserWithAuthenticatedIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.True);

            isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext);

            _claimValueProviderMock.AssertWasCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Equal(filterContext.HttpContext.User.Identity)));
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user with an authenticated identity who are not a validated household member.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithAuthenticatedIdentityWhoAreNotValidatedHouseholdMember()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute(isValidatedHouseholdMember: false);
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.True);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting does not throw an Exception when the HTTP context on the filter context does have a user with an authenticated identity who are a validated household member.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingDoesNotThrowExceptionWhenHttpContextOnFilterContextDoesHaveUserWithAuthenticatedIdentityWhoAreValidatedHouseholdMember()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.True);

            isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext);
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
        /// Tests that OnResultExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does not have an user.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesNotHaveUser()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(hasUser: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Null);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnResultExecuting does not call IsValidatedHouseholdMember on the provider which can get values from claims when the HTTP context on the filter context does not have an user.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingDoesNotCallIsValidatedHouseholdMemberOnClaimValueProviderWhenHttpContextOnFilterContextDoesNotHaveUser()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(hasUser: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Null);

            Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext));

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that OnResultExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithoutIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(hasIdentity: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Null);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnResultExecuting does not call IsValidatedHouseholdMember on the provider which can get values from claims when the HTTP context on the filter context does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingDoesNotCallIsValidatedHouseholdMemberOnClaimValueProviderWhenHttpContextOnFilterContextDoesHaveUserWithoutIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(hasIdentity: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Null);

            Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext));

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that OnResultExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithUnauthenticatedIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(isAuthenticated: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.False);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnResultExecuting does not call IsValidatedHouseholdMember on the provider which can get values from claims when the HTTP context on the filter context does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingDoesNotCallIsValidatedHouseholdMemberOnClaimValueProviderWhenHttpContextOnFilterContextDoesHaveUserWithUnauthenticatedIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(isAuthenticated: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.False);

            Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext));

            _claimValueProviderMock.AssertWasNotCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything));
        }

        /// <summary>
        /// Tests that OnResultExecuting calls IsValidatedHouseholdMember on the provider which can get values from claims when the HTTP context on the filter context does have a user with an authenticated identity.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingCallsIsValidatedHouseholdMemberOnClaimValueProviderWhenHttpContextOnFilterContextDoesHaveUserWithAuthenticatedIdentity()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.True);

            isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext);

            _claimValueProviderMock.AssertWasCalled(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Equal(filterContext.HttpContext.User.Identity)));
        }

        /// <summary>
        /// Tests that OnResultExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user with an authenticated identity who are not a validated household member.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithAuthenticatedIdentityWhoAreNotValidatedHouseholdMember()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute(isValidatedHouseholdMember: false);
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.True);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnResultExecuting does not throw an Exception when the HTTP context on the filter context does have a user with an authenticated identity who are a validated household member.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingDoesNotThrowExceptionWhenHttpContextOnFilterContextDoesHaveUserWithAuthenticatedIdentityWhoAreValidatedHouseholdMember()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.True);

            isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Creates an attribute which can insure that the user is a created household member for unit testing.
        /// </summary>
        /// <param name="isCreatedHouseholdMember">Sets whether the user is a created household member.</param>
        /// <returns>Attribute which can insure that the user is a created household member.</returns>
        private IsCreatedHouseholdMemberAttribute CreateIsCreatedHouseholdMemberAttribute(bool isCreatedHouseholdMember = true)
        {
            _claimValueProviderMock.Stub(m => m.IsCreatedHouseholdMember(Arg<IIdentity>.Is.Anything))
                .Return(isCreatedHouseholdMember)
                .Repeat.Any();

            return new IsCreatedHouseholdMemberAttribute(_claimValueProviderMock);
        }
    }
}
