using System;
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

            handleErrorAttribute.OnException(exceptionContext);

            Assert.That(exceptionContext.Result, Is.Not.Null);
            Assert.That(exceptionContext.Result, Is.TypeOf<ViewResult>());
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
