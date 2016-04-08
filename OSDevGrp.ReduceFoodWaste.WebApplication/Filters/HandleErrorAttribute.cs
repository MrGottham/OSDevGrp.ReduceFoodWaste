using System;
using System.Web.Mvc;
using System.Web.Routing;
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
        /// Handles an exception which has occurred in the MVC controllers.
        /// </summary>
        /// <param name="exceptionContext">Exception context.</param>
        public override void OnException(ExceptionContext exceptionContext)
        {
            if (exceptionContext == null)
            {
                throw new ArgumentNullException("exceptionContext");
            }

            exceptionContext.ExceptionHandled = true;
            if (exceptionContext.Exception is ReduceFoodWasteBusinessException)
            {
                exceptionContext.Result = new ViewResult
                {
                    ViewName = ErrorViewName,
                    ViewData = new ViewDataDictionary(GenerateHandleErrorInfo(exceptionContext.Exception as ReduceFoodWasteBusinessException, GetControllerName(exceptionContext.RouteData), GetActionName(exceptionContext.RouteData)))
                };
                return;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a handle error info.
        /// </summary>
        /// <param name="exception">Exception which has occurred.</param>
        /// <param name="controllerName">Name of the controller where the exception occurred.</param>
        /// <param name="actionName">Name of the action where the exception occurred.</param>
        /// <returns>Handle error info.</returns>
        private static HandleErrorInfo GenerateHandleErrorInfo(Exception exception, string controllerName, string actionName)
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

        /// <summary>
        /// Gets the name of the controller where the exception occurred.
        /// </summary>
        /// <param name="routeData">Route data.</param>
        /// <returns>Name of the controller where the exception occurred.</returns>
        private static string GetControllerName(RouteData routeData)
        {
            if (routeData == null)
            {
                throw new ArgumentNullException("routeData");
            }
            return (string) routeData.Values["controller"];
        }

        /// <summary>
        /// Gets the name of the action where the exception occurred.
        /// </summary>
        /// <param name="routeData">Route data.</param>
        /// <returns>Name of the action where the exception occurred.</returns>
        private static string GetActionName(RouteData routeData)
        {
            if (routeData == null)
            {
                throw new ArgumentNullException("routeData");
            }
            return (string)routeData.Values["action"];
        }

        #endregion
    }
}