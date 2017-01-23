using System;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories.Configuration
{
    /// <summary>
    /// Tests the configuration for payments.
    /// </summary>
    [TestFixture]
    public class PaymentConfigurationTests : TestBase
    {
        #region Private variables

        private static readonly Guid DataProviderIdentifierForPayPal = Guid.Parse("{9FF5EB98-B475-4FEB-A621-0DFBEA881552}");

        #endregion

        /// <summary>
        /// Tests that the constructor initialize the configuration for payments.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentConfiguration()
        {
            IPaymentConfiguration paymentConfiguration = PaymentConfiguration.Create();
            Assert.That(paymentConfiguration, Is.Not.Null);
            Assert.That(paymentConfiguration.PaymentHandlers, Is.Not.Null);
            Assert.That(paymentConfiguration.PaymentHandlers, Is.Not.Empty);
            Assert.That(paymentConfiguration.PaymentHandlers.Count(), Is.EqualTo(1));

            IPaymentHandlerElement paymentHandlerElementForPayPal = paymentConfiguration.GetPaymentHandler(DataProviderIdentifierForPayPal);
            Assert.That(paymentHandlerElementForPayPal, Is.Not.Null);
            Assert.That(paymentHandlerElementForPayPal.Identifier, Is.EqualTo(DataProviderIdentifierForPayPal));
            Assert.That(paymentHandlerElementForPayPal.ImagePath, Is.Not.Null);
            Assert.That(paymentHandlerElementForPayPal.ImagePath, Is.Not.Empty);
            Assert.That(paymentHandlerElementForPayPal.ImagePath, Is.EqualTo("~/Images/paypal.png"));
        }
    }
}
