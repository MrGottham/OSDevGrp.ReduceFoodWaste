using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model on which it's possible to make a payment.
    /// </summary>
    [TestFixture]
    public class PayableModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model on which it's possible to make a payment.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePayableModel()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Null);
            Assert.That(payableModel.Price, Is.EqualTo(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.EqualTo(CultureInfo.CurrentUICulture));
            Assert.That(payableModel.IsFreeOfCost, Is.True);
            Assert.That(payableModel.PaymentHandler, Is.Null);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
        }

        /// <summary>
        /// Tests that the getter for BillingInformation converts the price tag.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatBillingInformationGetterConvertsPriceTag(string cultureName)
        {
            var billingInformation = $"{Fixture.Create<string>()}[Price]{Fixture.Create<string>()}";
            Assert.That(billingInformation, Is.Not.Null);
            Assert.That(billingInformation, Is.Not.Empty);
            Assert.That(billingInformation.Contains("[Price]"), Is.True);

            var price = Fixture.Create<decimal>();

            var expectedBillingInformation = billingInformation.Replace("[Price]", price.ToString("C", new CultureInfo(cultureName)));
            Assert.That(expectedBillingInformation, Is.Not.Null);
            Assert.That(expectedBillingInformation, Is.Not.Empty);
            Assert.That(expectedBillingInformation.Contains("[Price]"), Is.False);

            var payableModel = new PayableModel
            {
                BillingInformation = billingInformation,
                Price = price,
                PriceCultureInfoName = cultureName
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Empty);
            Assert.That(payableModel.BillingInformation, Is.EqualTo(expectedBillingInformation));
            Assert.That(payableModel.Price, Is.EqualTo(price));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(cultureName));
            Assert.That(payableModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.EqualTo(new CultureInfo(cultureName)));
        }

        /// <summary>
        /// Tests that the setter for BillingInformation sets a new value.
        /// </summary>
        [Test]
        public void TestThatBillingInformationSetterSetsValue()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            payableModel.BillingInformation = newValue;
            Assert.That(payableModel.BillingInformation, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Empty);
            Assert.That(payableModel.BillingInformation, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for BillingInformation sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatBillingInformationSetterSetsValueToNull()
        {
            var payableModel = new PayableModel
            {
                BillingInformation = Fixture.Create<string>()
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Empty);

            payableModel.BillingInformation = null;
            Assert.That(payableModel.BillingInformation, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Price sets a new value.
        /// </summary>
        [Test]
        public void TestThatPriceSetterSetsValue()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.Price, Is.EqualTo(0M));
            Assert.That(payableModel.IsFreeOfCost, Is.True);

            var newValue = Fixture.Create<decimal>();
            Assert.That(newValue, Is.Not.EqualTo(0M));

            payableModel.Price = newValue;
            Assert.That(payableModel.Price, Is.EqualTo(newValue));
            Assert.That(payableModel.IsFreeOfCost, Is.EqualTo(newValue <= 0));
        }

        /// <summary>
        /// Tests that the setter for Price sets the value to 0.
        /// </summary>
        [Test]
        public void TestThatPriceSetterSetsValueToZero()
        {
            var payableModel = new PayableModel
            {
                Price = Fixture.Create<decimal>()
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.Price, Is.Not.EqualTo(0M));
            Assert.That(payableModel.IsFreeOfCost, Is.EqualTo(payableModel.Price <= 0));

            payableModel.Price = 0M;
            Assert.That(payableModel.Price, Is.EqualTo(0M));
            Assert.That(payableModel.IsFreeOfCost, Is.True);
        }

        /// <summary>
        /// Tests that the setter for PriceCultureInfoName sets a new value.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatPriceCultureInfoNameSetterSetsValue(string cultureName)
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.EqualTo(CultureInfo.CurrentUICulture));

            payableModel.PriceCultureInfoName = cultureName;
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(cultureName));
            Assert.That(payableModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.EqualTo(new CultureInfo(cultureName)));
        }

        /// <summary>
        /// Tests that the setter for PriceCultureInfoName sets the value to NULL.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatPriceCultureInfoNameSetterSetsValueToNull(string cultureName)
        {
            var payableModel = new PayableModel
            {
                PriceCultureInfoName = cultureName
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(cultureName));
            Assert.That(payableModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.EqualTo(new CultureInfo(cultureName)));

            payableModel.PriceCultureInfoName = null;
            Assert.That(payableModel.PriceCultureInfoName, Is.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfo, Is.EqualTo(CultureInfo.CurrentUICulture));
        }

        /// <summary>
        /// Tests that the setter for PaymentHandler sets a new value.
        /// </summary>
        [Test]
        public void TestThatPaymentHandlerSetterSetsValue()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentHandler, Is.Null);

            var newValue = Fixture.Create<PaymentHandlerModel>();
            Assert.That(newValue, Is.Not.Null);

            payableModel.PaymentHandler = newValue;
            Assert.That(payableModel.PaymentHandler, Is.Not.Null);
            Assert.That(payableModel.PaymentHandler, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PaymentHandler sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatPaymentHandlerSetterSetsValueToNull()
        {
            var payableModel = new PayableModel
            {
                PaymentHandler = Fixture.Create<PaymentHandlerModel>()
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentHandler, Is.Not.Null);

            payableModel.PaymentHandler = null;
            Assert.That(payableModel.PaymentHandler, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for PaymentHandlers sets a new value.
        /// </summary>
        [Test]
        public void TestThatPaymentHandlersSetterSetsValue()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlers, Is.Null);

            var newValue = Fixture.CreateMany<PaymentHandlerModel>(Random.Next(5, 10)).ToList();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            payableModel.PaymentHandlers = newValue;
            Assert.That(payableModel.PaymentHandlers, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlers, Is.Not.Empty);
            Assert.That(payableModel.PaymentHandlers, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PaymentHandlers sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatPaymentHandlersSetterSetsValueToNull()
        {
            var payableModel = new PayableModel
            {
                PaymentHandlers = Fixture.CreateMany<PaymentHandlerModel>(Random.Next(5, 10)).ToList()
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlers, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlers, Is.Not.Empty);

            payableModel.PaymentHandlers = null;
            Assert.That(payableModel.PaymentHandlers, Is.Null);
        }
    }
}
