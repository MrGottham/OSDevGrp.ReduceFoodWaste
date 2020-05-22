using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a read only collection of household identifications.
    /// </summary>
    [TestFixture]
    public class HouseholdIdentificationCollectionModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize the model for a read only collection of household identifications.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatConstructorInitializeHouseholdIdentificationCollectionModel(bool householdMemberCanAddHouseholds)
        {
            IList<HouseholdIdentificationModel> householdIdentificationModels = Fixture.CreateMany<HouseholdIdentificationModel>(Random.Next(5, 10)).ToList();
            Assert.That(householdIdentificationModels, Is.Not.Null);
            Assert.That(householdIdentificationModels, Is.Not.Empty);

            HouseholdIdentificationCollectionModel householdIdentificationCollectionModel = new HouseholdIdentificationCollectionModel(householdIdentificationModels, householdMemberCanAddHouseholds);
            Assert.That(householdIdentificationCollectionModel, Is.Not.Null);
            Assert.That(householdIdentificationCollectionModel, Is.Not.Empty);
            Assert.That(householdIdentificationCollectionModel.Count, Is.EqualTo(householdIdentificationCollectionModel.Count));
            foreach (HouseholdIdentificationModel householdIdentificationModel in householdIdentificationModels)
            {
                Assert.That(householdIdentificationCollectionModel.Contains(householdIdentificationModel), Is.True);
            }
            Assert.That(householdIdentificationCollectionModel.HouseholdMemberCanAddHouseholds, Is.EqualTo(householdMemberCanAddHouseholds));
        }
    }
}
