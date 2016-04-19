using System;
using System.Web.Mvc;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Filters
{
    /// <summary>
    /// Attribute which can insure that the user is a validated household member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IsValidatedHouseholdMemberAttribute : ActionFilterAttribute
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        #endregion
    }
}