using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories
{
    /// <summary>
    /// Tests the user name and password credential.
    /// </summary>
    [TestFixture]
    public class UserNamePasswordCredentialTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize an user name and password credential.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeUserNamePasswordCredential()
        {
            var userName = Fixture.Create<string>();
            var password = Fixture.Create<string>();

            var userNamePasswordCredential = new UserNamePasswordCredential(userName, password);
            Assert.That(userNamePasswordCredential, Is.Not.Null);
            Assert.That(userNamePasswordCredential.UserName, Is.Not.Null);
            Assert.That(userNamePasswordCredential.UserName, Is.Not.Empty);
            Assert.That(userNamePasswordCredential.UserName, Is.EqualTo(userName));
            Assert.That(userNamePasswordCredential.Password, Is.Not.Null);
            Assert.That(userNamePasswordCredential.Password, Is.Not.Empty);
            Assert.That(userNamePasswordCredential.Password, Is.EqualTo(password));
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the user name is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorInitializeThrowsArgumentNullExceptionWhenUserNameIsInvalid(string invalidUserName)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new UserNamePasswordCredential(invalidUserName, Fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("userName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the password is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatConstructorInitializeThrowsArgumentNullExceptionWhenPasswordIsInvalid(string invalidPassword)
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new UserNamePasswordCredential(Fixture.Create<string>(), invalidPassword));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("password"));
            Assert.That(exception.InnerException, Is.Null);
        }
    }
}
