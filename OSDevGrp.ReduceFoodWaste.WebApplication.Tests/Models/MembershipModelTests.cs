﻿using System.Globalization;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

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
            Assert.That(membershipModel.Price, Is.EqualTo(0M));
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(CultureInfo.CurrentUICulture));
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
        /// Tests that the getter for Description converts the price tag.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatDescriptionGetterConvertsPriceTag(string cultureName)
        {
            var description = string.Format("{0}[Price]{1}", Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(description, Is.Not.Null);
            Assert.That(description, Is.Not.Empty);
            Assert.That(description.Contains("[Price]"), Is.True);

            var price = Fixture.Create<decimal>();

            var priceCultureInfo = new CultureInfo(cultureName);
            Assert.That(priceCultureInfo, Is.Not.Null);

            var expectedDescription = description.Replace("[Price]", price.ToString("C", priceCultureInfo));
            Assert.That(expectedDescription, Is.Not.Null);
            Assert.That(expectedDescription, Is.Not.Empty);
            Assert.That(expectedDescription.Contains("[Price]"), Is.False);

            var membershipModel = new MembershipModel
            {
                Description = description,
                Price = price,
                PriceCultureInfo = priceCultureInfo
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Empty);
            Assert.That(membershipModel.Description, Is.EqualTo(expectedDescription));
            Assert.That(membershipModel.Price, Is.EqualTo(price));
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(priceCultureInfo));
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
        /// Tests that the setter for Price sets a new value.
        /// </summary>
        [Test]
        public void TestThatPriceSetterSetsValue()
        {
            var membershipModel = new MembershipModel();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Price, Is.EqualTo(0M));

            var newValue = Fixture.Create<decimal>();
            Assert.That(newValue, Is.Not.EqualTo(0M));

            membershipModel.Price = newValue;
            Assert.That(membershipModel.Price, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Price sets the value to 0.
        /// </summary>
        [Test]
        public void TestThatPriceSetterSetsValueToZero()
        {
            var membershipModel = new MembershipModel
            {
                Price = Fixture.Create<decimal>()
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Price, Is.Not.EqualTo(0M));

            membershipModel.Price = 0M;
            Assert.That(membershipModel.Price, Is.EqualTo(0M));
        }

        /// <summary>
        /// Tests that the setter for PriceCultureInfo sets a new value.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatPriceCultureInfoSetterSetsValue(string cultureName)
        {
            var membershipModel = new MembershipModel();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(CultureInfo.CurrentUICulture));

            var newValue = new CultureInfo(cultureName);
            Assert.That(newValue, Is.Not.Null);

            membershipModel.PriceCultureInfo = newValue;
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for PriceCultureInfo sets the value to NULL.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatPriceCultureInfoSetterSetsValueToNull(string cultureName)
        {
            var cultureInfo = new CultureInfo(cultureName);
            Assert.That(cultureInfo, Is.Not.Null);

            var membershipModel = new MembershipModel
            {
                PriceCultureInfo = cultureInfo
            };
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(cultureInfo));

            membershipModel.PriceCultureInfo = null;
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfo, Is.EqualTo(CultureInfo.CurrentUICulture));
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