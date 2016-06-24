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
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            Assert.That(householdMemberModel.IsActivated, Is.False);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Null);
            Assert.That(householdMemberModel.HasAcceptedPrivacyPolicy, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);
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
        /// Tests that the setter for ActivationCode sets the value to a given activation code.
        /// </summary>
        [Test]
        public void TestThatActivationCodeSetterSetsValueToActivationCode()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.ActivationCode));

            householdMemberModel.ActivationCode = newValue;
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for ActivationCode sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatActivationCodeSetterSetsValueToNull()
        {
            var householdMemberModel = new HouseholdMemberModel
            {
                ActivationCode = Fixture.Create<string>()
            };
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);

            const string newValue = null;
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.ActivationCode));

            householdMemberModel.ActivationCode = newValue;
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for ActivatedTime sets the value to a given date and time.
        /// </summary>
        [Test]
        public void TestThatActivatedTimeSetterSetsValueToDateTime()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.IsActivated, Is.False);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            var newValue = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));

            householdMemberModel.ActivatedTime = newValue;
            Assert.That(householdMemberModel.IsActivated, Is.True);
            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.EqualTo(newValue));
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        /// <summary>
        /// Tests that the setter for ActivatedTime sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatActivatedTimeSetterSetsValueToNull()
        {
            var householdMemberModel = new HouseholdMemberModel
            {
                ActivatedTime = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120))
            };
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.IsActivated, Is.True);
            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.True);

            householdMemberModel.ActivatedTime = null;
            Assert.That(householdMemberModel.IsActivated, Is.False);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
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

        /// <summary>
        /// Tests that the setter for PrivacyPolicyAcceptedTime sets the value to a given date and time.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyAcceptedTimeSetterSetsValueToDateTime()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.HasAcceptedPrivacyPolicy, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            var newValue = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));

            householdMemberModel.PrivacyPolicyAcceptedTime = newValue;
            Assert.That(householdMemberModel.HasAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.EqualTo(newValue));
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        /// <summary>
        /// Tests that the setter for PrivacyPolicyAcceptedTime sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatPrivacyPolicyAcceptedTimeSetterSetsValueToNull()
        {
            var householdMemberModel = new HouseholdMemberModel
            {
                PrivacyPolicyAcceptedTime = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120))
            };
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.HasAcceptedPrivacyPolicy, Is.True);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);

            householdMemberModel.PrivacyPolicyAcceptedTime = null;
            Assert.That(householdMemberModel.HasAcceptedPrivacyPolicy, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);
        }
    }
}
