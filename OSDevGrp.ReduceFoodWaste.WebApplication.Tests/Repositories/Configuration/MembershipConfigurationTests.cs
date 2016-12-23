using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories.Configuration
{
    /// <summary>
    /// Tests the configuration for memberships.
    /// </summary>
    [TestFixture]
    public class MembershipConfigurationTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the configuration for memberships-
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeMembershipConfiguration()
        {
            IMembershipConfiguration membershipConfiguration = MembershipConfiguration.Create();
            Assert.That(membershipConfiguration, Is.Not.Null);

            IMembershipElement basicMembership = membershipConfiguration.BasicMembership;
            Assert.That(basicMembership, Is. Not.Null);
            Assert.That(basicMembership.Name, Is.Not.Null);
            Assert.That(basicMembership.Name, Is.Not.Empty);
            Assert.That(basicMembership.Name, Is.EqualTo("Basic"));
            Assert.That(basicMembership.Pricing, Is.Not.Null);
            Assert.That(basicMembership.Pricing, Is.Not.Empty);
            Assert.That(basicMembership.Pricing.SingleOrDefault(m => string.Compare(m.Key.Name, new CultureInfo("da-DK").Name, StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(basicMembership.Pricing.Single(m => string.Compare(m.Key.Name, new CultureInfo("da-DK").Name, StringComparison.Ordinal) == 0).Value, Is.EqualTo(0M));
            Assert.That(basicMembership.Pricing.SingleOrDefault(m => string.Compare(m.Key.Name, new CultureInfo("en-Us").Name, StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(basicMembership.Pricing.Single(m => string.Compare(m.Key.Name, new CultureInfo("en-Us").Name, StringComparison.Ordinal) == 0).Value, Is.EqualTo(0M));

            IMembershipElement deluxeMembership = membershipConfiguration.DeluxeMembership;
            Assert.That(deluxeMembership, Is.Not.Null);
            Assert.That(deluxeMembership.Name, Is.Not.Null);
            Assert.That(deluxeMembership.Name, Is.Not.Empty);
            Assert.That(deluxeMembership.Name, Is.EqualTo("Deluxe"));
            Assert.That(deluxeMembership.Pricing, Is.Not.Null);
            Assert.That(deluxeMembership.Pricing, Is.Not.Empty);
            Assert.That(deluxeMembership.Pricing.SingleOrDefault(m => string.Compare(m.Key.Name, new CultureInfo("da-DK").Name, StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(deluxeMembership.Pricing.Single(m => string.Compare(m.Key.Name, new CultureInfo("da-DK").Name, StringComparison.Ordinal) == 0).Value, Is.EqualTo(15M));
            Assert.That(deluxeMembership.Pricing.SingleOrDefault(m => string.Compare(m.Key.Name, new CultureInfo("en-Us").Name, StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(deluxeMembership.Pricing.Single(m => string.Compare(m.Key.Name, new CultureInfo("en-Us").Name, StringComparison.Ordinal) == 0).Value, Is.EqualTo(3M));

            IMembershipElement premiumMembership = membershipConfiguration.PremiumMembership;
            Assert.That(premiumMembership, Is.Not.Null);
            Assert.That(premiumMembership.Name, Is.Not.Null);
            Assert.That(premiumMembership.Name, Is.Not.Empty);
            Assert.That(premiumMembership.Name, Is.EqualTo("Premium"));
            Assert.That(premiumMembership.Pricing, Is.Not.Null);
            Assert.That(premiumMembership.Pricing, Is.Not.Empty);
            Assert.That(premiumMembership.Pricing.SingleOrDefault(m => string.Compare(m.Key.Name, new CultureInfo("da-DK").Name, StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(premiumMembership.Pricing.Single(m => string.Compare(m.Key.Name, new CultureInfo("da-DK").Name, StringComparison.Ordinal) == 0).Value, Is.EqualTo(25M));
            Assert.That(premiumMembership.Pricing.SingleOrDefault(m => string.Compare(m.Key.Name, new CultureInfo("en-Us").Name, StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(premiumMembership.Pricing.Single(m => string.Compare(m.Key.Name, new CultureInfo("en-Us").Name, StringComparison.Ordinal) == 0).Value, Is.EqualTo(5M));
        }
    }
}
