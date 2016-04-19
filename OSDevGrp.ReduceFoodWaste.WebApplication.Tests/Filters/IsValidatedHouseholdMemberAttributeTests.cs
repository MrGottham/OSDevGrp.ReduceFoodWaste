using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
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

        /// <summary>
        /// Creates an action executing context which can be used in unit tests.
        /// </summary>
        /// <returns>Action executing context which can be used in unit tests.</returns>
        private static ActionExecutingContext CreateActionExecutingContext()
        {
            return new ActionExecutingContext(null, null, null);
        }

        /// <summary>
        /// Creates a result executing context which can be used in unit tests.
        /// </summary>
        /// <returns>Result executing context which can be used in unit tests.</returns>
        private static ResultExecutingContext CreateResultExecutingContext()
        {
            return new ResultExecutingContext(null, null);
        }

        /// <summary>
        /// Creates a controller context which can be used in unit tests.
        /// </summary>
        /// <returns>Controller context which can be used in unit tests.</returns>
        private static ControllerContext CreateControllerContext()
        {
            var identityMock = MockRepository.GenerateMock<IIdentity>();
            identityMock.Stub(m => m.IsAuthenticated)
                .Return(true)
                .Repeat.Any();

            var userMock = MockRepository.GenerateMock<IPrincipal>();
            userMock.Stub(m => m.Identity)
                .Return(identityMock)
                .Repeat.Any();

            var httpContextMock = MockRepository.GenerateMock<HttpContextBase>();
            httpContextMock.Stub(m => m.User)
                .Return(userMock)
                .Repeat.Any();

            return new ControllerContext(httpContextMock, new RouteData(), null);

        }
    }
}
