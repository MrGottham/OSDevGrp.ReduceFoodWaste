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
                    ViewData =
                        new ViewDataDictionary(
                            GenerateModel(exceptionContext.Exception as ReduceFoodWasteBusinessException,
                                (string) exceptionContext.RouteData.Values["controller"],
                                (string) exceptionContext.RouteData.Values["action"]))
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