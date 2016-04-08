using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Infrastructure.Security.Exceptions
{
    /// <summary>
    /// Tests the business exception used by the Reduce Food Waste Web Application.
    /// </summary>
    [TestFixture]
    public class ReduceFoodWasteBusinessExceptionTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor without an inner exception initialize a business exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatConstructorWithoutInnerExceptionInitializeReduceFoodWasteBusinessException()
        {
            var message = Fixture.Create<string>();

            var reduceFoodWasteBusinessException = new ReduceFoodWasteBusinessException(message);
            Assert.That(reduceFoodWasteBusinessException, Is.Not.Null);
            Assert.That(reduceFoodWasteBusinessException.Message, Is.Not.Null);
            Assert.That(reduceFoodWasteBusinessException.Message, Is.Not.Empty);
            Assert.That(reduceFoodWasteBusinessException.Message, Is.EqualTo(message));
            Assert.That(reduceFoodWasteBusinessException.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor without an inner exception throws an ArgumentNullException when the message is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorWithoutInnerExceptionThrowsArgumentNullExceptionWhenMessageIsNullOrEmpty(string testValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ReduceFoodWasteBusinessException(testValue));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("message"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor with an inner exception initialize a business exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatConstructorWithInnerExceptionInitializeReduceFoodWasteBusinessException()
        {
            var message = Fixture.Create<string>();
            var innerException = Fixture.Create<Exception>();

            var reduceFoodWasteBusinessException = new ReduceFoodWasteBusinessException(message, innerException);
            Assert.That(reduceFoodWasteBusinessException, Is.Not.Null);
            Assert.That(reduceFoodWasteBusinessException.Message, Is.Not.Null);
            Assert.That(reduceFoodWasteBusinessException.Message, Is.Not.Empty);
            Assert.That(reduceFoodWasteBusinessException.Message, Is.EqualTo(message));
            Assert.That(reduceFoodWasteBusinessException.InnerException, Is.Not.Null);
            Assert.That(reduceFoodWasteBusinessException.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// Tests that the constructor with an inner exception throws an ArgumentNullException when the message is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorWithInnerExceptionThrowsArgumentNullExceptionWhenMessageIsNullOrEmpty(string testValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ReduceFoodWasteBusinessException(testValue, Fixture.Create<Exception>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("message"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor with an inner exception throws an ArgumentNullException when the inner exception is null.
        /// </summary>
        [Test]
        public void TestThatConstructorWithInnerExceptionThrowsArgumentNullExceptionWhenInnerExceptionIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ReduceFoodWasteBusinessException(Fixture.Create<string>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("innerException"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
