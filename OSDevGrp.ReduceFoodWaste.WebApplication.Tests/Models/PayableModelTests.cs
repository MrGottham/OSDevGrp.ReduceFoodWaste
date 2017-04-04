using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models.Enums;
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
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentTime, Is.Null);
            Assert.That(payableModel.PaymentTime.HasValue, Is.False);
            Assert.That(payableModel.PaymentReference, Is.Null);
            Assert.That(payableModel.PaymentReceipt, Is.Null);
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
        /// Tests that the setter for PaymentHandlerIdentifier sets a new value.
        /// </summary>
        [Test]
        public void TestThatPaymentHandlerIdentifierSetterSetsValue()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(Guid.Empty));

            payableModel.PaymentHandlerIdentifier = newValue;
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Not.Null);
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            Assert.That(payableModel.PaymentHandlerIdentifier.Value, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PaymentHandlerIdentifier sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatPaymentHandlerIdentifierSetterSetsValueToNull()
        {
            var payableModel = new PayableModel
            {
                PaymentHandlerIdentifier = Guid.NewGuid()
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.True);

            payableModel.PaymentHandlerIdentifier = null;
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);
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

        /// <summary>
        /// Tests that the setter for PaymentStatus sets a new value.
        /// </summary>
        [Test]
        [TestCase(PaymentStatus.Paid)]
        [TestCase(PaymentStatus.Cancelled)]
        public void TestThatPaymentStatusSetterSetsValue(PaymentStatus newValue)
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));

            payableModel.PaymentStatus = newValue;
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PaymentStatus sets the value to Unpaid.
        /// </summary>
        [Test]
        [TestCase(PaymentStatus.Paid)]
        [TestCase(PaymentStatus.Cancelled)]
        public void TestThatPaymentStatusSetterSetsValueToUnpaid(PaymentStatus value)
        {
            var payableModel = new PayableModel
            {
                PaymentStatus = value
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(value));

            payableModel.PaymentStatus = PaymentStatus.Unpaid;
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
        }

        /// <summary>
        /// Tests that the setter for PaymentTime sets a new value.
        /// </summary>
        [Test]
        public void TestThatPaymentTimeSetterSetsValue()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentTime, Is.Null);
            Assert.That(payableModel.PaymentTime.HasValue, Is.False);

            var newValue = Fixture.Create<DateTime>();

            payableModel.PaymentTime = newValue;
            Assert.That(payableModel.PaymentTime, Is.Not.Null);
            Assert.That(payableModel.PaymentTime.HasValue, Is.True);
            Assert.That(payableModel.PaymentTime.Value, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PaymentTime sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatPaymentTimeSetterSetsValueToNull()
        {
            var payableModel = new PayableModel
            {
                PaymentTime = Fixture.Create<DateTime>()
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentTime, Is.Not.Null);
            Assert.That(payableModel.PaymentTime.HasValue, Is.True);

            payableModel.PaymentTime = null;
            Assert.That(payableModel.PaymentTime, Is.Null);
            Assert.That(payableModel.PaymentTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the setter for PaymentReference sets a new value.
        /// </summary>
        [Test]
        public void TestThatPaymentReferenceSetterSetsValue()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentReference, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            payableModel.PaymentReference = newValue;
            Assert.That(payableModel.PaymentReference, Is.Not.Null);
            Assert.That(payableModel.PaymentReference, Is.Not.Empty);
            Assert.That(payableModel.PaymentReference, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PaymentReference sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatPaymentReferenceSetterSetsValueToNull()
        {
            var payableModel = new PayableModel
            {
                PaymentReference = Fixture.Create<string>()
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentReference, Is.Not.Null);
            Assert.That(payableModel.PaymentReference, Is.Not.Empty);

            payableModel.PaymentReference = null;
            Assert.That(payableModel.PaymentReference, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for PaymentReceipt sets a new value.
        /// </summary>
        [Test]
        public void TestThatPaymentReceiptSetterSetsValue()
        {
            var payableModel = new PayableModel();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentReceipt, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            payableModel.PaymentReceipt = newValue;
            Assert.That(payableModel.PaymentReceipt, Is.Not.Null);
            Assert.That(payableModel.PaymentReceipt, Is.Not.Empty);
            Assert.That(payableModel.PaymentReceipt, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PaymentReceipt sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatPaymentReceiptSetterSetsValueToNull()
        {
            var payableModel = new PayableModel
            {
                PaymentReceipt = Fixture.Create<string>()
            };
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.PaymentReceipt, Is.Not.Null);
            Assert.That(payableModel.PaymentReceipt, Is.Not.Empty);

            payableModel.PaymentReceipt = null;
            Assert.That(payableModel.PaymentReceipt, Is.Null);
        }
    }
}
