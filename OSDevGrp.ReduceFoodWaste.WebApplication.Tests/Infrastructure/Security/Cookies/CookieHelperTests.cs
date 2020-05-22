using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Cookies;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;
using System;
using System.Web;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Infrastructure.Security.Cookies
{
    /// <summary>
    /// Tests the helper functionality to cookies.
    /// </summary>
    [TestFixture]
    public class CookieHelperTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the helper functionality to cookies.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeCookieHelper()
        {
            var cookieHelper = new CookieHelper();
            Assert.That(cookieHelper, Is.Not.Null);
        }

        /// <summary>
        /// Tests that SetCookieConsent throws an ArgumentNullException when the HTTP response is null.
        /// </summary>
        [Test]
        public void TestThatSetCookieConsentThrowsArgumentNullExceptionWhenHttpResponseIsNull()
        {
            var cookieHelper = CreateCookieHelper();
            Assert.That(cookieHelper, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => cookieHelper.SetCookieConsent(null, Fixture.Create<bool>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("httpResponse"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that SetCookieConsent sets the value to true on the consent cookie when acceptance of the cookie consent is true.
        /// </summary>
        [Test]
        public void TestThatSetCookieConsentSetsValueToTrueOnConsentCookieWhenConsentIsTrue()
        {
            var cookieHelper = CreateCookieHelper();
            Assert.That(cookieHelper, Is.Not.Null);

            var cookie = new HttpCookie(CookieConsentAttribute.ConsentCookieName, Fixture.Create<string>())
            {
                Expires = DateTime.Now.AddDays(1)
            };
            Assert.That(cookie, Is.Not.Null);
            Assert.That(cookie.Value, Is.Not.Null);
            Assert.That(cookie.Value, Is.Not.Empty);
            Assert.That(cookie.Value, Is.Not.EqualTo("true"));
            Assert.That(cookie.Expires, Is.GreaterThan(DateTime.Now.AddSeconds(3)));

            var httpResponseMock = CreateHttpResponseMockWithCookie(cookie);
            Assert.That(httpResponseMock, Is.Not.Null);
            Assert.That(httpResponseMock.Cookies, Is.Not.Null);
            Assert.That(httpResponseMock.Cookies, Is.Not.Empty);
            Assert.That(httpResponseMock.Cookies[cookie.Name], Is.Not.Null);

            cookieHelper.SetCookieConsent(httpResponseMock, true);

            Assert.That(httpResponseMock.Cookies, Is.Not.Null);
            Assert.That(httpResponseMock.Cookies, Is.Not.Empty);

            var updatedCookie = httpResponseMock.Cookies[cookie.Name];
            Assert.That(updatedCookie, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(updatedCookie.Value, Is.Not.Null);
            Assert.That(updatedCookie.Value, Is.Not.Empty);
            Assert.That(updatedCookie.Value, Is.EqualTo("true"));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Tests that SetCookieConsent sets the value to false on the consent cookie when acceptance of the cookie consent is false.
        /// </summary>
        [Test]
        public void TestThatSetCookieConsentSetsValueToFalseOnConsentCookieWhenConsentIsFalse()
        {
            var cookieHelper = CreateCookieHelper();
            Assert.That(cookieHelper, Is.Not.Null);

            var cookie = new HttpCookie(CookieConsentAttribute.ConsentCookieName, Fixture.Create<string>())
            {
                Expires = DateTime.Now.AddDays(1)
            };
            Assert.That(cookie, Is.Not.Null);
            Assert.That(cookie.Value, Is.Not.Null);
            Assert.That(cookie.Value, Is.Not.Empty);
            Assert.That(cookie.Value, Is.Not.EqualTo("false"));
            Assert.That(cookie.Expires, Is.GreaterThan(DateTime.Now.AddSeconds(3)));

            var httpResponseMock = CreateHttpResponseMockWithCookie(cookie);
            Assert.That(httpResponseMock, Is.Not.Null);
            Assert.That(httpResponseMock.Cookies, Is.Not.Null);
            Assert.That(httpResponseMock.Cookies, Is.Not.Empty);
            Assert.That(httpResponseMock.Cookies[cookie.Name], Is.Not.Null);

            cookieHelper.SetCookieConsent(httpResponseMock, false);

            Assert.That(httpResponseMock.Cookies, Is.Not.Null);
            Assert.That(httpResponseMock.Cookies, Is.Not.Empty);

            var updatedCookie = httpResponseMock.Cookies[cookie.Name];
            Assert.That(updatedCookie, Is.Not.Null);
            // ReSharper disable PossibleNullReferenceException
            Assert.That(updatedCookie.Value, Is.Not.Null);
            Assert.That(updatedCookie.Value, Is.Not.Empty);
            Assert.That(updatedCookie.Value, Is.EqualTo("false"));
            // ReSharper restore PossibleNullReferenceException
        }

        /// <summary>
        /// Creates an instance of the helper functionality to cookies for unit testing.
        /// </summary>
        /// <returns>Instance of the helper functionality to cookies for unit testing.</returns>
        private static ICookieHelper CreateCookieHelper()
        {
            return new CookieHelper();
        }

        /// <summary>
        /// Creates a mockup for a HTTP response with a cookie included.
        /// </summary>
        /// <param name="cookie">Cookie which should be included in the HTTP response.</param>
        /// <returns>Mockup for a HTTP response with a cookie included.</returns>
        private static HttpResponseBase CreateHttpResponseMockWithCookie(HttpCookie cookie)
        {
            if (cookie == null)
            {
                throw new ArgumentNullException("cookie");
            }

            var httpResponseMock = MockRepository.GenerateMock<HttpResponseBase>();
            httpResponseMock.Stub(m => m.Cookies)
                .Return(new HttpCookieCollection {cookie})
                .Repeat.Any();

            return httpResponseMock;
        }
    }
}
