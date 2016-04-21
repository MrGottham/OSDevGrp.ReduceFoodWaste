using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for the privacy policies.
    /// </summary>
    [TestFixture]
    public class PrivacyPolicyModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for the privacy policies.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePrivacyPolicyModel()
        {
            var privacyPolicyModel = new PrivacyPolicyModel();
            Assert.That(privacyPolicyModel, Is.Not.Null);
        }
    }
}
