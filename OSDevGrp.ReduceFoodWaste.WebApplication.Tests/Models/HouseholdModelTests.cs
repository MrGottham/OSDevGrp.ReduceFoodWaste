using System;
using System.Collections.Generic;
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
            Assert.That(householdModel.CreationTime, Is.EqualTo(default(DateTime)));
            Assert.That(householdModel.HouseholdMembers, Is.Null);
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

        /// <summary>
        /// Tests that the setter for CreationTime sets the value to a given date and time.
        /// </summary>
        [Test]
        public void TestThatCreationTimeSetterSetsValueToDateTime()
        {
            var householdModel = new HouseholdModel();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.CreationTime, Is.EqualTo(default(DateTime)));

            var newValue = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));
            Assert.That(newValue, Is.Not.EqualTo(householdModel.CreationTime));

            householdModel.CreationTime = newValue;
            Assert.That(householdModel.CreationTime, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for HouseholdMembers sets the value to a collection of household models.
        /// </summary>
        [Test]
        public void TestThatHouseholdMembersSetterSetsValueToHouseholdMembersModelCollection()
        {
            var householdModel = new HouseholdModel();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.HouseholdMembers, Is.Null);

            var householdMemberModelCollection = new List<MemberOfHouseholdModel>(Random.Next(5, 10));
            while (householdMemberModelCollection.Count < householdMemberModelCollection.Capacity)
            {
                var memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                    .With(m => m.HouseholdMemberIdentifier, Guid.NewGuid())
                    .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                    .With(m => m.MailAddress, Fixture.Create<string>())
                    .With(m => m.Removable, Fixture.Create<bool>())
                    .Create();
                householdMemberModelCollection.Add(memberOfHouseholdModel);
            }
            
            householdModel.HouseholdMembers = householdMemberModelCollection;
            Assert.That(householdModel.HouseholdMembers, Is.Not.Null);
            Assert.That(householdModel.HouseholdMembers, Is.Not.Empty);
            Assert.That(householdModel.HouseholdMembers, Is.EqualTo(householdMemberModelCollection));
        }

        /// <summary>
        /// Tests that the setter for HouseholdMembers sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatHouseholdMembersSetterSetsValueToNull()
        {
            var householdMemberModelCollection = new List<MemberOfHouseholdModel>(Random.Next(5, 10));
            while (householdMemberModelCollection.Count < householdMemberModelCollection.Capacity)
            {
                var memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                    .With(m => m.HouseholdMemberIdentifier, Guid.NewGuid())
                    .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                    .With(m => m.MailAddress, Fixture.Create<string>())
                    .With(m => m.Removable, Fixture.Create<bool>())
                    .Create();
                householdMemberModelCollection.Add(memberOfHouseholdModel);
            }

            var householdModel = new HouseholdModel
            {
                HouseholdMembers = householdMemberModelCollection
            };
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.HouseholdMembers, Is.Not.Null);
            Assert.That(householdModel.HouseholdMembers, Is.Not.Empty);

            householdModel.HouseholdMembers = null;
            Assert.That(householdModel.HouseholdMembers, Is.Null);
        }
    }
}
