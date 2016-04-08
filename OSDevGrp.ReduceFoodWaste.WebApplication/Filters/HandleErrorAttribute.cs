using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Filters
{
    /// <summary>
    /// Attribute which handle an exception in the MVC controllers.
    /// </summary>
    public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        #region Private constants

        private const string ErrorViewName = "Error";

        #endregion

        #region Methods

        /// <summary>
        /// Handles an exception which has occourred in the MVC controllers.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            filterContext.ExceptionHandled = true;
            if (filterContext.Exception is ReduceFoodWasteBusinessException)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = ErrorViewName,
                    ViewData = new ViewDataDictionary(GenerateModel(filterContext.Exception as ReduceFoodWasteBusinessException, (string) filterContext.RouteData.Values["controller"], (string) filterContext.RouteData.Values["action"]))
                };
                return;
            }

            throw new NotImplementedException();
        }

        private HandleErrorInfo GenerateModel(Exception exception, string controllerName, string actionName)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentNullException("controllerName");
            }
            return new HandleErrorInfo(exception, controllerName, actionName);
        }

        #endregion
    }
}