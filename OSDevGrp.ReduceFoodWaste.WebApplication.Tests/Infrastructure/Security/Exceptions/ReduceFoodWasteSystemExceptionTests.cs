using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Infrastructure.Security.Exceptions
{
    /// <summary>
    /// Tests the system exception used by the Reduce Food Waste Web Application.
    /// </summary>
    [TestFixture]
    public class ReduceFoodWasteSystemExceptionTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor without an inner exception initialize a ystem exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatConstructorWithoutInnerExceptionInitializeReduceFoodWasteSystemException()
        {
            var message = Fixture.Create<string>();

            var reduceFoodWasteSystemException = new ReduceFoodWasteSystemException(message);
            Assert.That(reduceFoodWasteSystemException, Is.Not.Null);
            Assert.That(reduceFoodWasteSystemException.Message, Is.Not.Null);
            Assert.That(reduceFoodWasteSystemException.Message, Is.Not.Empty);
            Assert.That(reduceFoodWasteSystemException.Message, Is.EqualTo(message));
            Assert.That(reduceFoodWasteSystemException.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor without an inner exception throws an ArgumentNullException when the message is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorWithoutInnerExceptionThrowsArgumentNullExceptionWhenMessageIsNullOrEmpty(string testValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ReduceFoodWasteSystemException(testValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("message"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor with an inner exception initialize a ystem exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatConstructorWithInnerExceptionInitializeReduceFoodWasteSystemException()
        {
            var message = Fixture.Create<string>();
            var innerException = Fixture.Create<Exception>();

            var reduceFoodWasteSystemException = new ReduceFoodWasteSystemException(message, innerException);
            Assert.That(reduceFoodWasteSystemException, Is.Not.Null);
            Assert.That(reduceFoodWasteSystemException.Message, Is.Not.Null);
            Assert.That(reduceFoodWasteSystemException.Message, Is.Not.Empty);
            Assert.That(reduceFoodWasteSystemException.Message, Is.EqualTo(message));
            Assert.That(reduceFoodWasteSystemException.InnerException, Is.Not.Null);
            Assert.That(reduceFoodWasteSystemException.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// Tests that the constructor with an inner exception throws an ArgumentNullException when the message is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorWithInnerExceptionThrowsArgumentNullExceptionWhenMessageIsNullOrEmpty(string testValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ReduceFoodWasteSystemException(testValue, Fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("message"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
