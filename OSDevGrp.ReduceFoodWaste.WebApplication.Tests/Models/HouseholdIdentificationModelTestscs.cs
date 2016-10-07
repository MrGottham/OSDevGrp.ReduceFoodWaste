using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a household identification.
    /// </summary>
    [TestFixture]
    public class HouseholdIdentificationModelTestscs : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdIdentificationModel()
        {
            var householdIdentificationModel = new HouseholdModel();
            Assert.That(householdIdentificationModel, Is.Not.Null);
            Assert.That(householdIdentificationModel.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(householdIdentificationModel.Name, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Identifier sets a new value.
        /// </summary>
        [Test]
        public void TestThatIdentifierSetterSetsValue()
        {
            var householdIdentificationModel = new HouseholdModel();
            Assert.That(householdIdentificationModel, Is.Not.Null);
            Assert.That(householdIdentificationModel.Identifier, Is.EqualTo(default(Guid)));

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(householdIdentificationModel.Identifier));

            householdIdentificationModel.Identifier = newValue;
            Assert.That(householdIdentificationModel.Identifier, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Name sets a new value.
        /// </summary>
        [Test]
        public void TestThatNameSetterSetsValue()
        {
            var householdIdentificationModel = new HouseholdModel();
            Assert.That(householdIdentificationModel, Is.Not.Null);
            Assert.That(householdIdentificationModel.Name, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(householdIdentificationModel.Name));

            householdIdentificationModel.Name = newValue;
            Assert.That(householdIdentificationModel.Name, Is.EqualTo(newValue));
        }
    }
}
