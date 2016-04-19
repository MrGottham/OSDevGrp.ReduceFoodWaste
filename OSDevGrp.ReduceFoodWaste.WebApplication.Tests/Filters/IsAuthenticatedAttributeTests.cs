using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Filters
{
    /// <summary>
    /// Tests the attribute which can insure that the user is authenticated.
    /// </summary>
    [TestFixture]
    public class IsAuthenticatedAttributeTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize an attribute which can insure that the user is authenticated.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeIsAuthenticatedAttribute()
        {
            var isAuthenticatedAttribute = new IsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an ArgumentNullException when the filter context is null.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsArgumentNullExceptionWhenFilterContextIsNull()
        {
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => isAuthenticatedAttribute.OnActionExecuting(null));
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
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(hasUser: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Null);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isAuthenticatedAttribute.OnActionExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithoutIdentity()
        {
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(hasIdentity: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Null);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isAuthenticatedAttribute.OnActionExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithUnauthenticatedIdentity()
        {
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(isAuthenticated: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.False);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isAuthenticatedAttribute.OnActionExecuting(filterContext));
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
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.True);

            isAuthenticatedAttribute.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Tests that OnResultExecuting throws an ArgumentNullException when the filter context is null.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingThrowsArgumentNullExceptionWhenFilterContextIsNull()
        {
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => isAuthenticatedAttribute.OnResultExecuting(null));
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
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(hasUser: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Null);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isAuthenticatedAttribute.OnResultExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnResultExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user without an identity.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithoutIdentity()
        {
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(hasIdentity: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Null);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isAuthenticatedAttribute.OnResultExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnResultExecuting throws an UnauthorizedAccessException when the HTTP context on the filter context does have a user with an unauthenticated identity.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingThrowsUnauthorizedAccessExceptionWhenHttpContextOnFilterContextDoesHaveUserWithUnauthenticatedIdentity()
        {
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext(isAuthenticated: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.False);

            var exception = Assert.Throws<UnauthorizedAccessException>(() => isAuthenticatedAttribute.OnResultExecuting(filterContext));
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
            var isAuthenticatedAttribute = CreateIsAuthenticatedAttribute();
            Assert.That(isAuthenticatedAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity.IsAuthenticated, Is.True);

            isAuthenticatedAttribute.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Creates an attribute which can insure that the user is authenticated for unit testing.
        /// </summary>
        /// <returns>Attribute which can insure that the user is authenticated for unit testing</returns>
        private static IsAuthenticatedAttribute CreateIsAuthenticatedAttribute()
        {
            return new IsAuthenticatedAttribute();
        }
    }
}
