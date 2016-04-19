using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Filters
{
    /// <summary>
    /// Test helper functionality for a filter.
    /// </summary>
    public static class FilterTestHelper
    {
        /// <summary>
        /// Creates an action executing context which can be used for unit testing.
        /// </summary>
        /// <param name="hasUser">Sets whether the controller should have an user.</param>
        /// <param name="hasIdentity">Sets whether the controller should have an user with an identity.</param>
        /// <param name="isAuthenticated">Sets whether the user is authenticated.</param>
        /// <returns>Action executing context which can be used for unit testing.</returns>
        public static ActionExecutingContext CreateActionExecutingContext(bool hasUser = true, bool hasIdentity = true, bool isAuthenticated = true)
        {
            var controllerContext = ControllerTestHelper.CreateControllerContext(hasUser, hasIdentity, isAuthenticated);

            var methodInfo = controllerContext.Controller.GetType().GetMethods().First();
            var reflectedActionDescriptor = new ReflectedActionDescriptor(methodInfo, methodInfo.Name, new ReflectedAsyncControllerDescriptor(controllerContext.Controller.GetType()));

            return new ActionExecutingContext(controllerContext, reflectedActionDescriptor, new Dictionary<string, object>(0));
        }

        /// <summary>
        /// Creates a result executing context which can be used for unit testing.
        /// </summary>
        /// <param name="hasUser">Sets whether the controller should have an user.</param>
        /// <param name="hasIdentity">Sets whether the controller should have an user with an identity.</param>
        /// <param name="isAuthenticated">Sets whether the user is authenticated.</param>
        /// <returns>Result executing context which can be used for unit testing.</returns>
        public static ResultExecutingContext CreateResultExecutingContext(bool hasUser = true, bool hasIdentity = true, bool isAuthenticated = true)
        {
            var controllerContext = ControllerTestHelper.CreateControllerContext(hasUser, hasIdentity, isAuthenticated);

            return new ResultExecutingContext(controllerContext, new ViewResult());
        }
    }
}
