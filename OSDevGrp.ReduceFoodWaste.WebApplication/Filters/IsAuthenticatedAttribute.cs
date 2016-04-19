using System;
using System.Security.Principal;
using System.Web.Mvc;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Filters
{
    /// <summary>
    /// Attribute which can insure that the user is authenticated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IsAuthenticatedAttribute : ActionFilterAttribute
    {
        #region Methods

        /// <summary>
        /// Validates whether the user is a validated household member for each action.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;
            if (httpContext == null || httpContext.User == null || httpContext.User.Identity == null || httpContext.User.Identity.IsAuthenticated == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (ValidateIdentity(httpContext.User.Identity) == false)
            {
                throw new UnauthorizedAccessException();
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Validated whether the user is a validated houehold member for each result.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;
            if (httpContext == null || httpContext.User == null || httpContext.User.Identity == null || httpContext.User.Identity.IsAuthenticated == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (ValidateIdentity(httpContext.User.Identity) == false)
            {
                throw new UnauthorizedAccessException();
            }

            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Validates whether a given identity meets the requirements for authentication.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the given identity meets the requirements for authentication otherwise false.</returns>
        protected virtual bool ValidateIdentity(IIdentity identity)
        {
            return identity != null;
        }

        #endregion
    }
}