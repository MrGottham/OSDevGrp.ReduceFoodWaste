using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using System;
using System.Reflection;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Infrastructure.Security.Exceptions
{
    /// <summary>
    /// Tests the repository exception used by the Reduce Food Waste Web Application.
    /// </summary>
    [TestFixture]
    public class ReduceFoodWasteRepositoryExceptionTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor without an inner exception initialize a repository exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatConstructorWithoutInnerExceptionInitializeReduceFoodWasteRepositoryException()
        {
            var message = Fixture.Create<string>();
            var repositoryMethod = MethodBase.GetCurrentMethod();

            var reduceFoodWasteRepositoryException = new ReduceFoodWasteRepositoryException(message, repositoryMethod);
            Assert.That(reduceFoodWasteRepositoryException, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.Message, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.Message, Is.Not.Empty);
            Assert.That(reduceFoodWasteRepositoryException.Message, Is.EqualTo(message));
            Assert.That(reduceFoodWasteRepositoryException.MethodName, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.MethodName, Is.Not.Empty);
            Assert.That(reduceFoodWasteRepositoryException.MethodName, Is.EqualTo(repositoryMethod.Name));
            Assert.That(reduceFoodWasteRepositoryException.RepositoryName, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.RepositoryName, Is.Not.Empty);
            Assert.That(reduceFoodWasteRepositoryException.RepositoryName, Is.EqualTo(repositoryMethod.ReflectedType.Name));
            Assert.That(reduceFoodWasteRepositoryException.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor without an inner exception throws an ArgumentNullException when the message is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorWithoutInnerExceptionThrowsArgumentNullExceptionWhenMessageIsNullOrEmpty(string testValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ReduceFoodWasteRepositoryException(testValue, MethodBase.GetCurrentMethod()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("message"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor with an inner exception initialize a repository exception used by the Reduce Food Waste Web Application.
        /// </summary>
        [Test]
        public void TestThatConstructorWithInnerExceptionInitializeReduceFoodWasteRepositoryException()
        {
            var message = Fixture.Create<string>();
            var repositoryMethod = MethodBase.GetCurrentMethod();
            var innerException = Fixture.Create<Exception>();

            var reduceFoodWasteRepositoryException = new ReduceFoodWasteRepositoryException(message, repositoryMethod, innerException);
            Assert.That(reduceFoodWasteRepositoryException, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.Message, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.Message, Is.Not.Empty);
            Assert.That(reduceFoodWasteRepositoryException.Message, Is.EqualTo(message));
            Assert.That(reduceFoodWasteRepositoryException.MethodName, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.MethodName, Is.Not.Empty);
            Assert.That(reduceFoodWasteRepositoryException.MethodName, Is.EqualTo(repositoryMethod.Name));
            Assert.That(reduceFoodWasteRepositoryException.RepositoryName, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.RepositoryName, Is.Not.Empty);
            Assert.That(reduceFoodWasteRepositoryException.RepositoryName, Is.EqualTo(repositoryMethod.ReflectedType.Name));
            Assert.That(reduceFoodWasteRepositoryException.InnerException, Is.Not.Null);
            Assert.That(reduceFoodWasteRepositoryException.InnerException, Is.EqualTo(innerException));
        }

        /// <summary>
        /// Tests that the constructor with an inner exception throws an ArgumentNullException when the message is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorWithInnerExceptionThrowsArgumentNullExceptionWhenMessageIsNullOrEmpty(string testValue)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ReduceFoodWasteRepositoryException(testValue, MethodBase.GetCurrentMethod(), Fixture.Create<Exception>()));
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
            var exception = Assert.Throws<ArgumentNullException>(() => new ReduceFoodWasteRepositoryException(Fixture.Create<string>(), MethodBase.GetCurrentMethod(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("innerException"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
