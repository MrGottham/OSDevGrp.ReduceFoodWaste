using System.Linq;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a dashboard.
    /// </summary>
    [TestFixture]
    public class DashboardModelTest : TestBase
    {
        /// <summary>
        /// Test that the constructor initialize a model for a dashboard.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDashboardModel()
        {
            var dashboardModel = new DashboardModel();
            Assert.That(dashboardModel, Is.Not.Null);
            Assert.That(dashboardModel.HouseholdMember, Is.Null);
            Assert.That(dashboardModel.Households, Is.Null);
        }

        /// <summary>
        /// Test that the setter of HouseholdMember sets the value to a model for a household member.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberSetterSetsValueToHouseholdMemberModel()
        {
            var dashboardModel = new DashboardModel();
            Assert.That(dashboardModel, Is.Not.Null);
            Assert.That(dashboardModel.HouseholdMember, Is.Null);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Households, Fixture.CreateMany<HouseholdModel>(Random.Next(1, 5)).ToList())
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.Households, Is.Not.Null);
            Assert.That(householdMemberModel.Households, Is.Not.Empty);

            dashboardModel.HouseholdMember = householdMemberModel;
            Assert.That(dashboardModel.HouseholdMember, Is.Not.Null);
            Assert.That(dashboardModel.HouseholdMember, Is.EqualTo(householdMemberModel));
            Assert.That(dashboardModel.Households, Is.Not.Null);
            Assert.That(dashboardModel.Households, Is.Not.Empty);
            Assert.That(dashboardModel.Households, Is.EqualTo(householdMemberModel.Households));
        }

        /// <summary>
        /// Test that the setter of HouseholdMember sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberSetterSetsValueToNull()
        {
            var dashboardModel = new DashboardModel
            {
                HouseholdMember = Fixture.Build<HouseholdMemberModel>()
                    .With(m => m.Households, Fixture.CreateMany<HouseholdModel>(Random.Next(1, 5)).ToList())
                    .Create()
            };
            Assert.That(dashboardModel, Is.Not.Null);
            Assert.That(dashboardModel.HouseholdMember, Is.Not.Null);
            Assert.That(dashboardModel.Households, Is.Not.Null);
            Assert.That(dashboardModel.Households, Is.Not.Empty);

            dashboardModel.HouseholdMember = null;
            Assert.That(dashboardModel.HouseholdMember, Is.Null);
            Assert.That(dashboardModel.Households, Is.Null);
        }
    }
}
