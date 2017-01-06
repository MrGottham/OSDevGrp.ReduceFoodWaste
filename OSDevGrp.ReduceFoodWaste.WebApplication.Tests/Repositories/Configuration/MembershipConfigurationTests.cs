using System;
using System.Collections.Generic;
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
        /// Tests that the constructor initialize the configuration for memberships.
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

            IEnumerable<IMembershipPriceElement> basicMembershipPricing = basicMembership.Pricing.ToList();
            Assert.That(basicMembershipPricing, Is.Not.Empty);
            Assert.That(basicMembershipPricing.SingleOrDefault(m => string.Compare(m.CultureName, "da-DK", StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(basicMembershipPricing.Single(m => string.Compare(m.CultureName, "da-DK", StringComparison.Ordinal) == 0).Price, Is.EqualTo(0M));
            Assert.That(basicMembershipPricing.SingleOrDefault(m => string.Compare(m.CultureName, "en-US", StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(basicMembershipPricing.Single(m => string.Compare(m.CultureName, "en-US", StringComparison.Ordinal) == 0).Price, Is.EqualTo(0M));

            IMembershipElement deluxeMembership = membershipConfiguration.DeluxeMembership;
            Assert.That(deluxeMembership, Is.Not.Null);
            Assert.That(deluxeMembership.Name, Is.Not.Null);
            Assert.That(deluxeMembership.Name, Is.Not.Empty);
            Assert.That(deluxeMembership.Name, Is.EqualTo("Deluxe"));
            Assert.That(deluxeMembership.Pricing, Is.Not.Null);

            IEnumerable<IMembershipPriceElement> deluxeMembershipPricing = deluxeMembership.Pricing.ToList();
            Assert.That(deluxeMembershipPricing, Is.Not.Empty);
            Assert.That(deluxeMembershipPricing.SingleOrDefault(m => string.Compare(m.CultureName, "da-DK", StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(deluxeMembershipPricing.Single(m => string.Compare(m.CultureName, "da-DK", StringComparison.Ordinal) == 0).Price, Is.EqualTo(15M));
            Assert.That(deluxeMembershipPricing.SingleOrDefault(m => string.Compare(m.CultureName, "en-US", StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(deluxeMembershipPricing.Single(m => string.Compare(m.CultureName, "en-US", StringComparison.Ordinal) == 0).Price, Is.EqualTo(3M));

            IMembershipElement premiumMembership = membershipConfiguration.PremiumMembership;
            Assert.That(premiumMembership, Is.Not.Null);
            Assert.That(premiumMembership.Name, Is.Not.Null);
            Assert.That(premiumMembership.Name, Is.Not.Empty);
            Assert.That(premiumMembership.Name, Is.EqualTo("Premium"));
            Assert.That(premiumMembership.Pricing, Is.Not.Null);

            IEnumerable<IMembershipPriceElement> premiumMembershipPricing = premiumMembership.Pricing.ToList();
            Assert.That(premiumMembershipPricing, Is.Not.Empty);
            Assert.That(premiumMembershipPricing.SingleOrDefault(m => string.Compare(m.CultureName, "da-DK", StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(premiumMembershipPricing.Single(m => string.Compare(m.CultureName, "da-DK", StringComparison.Ordinal) == 0).Price, Is.EqualTo(25M));
            Assert.That(premiumMembershipPricing.SingleOrDefault(m => string.Compare(m.CultureName, "en-US", StringComparison.Ordinal) == 0), Is.Not.Null);
            Assert.That(premiumMembershipPricing.Single(m => string.Compare(m.CultureName, "en-US", StringComparison.Ordinal) == 0).Price, Is.EqualTo(5M));
        }
    }
}
