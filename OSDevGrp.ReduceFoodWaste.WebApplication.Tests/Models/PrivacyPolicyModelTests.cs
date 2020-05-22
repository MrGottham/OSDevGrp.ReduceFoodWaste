using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using System;

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
            Assert.That(privacyPolicyModel.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(privacyPolicyModel.Header, Is.Null);
            Assert.That(privacyPolicyModel.Body, Is.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that the setter for Identifier sets a new value.
        /// </summary>
        [Test]
        public void TestThatIdentifierSetterSetsValue()
        {
            var privacyPolicyModel = new PrivacyPolicyModel();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.Identifier, Is.EqualTo(default(Guid)));

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(privacyPolicyModel.Identifier));

            privacyPolicyModel.Identifier = newValue;
            Assert.That(privacyPolicyModel.Identifier, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Header sets a new value.
        /// </summary>
        [Test]
        public void TestThatHeaderSetterSetsValue()
        {
            var privacyPolicyModel = new PrivacyPolicyModel();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.Header, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(privacyPolicyModel.Header));

            privacyPolicyModel.Header= newValue;
            Assert.That(privacyPolicyModel.Header, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Body sets a new value.
        /// </summary>
        [Test]
        public void TestThatHeaderBodySetsValue()
        {
            var privacyPolicyModel = new PrivacyPolicyModel();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.Body, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.EqualTo(privacyPolicyModel.Body));

            privacyPolicyModel.Body = newValue;
            Assert.That(privacyPolicyModel.Body, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for IsAccepted sets a new value.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatHeaderIsAcceptedSetsValue(bool newValue)
        {
            var privacyPolicyModel = new PrivacyPolicyModel
            {
                IsAccepted = !newValue
            };
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.Not.EqualTo(newValue));

            privacyPolicyModel.IsAccepted = newValue;
            Assert.That(privacyPolicyModel.IsAccepted, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for AcceptedTime sets the value to a given date and time.
        /// </summary>
        [Test]
        public void TestThatAcceptedTimeSetterSetsValueToDateTime()
        {
            var privacyPolicyModel = new PrivacyPolicyModel();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var newValue = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));

            privacyPolicyModel.AcceptedTime = newValue;
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.EqualTo(newValue));
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        /// <summary>
        /// Tests that the setter for AcceptedTime sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatAcceptedTimeSetterSetsValueToNull()
        {
            var privacyPolicyModel = new PrivacyPolicyModel
            {
                AcceptedTime = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120))
            };
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);

            privacyPolicyModel.AcceptedTime = null;
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that Clone makes a clone of the model for the privacy policies.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCloneClonesPrivacyPolicyModel(bool isAccepted)
        {
            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.IsAccepted, isAccepted)
                .With(m => m.AcceptedTime, isAccepted ? DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.Identifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(privacyPolicyModel.Header, Is.Not.Null);
            Assert.That(privacyPolicyModel.Header, Is.Not.Empty);
            Assert.That(privacyPolicyModel.Body, Is.Not.Null);
            Assert.That(privacyPolicyModel.Body, Is.Not.Empty);
            Assert.That(privacyPolicyModel.IsAccepted, Is.EqualTo(isAccepted));
            if (isAccepted)
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
            }

            var clone = (PrivacyPolicyModel) privacyPolicyModel.Clone();
            Assert.That(clone, Is.Not.Null);
            Assert.That(clone, Is.Not.SameAs(privacyPolicyModel));
            Assert.That(clone.Identifier, Is.EqualTo(privacyPolicyModel.Identifier));
            Assert.That(clone.Header, Is.Not.Null);
            Assert.That(clone.Header, Is.Not.Empty);
            Assert.That(clone.Header, Is.EqualTo(privacyPolicyModel.Header));
            Assert.That(clone.Body, Is.Not.Null);
            Assert.That(clone.Body, Is.Not.Empty);
            Assert.That(clone.Body, Is.EqualTo(privacyPolicyModel.Body));
            Assert.That(clone.IsAccepted, Is.False);
            Assert.That(clone.AcceptedTime, Is.Null);
            Assert.That(clone.AcceptedTime.HasValue, Is.False);
        }
    }
}
