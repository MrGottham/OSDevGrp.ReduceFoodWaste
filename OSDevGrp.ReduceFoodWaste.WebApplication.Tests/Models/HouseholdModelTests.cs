using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a household.
    /// </summary>
    [TestFixture]
    public class HouseholdModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdModel()
        {
            var householdModel = new HouseholdModel();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(householdModel.Name, Is.Null);
            Assert.That(householdModel.Description, Is.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Identifier sets a new value.
        /// </summary>
        [Test]
        public void TestThatIdentifierSetterSetsValue()
        {
            var householdModel = new HouseholdModel();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(default(Guid)));

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(householdModel.Identifier));

            householdModel.Identifier = newValue;
            Assert.That(householdModel.Identifier, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Name sets a new value.
        /// </summary>
        [Test]
        public void TestThatNameSetterSetsValue()
        {
            var householdModel = new HouseholdModel();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Name, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(householdModel.Name));

            householdModel.Name = newValue;
            Assert.That(householdModel.Name, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Description sets a new value.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterSetsValue()
        {
            var householdModel = new HouseholdModel();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Description, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(householdModel.Description));

            householdModel.Description = newValue;
            Assert.That(householdModel.Description, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PrivacyPolicy sets a new value.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicySetterSetsValue()
        {
            var householdModel = new HouseholdModel();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Null);

            var newValue = Fixture.Create<PrivacyPolicyModel>();
            Assert.That(newValue, Is.Not.EqualTo(householdModel.PrivacyPolicy));

            householdModel.PrivacyPolicy = newValue;
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(newValue));
        }
    }
}
