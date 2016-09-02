using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a given household member who are a member of a given household.
    /// </summary>
    [TestFixture]
    public class MemberOfHouseholdModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a given household member who are a member of a given household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeMemberOfHouseholdModel()
        {
            var memberOfHouseholdModel = new MemberOfHouseholdModel();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.HouseholdMemberIdentifier, Is.EqualTo(default(Guid)));
            Assert.That(memberOfHouseholdModel.HouseholdIdentifier, Is.EqualTo(default(Guid)));
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Null);
            Assert.That(memberOfHouseholdModel.Removable, Is.False);
        }

        /// <summary>
        /// Tests that the setter for HouseholdMemberIdentifier sets a new value.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberIdentifierSetterSetsValue()
        {
            var memberOfHouseholdModel = new MemberOfHouseholdModel();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.HouseholdMemberIdentifier, Is.EqualTo(default(Guid)));

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(memberOfHouseholdModel.HouseholdMemberIdentifier));

            memberOfHouseholdModel.HouseholdMemberIdentifier = newValue;
            Assert.That(memberOfHouseholdModel.HouseholdMemberIdentifier, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for HouseholdIdentifier sets a new value.
        /// </summary>
        [Test]
        public void TestThatHouseholdIdentifierSetterSetsValue()
        {
            var memberOfHouseholdModel = new MemberOfHouseholdModel();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.HouseholdIdentifier, Is.EqualTo(default(Guid)));

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(memberOfHouseholdModel.HouseholdIdentifier));

            memberOfHouseholdModel.HouseholdIdentifier = newValue;
            Assert.That(memberOfHouseholdModel.HouseholdIdentifier, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for MailAddress sets the value to a given mail address.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterSetsValueToMailAddress()
        {
            var memberOfHouseholdModel = new MemberOfHouseholdModel();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(memberOfHouseholdModel.MailAddress));

            memberOfHouseholdModel.MailAddress = newValue;
            Assert.That(memberOfHouseholdModel.MailAddress, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for MailAddress sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatMailAddressSetterSetsValueToNull()
        {
            var memberOfHouseholdModel = new MemberOfHouseholdModel
            {
                MailAddress = Fixture.Create<string>()
            };
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Not.Null);

            const string newValue = null;
            Assert.That(newValue, Is.Not.EqualTo(memberOfHouseholdModel.MailAddress));

            memberOfHouseholdModel.MailAddress = newValue;
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Removable sets a new value.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatRemovableSetterSetsValue(bool newValue)
        {
            var memberOfHouseholdModel = new MemberOfHouseholdModel
            {
                Removable = !newValue
            };
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.Removable, Is.Not.EqualTo(newValue));

            memberOfHouseholdModel.Removable = newValue;
            Assert.That(memberOfHouseholdModel.Removable, Is.EqualTo(newValue));
        }
    }
}
