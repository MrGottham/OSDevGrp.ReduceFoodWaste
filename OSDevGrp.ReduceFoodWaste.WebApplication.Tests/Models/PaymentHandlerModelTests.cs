using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a data provider who can handle payments.
    /// </summary>
    [TestFixture]
    public class PaymentHandlerModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a data provider who can handle payments.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentHandlerModel()
        {
            var paymentHandlerModel = new PaymentHandlerModel();
            Assert.That(paymentHandlerModel, Is.Not.Null);
            Assert.That(paymentHandlerModel.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(paymentHandlerModel.Name, Is.Null);
            Assert.That(paymentHandlerModel.DataSourceStatement, Is.Null);
            Assert.That(paymentHandlerModel.ImagePath, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for ImagePath sets a new value.
        /// </summary>
        [Test]
        public void TestThatImagePathSetterSetsValue()
        {
            var paymentHandlerModel = new PaymentHandlerModel();
            Assert.That(paymentHandlerModel, Is.Not.Null);
            Assert.That(paymentHandlerModel.ImagePath, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            paymentHandlerModel.ImagePath = newValue;
            Assert.That(paymentHandlerModel.ImagePath, Is.Not.Null);
            Assert.That(paymentHandlerModel.ImagePath, Is.Not.Empty);
            Assert.That(paymentHandlerModel.ImagePath, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for ImagePath sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatImagePathSetterSetsValueToNull()
        {
            var paymentHandlerModel = new PaymentHandlerModel
            {
                ImagePath = Fixture.Create<string>()
            };
            Assert.That(paymentHandlerModel, Is.Not.Null);
            Assert.That(paymentHandlerModel.ImagePath, Is.Not.Null);
            Assert.That(paymentHandlerModel.ImagePath, Is.Not.Empty);

            paymentHandlerModel.ImagePath = null;
            Assert.That(paymentHandlerModel.ImagePath, Is.Null);
        }

    }
}
