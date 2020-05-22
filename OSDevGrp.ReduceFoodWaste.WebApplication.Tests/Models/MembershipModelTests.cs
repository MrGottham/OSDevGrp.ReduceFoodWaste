using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models.Enums;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using System.Globalization;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a membership.
    /// </summary>
    [TestFixture]
    public class MembershipModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a membership.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeMembershipModel()
        {
            var membershipModel = new MembershipModel();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Null);
            Assert.That(membershipModel.Description, Is.Null);
            Assert.That(membershipModel.BillingInformation, Is.Null);
            Assert.That(membershipModel.Price, Is.EqualTo(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(CultureInfo.CurrentUICulture));
            Assert.That(membershipModel.IsFreeOfCost, Is.True);
            Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(membershipModel.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(membershipModel.PaymentHandlers, Is.Null);
            Assert.That(membershipModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(membershipModel.PaymentTime, Is.Null);
            Assert.That(membershipModel.PaymentTime.HasValue, Is.False);
            Assert.That(membershipModel.PaymentReference, Is.Null);
            Assert.That(membershipModel.PaymentReceipt, Is.Null);
            Assert.That(membershipModel.CanRenew, Is.False);
            Assert.That(membershipModel.CanUpgrade, Is.False);
        }

        /// <summary>
        /// Tests that the setter for Name sets a new value.
        /// </summary>
        [Test]
        public void TestThatNameSetterSetsValue()
        {
            var membershipModel = new MembershipModel();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            membershipModel.Name = newValue;
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Name sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatNameSetterSetsValueToNull()
        {
            var membershipModel = new MembershipModel
            {
                Name = Fixture.Create<string>()
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);

            membershipModel.Name = null;
            Assert.That(membershipModel.Name, Is.Null);
        }

        /// <summary>
        /// Tests that the getter for Description converts the name tag.
        /// </summary>
        [Test]
        public void TestThatDescriptionGetterConvertsNameTag()
        {
            var description = $"{Fixture.Create<string>()}[Name]{Fixture.Create<string>()}";
            Assert.That(description, Is.Not.Null);
            Assert.That(description, Is.Not.Empty);
            Assert.That(description.Contains("[Name]"), Is.True);

            var name = Fixture.Create<string>();
            Assert.That(name, Is.Not.Null);
            Assert.That(name, Is.Not.Empty);

            var expectedDescription = description.Replace("[Name]", name);
            Assert.That(expectedDescription, Is.Not.Null);
            Assert.That(expectedDescription, Is.Not.Empty);
            Assert.That(expectedDescription.Contains("[Name]"), Is.False);

            var membershipModel = new MembershipModel
            {
                Name = name,
                Description = description,
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(name));
            Assert.That(membershipModel.Description, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Empty);
            Assert.That(membershipModel.Description, Is.EqualTo(expectedDescription));
        }

        /// <summary>
        /// Tests that the getter for Description converts the price tag.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatDescriptionGetterConvertsPriceTag(string cultureName)
        {
            var description = $"{Fixture.Create<string>()}[Price]{Fixture.Create<string>()}";
            Assert.That(description, Is.Not.Null);
            Assert.That(description, Is.Not.Empty);
            Assert.That(description.Contains("[Price]"), Is.True);

            var price = Fixture.Create<decimal>();

            var expectedDescription = description.Replace("[Price]", price.ToString("C", new CultureInfo(cultureName)));
            Assert.That(expectedDescription, Is.Not.Null);
            Assert.That(expectedDescription, Is.Not.Empty);
            Assert.That(expectedDescription.Contains("[Price]"), Is.False);

            var membershipModel = new MembershipModel
            {
                Description = description,
                Price = price,
                PriceCultureInfoName = cultureName
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Empty);
            Assert.That(membershipModel.Description, Is.EqualTo(expectedDescription));
            Assert.That(membershipModel.Price, Is.EqualTo(price));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(cultureName));
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(new CultureInfo(cultureName)));
        }

        /// <summary>
        /// Tests that the setter for Description sets a new value.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterSetsValue()
        {
            var membershipModel = new MembershipModel();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            membershipModel.Description = newValue;
            Assert.That(membershipModel.Description, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Empty);
            Assert.That(membershipModel.Description, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Description sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatDescriptionSetterSetsValueToNull()
        {
            var membershipModel = new MembershipModel
            {
                Description = Fixture.Create<string>()
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Empty);

            membershipModel.Description = null;
            Assert.That(membershipModel.Description, Is.Null);
        }

        /// <summary>
        /// Tests that the getter for BillingInformation converts the name tag.
        /// </summary>
        [Test]
        public void TestThatBillingInformationGetterConvertsNameTag()
        {
            var billingInformation = $"{Fixture.Create<string>()}[Name]{Fixture.Create<string>()}";
            Assert.That(billingInformation, Is.Not.Null);
            Assert.That(billingInformation, Is.Not.Empty);
            Assert.That(billingInformation.Contains("[Name]"), Is.True);

            var name = Fixture.Create<string>();
            Assert.That(name, Is.Not.Null);
            Assert.That(name, Is.Not.Empty);

            var expectedBillingInformation = billingInformation.Replace("[Name]", name);
            Assert.That(expectedBillingInformation, Is.Not.Null);
            Assert.That(expectedBillingInformation, Is.Not.Empty);
            Assert.That(expectedBillingInformation.Contains("[Name]"), Is.False);

            var membershipModel = new MembershipModel
            {
                Name = name,
                BillingInformation = billingInformation,
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(name));
            Assert.That(membershipModel.BillingInformation, Is.Not.Null);
            Assert.That(membershipModel.BillingInformation, Is.Not.Empty);
            Assert.That(membershipModel.BillingInformation, Is.EqualTo(expectedBillingInformation));
        }

        /// <summary>
        /// Tests that the getter for BillingInformation converts the price tag.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatBillingInformationGetterConvertsPriceTag(string cultureName)
        {
            var billingInformation = $"{Fixture.Create<string>()}[Price]{Fixture.Create<string>()}";
            Assert.That(billingInformation, Is.Not.Null);
            Assert.That(billingInformation, Is.Not.Empty);
            Assert.That(billingInformation.Contains("[Price]"), Is.True);

            var price = Fixture.Create<decimal>();

            var expectedBillingInformation = billingInformation.Replace("[Price]", price.ToString("C", new CultureInfo(cultureName)));
            Assert.That(expectedBillingInformation, Is.Not.Null);
            Assert.That(expectedBillingInformation, Is.Not.Empty);
            Assert.That(expectedBillingInformation.Contains("[Price]"), Is.False);

            var membershipModel = new MembershipModel
            {
                BillingInformation = billingInformation,
                Price = price,
                PriceCultureInfoName = cultureName
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.BillingInformation, Is.Not.Null);
            Assert.That(membershipModel.BillingInformation, Is.Not.Empty);
            Assert.That(membershipModel.BillingInformation, Is.EqualTo(expectedBillingInformation));
            Assert.That(membershipModel.Price, Is.EqualTo(price));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(cultureName));
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(new CultureInfo(cultureName)));
        }

        /// <summary>
        /// Tests that the setter for BillingInformation sets a new value.
        /// </summary>
        [Test]
        public void TestThatBillingInformationSetterSetsValue()
        {
            var membershipModel = new MembershipModel();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.BillingInformation, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            membershipModel.BillingInformation = newValue;
            Assert.That(membershipModel.BillingInformation, Is.Not.Null);
            Assert.That(membershipModel.BillingInformation, Is.Not.Empty);
            Assert.That(membershipModel.BillingInformation, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for BillingInformation sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatBillingInformationSetterSetsValueToNull()
        {
            var membershipModel = new MembershipModel
            {
                BillingInformation = Fixture.Create<string>()
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.BillingInformation, Is.Not.Null);
            Assert.That(membershipModel.BillingInformation, Is.Not.Empty);

            membershipModel.BillingInformation = null;
            Assert.That(membershipModel.BillingInformation, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for CanRenew sets a new value.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCanRenewSetterSetsValue(bool newValue)
        {
            var membershipModel = new MembershipModel
            {
                CanRenew = !newValue
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.CanRenew, Is.Not.EqualTo(newValue));

            membershipModel.CanRenew = newValue;
            Assert.That(membershipModel.CanRenew, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for CanUpgrade sets a new value.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCanUpgradeSetterSetsValue(bool newValue)
        {
            var membershipModel = new MembershipModel
            {
                CanUpgrade = !newValue
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.CanUpgrade, Is.Not.EqualTo(newValue));

            membershipModel.CanUpgrade = newValue;
            Assert.That(membershipModel.CanUpgrade, Is.EqualTo(newValue));
        }
    }
}
