using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a household member.
    /// </summary>
    [TestFixture]
    public class HouseholdMemberModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberModel()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Identifier sets a new value.
        /// </summary>
        [Test]
        public void TestThatIdentifierSetterSetsValue()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Identifier, Is.EqualTo(default(Guid)));

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.Identifier));

            householdMemberModel.Identifier = newValue;
            Assert.That(householdMemberModel.Identifier, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PrivacyPolicy sets a new value.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicySetterSetsValue()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Null);

            var newValue = Fixture.Create<PrivacyPolicyModel>();
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.PrivacyPolicy));

            householdMemberModel.PrivacyPolicy = newValue;
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(newValue));
        }
    }
}
