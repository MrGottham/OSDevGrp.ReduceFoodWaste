using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

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
        }
    }
}
