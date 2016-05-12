using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Test helper functionality for a controller.
    /// </summary>
    public static class ControllerTestHelper
    {
        /// <summary>
        /// Private class which can initialize a controller for unit testing.
        /// </summary>
        private class TestController : Controller
        {
        }

        /// <summary>
        /// Creates a controller context which can be used for unit testing.
        /// </summary>
        /// <param name="controller">Controller which should have the controller context.</param>
        /// <param name="hasUser">Sets whether the controller should have an user.</param>
        /// <param name="hasIdentity">Sets whether the controller should have an user with an identity.</param>
        /// <param name="isAuthenticated">Sets whether the user is authenticated.</param>
        /// <param name="principal">Sets the user principal for the controller.</param>
        /// <returns>Controller context which can be used for unit testing.</returns>
        public static ControllerContext CreateControllerContext(Controller controller, bool hasUser = true, bool hasIdentity = true, bool isAuthenticated = true, IPrincipal principal = null)
        {
            if (controller == null)
            {
                throw new ArgumentNullException("controller");
            }

            var identityMock = MockRepository.GenerateMock<IIdentity>();
            identityMock.Stub(m => m.IsAuthenticated)
                .Return(isAuthenticated)
                .Repeat.Any();

            var userMock = MockRepository.GenerateMock<IPrincipal>();
            userMock.Stub(m => m.Identity)
                .Return(hasIdentity ? identityMock : null)
                .Repeat.Any();

            var httpContextMock = MockRepository.GenerateMock<HttpContextBase>();
            httpContextMock.Stub(m => m.User)
                .Return(hasUser ? (principal ?? userMock) : null)
                .Repeat.Any();

            return new ControllerContext(httpContextMock, new RouteData(), controller);
        }

        /// <summary>
        /// Creates a controller context which can be used for unit testing.
        /// </summary>
        /// <param name="hasUser">Sets whether the controller should have an user.</param>
        /// <param name="hasIdentity">Sets whether the controller should have an user with an identity.</param>
        /// <param name="isAuthenticated">Sets whether the user is authenticated.</param>
        /// <param name="principal">Sets the user principal for the controller.</param>
        /// <returns>Controller context which can be used for unit testing.</returns>
        public static ControllerContext CreateControllerContext(bool hasUser = true, bool hasIdentity = true, bool isAuthenticated = true, IPrincipal principal = null)
        {
            return CreateControllerContext(new TestController(), hasUser, hasIdentity, isAuthenticated, principal);
        }
    }
}
