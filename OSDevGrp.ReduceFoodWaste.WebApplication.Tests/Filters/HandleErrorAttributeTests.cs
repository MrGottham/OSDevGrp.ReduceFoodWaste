using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;
using HandleErrorAttribute = OSDevGrp.ReduceFoodWaste.WebApplication.Filters.HandleErrorAttribute;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Filters
{
    /// <summary>
    /// Tests the attribute which handle an exception in the MVC controllers.
    /// </summary>
    [TestFixture]
    public class HandleErrorAttributeTests : TestBase
    {
        #region Private constants

        private const string ErrorViewName = "Error";

        #endregion

        /// <summary>
        /// Tests that the constructor initialize an attribute which handle an exception in the MVC controllers.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHandleErrorAttribute()
        {
            var handleErrorAttribute = new HandleErrorAttribute();
            Assert.That(handleErrorAttribute, Is.Not.Null);
        }

        /// <summary>
        /// Tests that OnException throws an ArgumentNullException when the exception context is null.
        /// </summary>
        [Test]
        public void TestThatOnExceptionThrowsArgumentNullExceptionWhenExceptionContextIsNull()
        {
            var handleErrorAttribute = new HandleErrorAttribute();
            Assert.That(handleErrorAttribute, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => handleErrorAttribute.OnException(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("exceptionContext"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that OnException return a view result for the business exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatOnExceptionReturnViewResultForReduceFoodWasteBusinessException()
        {
            var handleErrorAttribute = new HandleErrorAttribute();
            Assert.That(handleErrorAttribute, Is.Not.Null);

            var reduceFoodWasteBusinessException = new ReduceFoodWasteBusinessException(Fixture.Create<string>());
            var controllerName = Fixture.Create<string>();
            var actionName = Fixture.Create<string>();

            var exceptionContext = CreateExceptionContext(reduceFoodWasteBusinessException, controllerName, actionName);
            Assert.That(exceptionContext, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<EmptyResult>());
            Assert.That(exceptionContext.ExceptionHandled, Is.False);

            handleErrorAttribute.OnException(exceptionContext);

            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<ViewResult>());
            Assert.That(exceptionContext.ExceptionHandled, Is.True);

            var viewResult = (ViewResult) exceptionContext.Result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo(ErrorViewName));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HandleErrorInfo>());

            var handleErrorInfo = (HandleErrorInfo) viewResult.Model;
            Assert.That(handleErrorInfo, Is.Not.Null);
            Assert.That(handleErrorInfo.Exception, Is.Not.Null);
            Assert.That(handleErrorInfo.Exception, Is.TypeOf<ReduceFoodWasteBusinessException>());
            Assert.That(handleErrorInfo.Exception, Is.EqualTo(reduceFoodWasteBusinessException));
            Assert.That(handleErrorInfo.ControllerName, Is.Not.Null);
            Assert.That(handleErrorInfo.ControllerName, Is.Not.Empty);
            Assert.That(handleErrorInfo.ControllerName, Is.EqualTo(controllerName));
            Assert.That(handleErrorInfo.ActionName, Is.Not.Null);
            Assert.That(handleErrorInfo.ActionName, Is.Not.Empty);
            Assert.That(handleErrorInfo.ActionName, Is.EqualTo(actionName));
        }

        /// <summary>
        /// Tests that OnException return a view result for the system exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatOnExceptionReturnViewResultForReduceFoodWasteSystemException()
        {
            var handleErrorAttribute = new HandleErrorAttribute();
            Assert.That(handleErrorAttribute, Is.Not.Null);

            var reduceFoodWasteSystemException = new ReduceFoodWasteSystemException(Fixture.Create<string>());
            var controllerName = Fixture.Create<string>();
            var actionName = Fixture.Create<string>();

            var exceptionContext = CreateExceptionContext(reduceFoodWasteSystemException, controllerName, actionName);
            Assert.That(exceptionContext, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<EmptyResult>());
            Assert.That(exceptionContext.ExceptionHandled, Is.False);

            handleErrorAttribute.OnException(exceptionContext);

            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<ViewResult>());
            Assert.That(exceptionContext.ExceptionHandled, Is.True);

            var viewResult = (ViewResult)exceptionContext.Result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo(ErrorViewName));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HandleErrorInfo>());

            var handleErrorInfo = (HandleErrorInfo) viewResult.Model;
            Assert.That(handleErrorInfo, Is.Not.Null);
            Assert.That(handleErrorInfo.Exception, Is.Not.Null);
            Assert.That(handleErrorInfo.Exception, Is.TypeOf<ReduceFoodWasteSystemException>());
            Assert.That(handleErrorInfo.Exception, Is.EqualTo(reduceFoodWasteSystemException));
            Assert.That(handleErrorInfo.ControllerName, Is.Not.Null);
            Assert.That(handleErrorInfo.ControllerName, Is.Not.Empty);
            Assert.That(handleErrorInfo.ControllerName, Is.EqualTo(controllerName));
            Assert.That(handleErrorInfo.ActionName, Is.Not.Null);
            Assert.That(handleErrorInfo.ActionName, Is.Not.Empty);
            Assert.That(handleErrorInfo.ActionName, Is.EqualTo(actionName));
        }

        /// <summary>
        /// Tests that OnException return a view result for the repository exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatOnExceptionReturnViewResultForReduceFoodWasteRepositoryException()
        {
            var handleErrorAttribute = new HandleErrorAttribute();
            Assert.That(handleErrorAttribute, Is.Not.Null);

            var reduceFoodWasteRepositoryException = new ReduceFoodWasteRepositoryException(Fixture.Create<string>(), MethodBase.GetCurrentMethod());
            var controllerName = Fixture.Create<string>();
            var actionName = Fixture.Create<string>();

            var exceptionContext = CreateExceptionContext(reduceFoodWasteRepositoryException, controllerName, actionName);
            Assert.That(exceptionContext, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<EmptyResult>());
            Assert.That(exceptionContext.ExceptionHandled, Is.False);

            handleErrorAttribute.OnException(exceptionContext);

            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<ViewResult>());
            Assert.That(exceptionContext.ExceptionHandled, Is.True);

            var viewResult = (ViewResult) exceptionContext.Result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo(ErrorViewName));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HandleErrorInfo>());

            var handleErrorInfo = (HandleErrorInfo) viewResult.Model;
            Assert.That(handleErrorInfo, Is.Not.Null);
            Assert.That(handleErrorInfo.Exception, Is.Not.Null);
            Assert.That(handleErrorInfo.Exception, Is.TypeOf<ReduceFoodWasteRepositoryException>());
            Assert.That(handleErrorInfo.Exception, Is.EqualTo(reduceFoodWasteRepositoryException));
            Assert.That(handleErrorInfo.ControllerName, Is.Not.Null);
            Assert.That(handleErrorInfo.ControllerName, Is.Not.Empty);
            Assert.That(handleErrorInfo.ControllerName, Is.EqualTo(controllerName));
            Assert.That(handleErrorInfo.ActionName, Is.Not.Null);
            Assert.That(handleErrorInfo.ActionName, Is.Not.Empty);
            Assert.That(handleErrorInfo.ActionName, Is.EqualTo(actionName));
        }

        /// <summary>
        /// Tests that OnException return a view result for an exception.
        /// </summary>
        [Test]
        public void TestThatOnExceptionReturnViewResultForException()
        {
            var handleErrorAttribute = new HandleErrorAttribute();
            Assert.That(handleErrorAttribute, Is.Not.Null);

            var exception = Fixture.Create<Exception>();
            var controllerName = Fixture.Create<string>();
            var actionName = Fixture.Create<string>();

            var exceptionContext = CreateExceptionContext(exception, controllerName, actionName);
            Assert.That(exceptionContext, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<EmptyResult>());
            Assert.That(exceptionContext.ExceptionHandled, Is.False);

            handleErrorAttribute.OnException(exceptionContext);

            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<ViewResult>());
            Assert.That(exceptionContext.ExceptionHandled, Is.True);

            var viewResult = (ViewResult)exceptionContext.Result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo(ErrorViewName));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HandleErrorInfo>());

            var handleErrorInfo = (HandleErrorInfo)viewResult.Model;
            Assert.That(handleErrorInfo, Is.Not.Null);
            Assert.That(handleErrorInfo.Exception, Is.Not.Null);
            Assert.That(handleErrorInfo.Exception, Is.TypeOf<Exception>());
            Assert.That(handleErrorInfo.Exception, Is.EqualTo(exception));
            Assert.That(handleErrorInfo.ControllerName, Is.Not.Null);
            Assert.That(handleErrorInfo.ControllerName, Is.Not.Empty);
            Assert.That(handleErrorInfo.ControllerName, Is.EqualTo(controllerName));
            Assert.That(handleErrorInfo.ActionName, Is.Not.Null);
            Assert.That(handleErrorInfo.ActionName, Is.Not.Empty);
            Assert.That(handleErrorInfo.ActionName, Is.EqualTo(actionName));
        }

        /// <summary>
        /// Creates an exception context which can be used for the tests.
        /// </summary>
        /// <typeparam name="TException">Type of the exception which should be handled.</typeparam>
        /// <param name="exception">Exception which should be handled.</param>
        /// <param name="controllerName">Name of the controller where the exception has occurred.</param>
        /// <param name="actionName">Name of the action where the exception has occurred.</param>
        /// <returns>Exception context.</returns>
        private static ExceptionContext CreateExceptionContext<TException>(TException exception, string controllerName, string actionName) where TException : Exception
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentNullException("controllerName");
            }
            if (string.IsNullOrEmpty("actionName"))
            {
                throw new ArgumentNullException("actionName");
            }

            var routeData = new RouteData();
            routeData.Values.Add("controller", controllerName);
            routeData.Values.Add("action", actionName);
            
            return new ExceptionContext
            {
                Exception = exception,
                ExceptionHandled = false,
                RouteData = routeData
            };
        }
    }
}
