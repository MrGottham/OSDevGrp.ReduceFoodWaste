using System;
using System.Linq;
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
            Assert.That(householdMemberModel.Name, Is.Null);
            Assert.That(householdMemberModel.MailAddress, Is.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            Assert.That(householdMemberModel.IsActivated, Is.False);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.Membership, Is.Null);
            Assert.That(householdMemberModel.MembershipExpireTime, Is.Null);
            Assert.That(householdMemberModel.MembershipExpireTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Null);
            Assert.That(householdMemberModel.HasAcceptedPrivacyPolicy, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.CreationTime, Is.EqualTo(default(DateTime)));
            Assert.That(householdMemberModel.Households, Is.Null);
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
        /// Tests that the setter for Name sets the value to a given name.
        /// </summary>
        [Test]
        public void TestThatNameSetterSetsValueToName()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Name, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.Name));

            householdMemberModel.Name = newValue;
            Assert.That(householdMemberModel.Name, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Name sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatNameSetterSetsValueToNull()
        {
            var householdMemberModel = new HouseholdMemberModel
            {
                Name = Fixture.Create<string>()
            };
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Name, Is.Not.Null);

            const string newValue = null;
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.Name));

            householdMemberModel.Name = newValue;
            Assert.That(householdMemberModel.Name, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for MailAddress sets the value to a given mail address.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterSetsValueToMailAddress()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.MailAddress, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.MailAddress));

            householdMemberModel.MailAddress = newValue;
            Assert.That(householdMemberModel.MailAddress, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for MailAddress sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterSetsValueToNull()
        {
            var householdMemberModel = new HouseholdMemberModel
            {
                MailAddress = Fixture.Create<string>()
            };
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.MailAddress, Is.Not.Null);

            const string newValue = null;
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.MailAddress));

            householdMemberModel.MailAddress = newValue;
            Assert.That(householdMemberModel.MailAddress, Is.Null);
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
        /// Tests that the setter for Membership sets the value to a given name of the household members membership.
        /// </summary>
        [Test]
        public void TestThatMembershipSetterSetsValueToMembership()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Membership, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.Membership));

            householdMemberModel.Membership = newValue;
            Assert.That(householdMemberModel.Membership, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Membership sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatMembershipSetterSetsValueToNull()
        {
            var householdMemberModel = new HouseholdMemberModel
            {
                Membership = Fixture.Create<string>()
            };
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Membership, Is.Not.Null);

            const string newValue = null;
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.Membership));

            householdMemberModel.Membership = newValue;
            Assert.That(householdMemberModel.Membership, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for MembershipExpireTime sets the value to a given date and time.
        /// </summary>
        [Test]
        public void TestThatMembershipExpireTimeSetterSetsValueToDateTime()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.MembershipExpireTime, Is.Null);
            Assert.That(householdMemberModel.MembershipExpireTime.HasValue, Is.False);

            var newValue = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));

            householdMemberModel.MembershipExpireTime = newValue;
            Assert.That(householdMemberModel.MembershipExpireTime, Is.Not.Null);
            Assert.That(householdMemberModel.MembershipExpireTime, Is.EqualTo(newValue));
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(householdMemberModel.MembershipExpireTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        /// <summary>
        /// Tests that the setter for MembershipExpireTime sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatMembershipExpireTimeSetterSetsValueToNull()
        {
            var householdMemberModel = new HouseholdMemberModel
            {
                MembershipExpireTime = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120))
            };
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.MembershipExpireTime, Is.Not.Null);
            Assert.That(householdMemberModel.MembershipExpireTime.HasValue, Is.True);

            householdMemberModel.MembershipExpireTime = null;
            Assert.That(householdMemberModel.MembershipExpireTime, Is.Null);
            Assert.That(householdMemberModel.MembershipExpireTime.HasValue, Is.False);
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

        /// <summary>
        /// Tests that the setter for CreationTime sets the value to a given date and time.
        /// </summary>
        [Test]
        public void TestThatCreationTimeSetterSetsValueToDateTime()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.CreationTime, Is.EqualTo(default(DateTime)));

            var newValue = DateTime.Now.AddDays(Random.Next(1, 7) * -1).AddMinutes(Random.Next(-120, 120));
            Assert.That(newValue, Is.Not.EqualTo(householdMemberModel.CreationTime));

            householdMemberModel.CreationTime = newValue;
            Assert.That(householdMemberModel.CreationTime, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Households sets the value to a collection of household models.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterSetsValueToHouseholdModelCollection()
        {
            var householdMemberModel = new HouseholdMemberModel();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Households, Is.Null);

            var newValue = Fixture.CreateMany<HouseholdModel>(Random.Next(5, 10)).ToList();

            householdMemberModel.Households = newValue;
            Assert.That(householdMemberModel.Households, Is.Not.Null);
            Assert.That(householdMemberModel.Households, Is.Not.Empty);
            Assert.That(householdMemberModel.Households, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Households sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatHouseholdsSetterSetsValueToNull()
        {
            var householdMemberModel = new HouseholdMemberModel
            {
                Households = Fixture.CreateMany<HouseholdModel>(Random.Next(5, 10)).ToList()
            };
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Households, Is.Not.Null);
            Assert.That(householdMemberModel.Households, Is.Not.Empty);

            householdMemberModel.Households = null;
            Assert.That(householdMemberModel.Households, Is.Null);
        }
    }
}
