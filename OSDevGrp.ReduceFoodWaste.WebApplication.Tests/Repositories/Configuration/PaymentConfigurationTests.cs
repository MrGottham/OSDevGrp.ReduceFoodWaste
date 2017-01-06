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
        /// <summary>
        /// Tests that the constructor initialize the configuration for payments.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentConfiguration()
        {
            IPaymentConfiguration paymentConfiguration = PaymentConfiguration.Create();
            Assert.That(paymentConfiguration, Is.Not.Null);
        }
    }
}
