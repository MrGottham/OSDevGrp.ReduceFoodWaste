using System;
using System.Web.Mvc;
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
        /// Tests that OnException throws an ArgumentNullException when the filter context is null.
        /// </summary>
        [Test]
        public void X_TestThatOnExceptionThrowsArgumentNullExceptionWhenFilterContextIsNull()
        {
            var handleErrorAttribute = new HandleErrorAttribute();
            Assert.That(handleErrorAttribute, Is.Not.Null);

            var reduceFoodWasteBusinessException = new ReduceFoodWasteBusinessException(Fixture.Create<string>());
            var exceptionContext = new ExceptionContext
            {
                Exception = reduceFoodWasteBusinessException,
                ExceptionHandled = false
            };

            handleErrorAttribute.OnException(exceptionContext);
        }
    }
}
