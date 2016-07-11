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

            var householdMemberModel = Fixture.Create<HouseholdMemberModel>();
            Assert.That(householdMemberModel, Is.Not.Null);

            dashboardModel.HouseholdMember = householdMemberModel;
            Assert.That(dashboardModel.HouseholdMember, Is.Not.Null);
            Assert.That(dashboardModel.HouseholdMember, Is.EqualTo(householdMemberModel));
        }

        /// <summary>
        /// Test that the setter of HouseholdMember sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatHouseholdMemberSetterSetsValueToNull()
        {
            var dashboardModel = new DashboardModel
            {
                HouseholdMember = Fixture.Create<HouseholdMemberModel>()
            };
            Assert.That(dashboardModel, Is.Not.Null);
            Assert.That(dashboardModel.HouseholdMember, Is.Not.Null);

            dashboardModel.HouseholdMember = null;
            Assert.That(dashboardModel.HouseholdMember, Is.Null);
        }
    }
}
