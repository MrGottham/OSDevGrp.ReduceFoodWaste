using System;
using System.Security.Authentication;
using System.Security.Principal;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Filters
{
    /// <summary>
    /// Tests the attribute which can insure that the user is a validated household member.
    /// </summary>
    [TestFixture]
    public class IsValidatedHouseholdMemberAttributeTests : TestBase
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
        /// Tests that the constructor initialize an attribute which can insure that the user is a validated household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeIsValidatedHouseholdMemberAttribute()
        {
            var isValidatedHouseholdMemberAttribute = new IsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can get values from claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new IsValidatedHouseholdMemberAttribute(null));
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
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => isValidatedHouseholdMemberAttribute.OnActionExecuting(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("filterContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnActionExecuting throws an X when the HTTP context on the filter context does not have an user.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingThrowsXWhenHttpContextOnFilterContextDoesNotHaveUser()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext(hasUser: false);
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Null);

            var exception = Assert.Throws<AuthenticationException>(() => isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo("XYZ"));
            Assert.That(exception.InnerException, Is.Null);
        }




        /// <summary>
        /// Tests that OnActionExecuting calls IsActivatedHouseholdMember on the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatOnActionExecutingCallsIsActivatedHouseholdMemberOnClaimValueProvide()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateActionExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);

            isValidatedHouseholdMemberAttribute.OnActionExecuting(filterContext);

            _claimValueProviderMock.AssertWasCalled(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Equal(filterContext.HttpContext.User.Identity)));
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
        /// Tests that OnResultExecuting calls IsActivatedHouseholdMember on the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatOnResultExecutingCallsIsActivatedHouseholdMemberOnClaimValueProvide()
        {
            var isValidatedHouseholdMemberAttribute = CreateIsValidatedHouseholdMemberAttribute();
            Assert.That(isValidatedHouseholdMemberAttribute, Is.Not.Null);

            var filterContext = FilterTestHelper.CreateResultExecutingContext();
            Assert.That(filterContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User, Is.Not.Null);
            Assert.That(filterContext.HttpContext.User.Identity, Is.Not.Null);

            isValidatedHouseholdMemberAttribute.OnResultExecuting(filterContext);

            _claimValueProviderMock.AssertWasCalled(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Equal(filterContext.HttpContext.User.Identity)));
        }

        /// <summary>
        /// Creates an attribute which can insure that the user is a validated household member for unit testing.
        /// </summary>
        /// <param name="isValidatedHouseholdMember">Sets whether the user is a validated household member.</param>
        /// <returns>Attribute which can insure that the user is a validated household member for unit testing</returns>
        private IsValidatedHouseholdMemberAttribute CreateIsValidatedHouseholdMemberAttribute(bool isValidatedHouseholdMember = true)
        {
            _claimValueProviderMock.Stub(m => m.IsValidatedHouseholdMember(Arg<IIdentity>.Is.Anything))
                .Return(isValidatedHouseholdMember)
                .Repeat.Any();

            return new IsValidatedHouseholdMemberAttribute(_claimValueProviderMock);
        }
    }
}
