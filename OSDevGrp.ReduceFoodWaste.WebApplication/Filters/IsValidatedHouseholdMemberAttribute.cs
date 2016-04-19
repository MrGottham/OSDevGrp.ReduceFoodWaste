using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Filters
{
    /// <summary>
    /// Attribute which can insure that the user is a validated household member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IsValidatedHouseholdMemberAttribute : ActionFilterAttribute
    {
        #region Private variables

        private readonly IClaimValueProvider _claimValueProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an attribute which can insure that the user is a validated household member.
        /// </summary>
        public IsValidatedHouseholdMemberAttribute()
            : this(UnityConfig.GetConfiguredContainer().Resolve<IClaimValueProvider>())
        {
        }

        /// <summary>
        /// Creates an attribute which can insure that the user is a validated household member.
        /// </summary>
        /// <param name="claimValueProvider">Implementation of a provider which can get values from claims.</param>
        public IsValidatedHouseholdMemberAttribute(IClaimValueProvider claimValueProvider)
        {
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException("claimValueProvider");
            }
            _claimValueProvider = claimValueProvider;
        }

        #endregion

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