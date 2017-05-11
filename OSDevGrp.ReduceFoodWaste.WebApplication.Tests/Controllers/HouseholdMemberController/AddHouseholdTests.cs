using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers.HouseholdMemberController
{
    /// <summary>
    /// Tests the AddHousehold methods controller for a household member.
    /// </summary>
    public class AddHouseholdTests : TestBase
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;
        private IClaimValueProvider _claimValueProviderMock;
        private ILocalClaimProvider _localClaimProviderMock;
        private IModelHelper _modelHelperMock;
        private IUtilities _utilitiesMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            _claimValueProviderMock = MockRepository.GenerateMock<IClaimValueProvider>();
            _localClaimProviderMock = MockRepository.GenerateMock<ILocalClaimProvider>();
            _modelHelperMock = MockRepository.GenerateMock<IModelHelper>();
            _utilitiesMock = MockRepository.GenerateMock<IUtilities>();
        }

        /// <summary>
        /// Tests that AddHousehold without a status message returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatAddHouseholdWithoutStatusMessageReturnsViewResultWithModelForAddingNewHousehold(string statusMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            ActionResult result = householdMemberController.AddHousehold(statusMessage: statusMessage);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result);
        }

        /// <summary>
        /// Tests that AddHousehold with a status message returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdWithStatusMessageReturnsViewResultWithModelForAddingNewHousehold()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            ActionResult result = householdMemberController.AddHousehold(statusMessage: statusMessage);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result, new Dictionary<string, string> {{"StatusMessage", statusMessage}});
        }

        /// <summary>
        /// Tests that AddHousehold without an error message returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatAddHouseholdWithoutErrorMessageReturnsViewResultWithModelForAddingNewHousehold(string errorMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            ActionResult result = householdMemberController.AddHousehold(errorMessage: errorMessage);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result);
        }

        /// <summary>
        /// Tests that AddHousehold with an error message returns a ViewResult with a model for adding a new household.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdWithErrorMessageReturnsViewResultWithModelForAddingNewHousehold()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            ActionResult result = householdMemberController.AddHousehold(errorMessage: errorMessage);

            AssertThatActionResultIsViewResultForAddingNewHousehold(result, new Dictionary<string, string> {{"ErrorMessage", errorMessage}});
        }

        private WebApplication.Controllers.HouseholdMemberController CreateHouseholdMemberController()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = new WebApplication.Controllers.HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock, _modelHelperMock, _utilitiesMock);
            householdMemberController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdMemberController);
            return householdMemberController;
        }

        private static void AssertThatActionResultIsViewResultForAddingNewHousehold(ActionResult actionResult, IDictionary<string, string> viewData = null)
        {
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) actionResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("AddHousehold"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            if (viewData == null)
            {
                Assert.That(viewResult.ViewData, Is.Not.Null);
                Assert.That(viewResult.ViewData, Is.Empty);
            }
            else
            {
                Assert.That(viewResult.ViewData, Is.Not.Null);
                Assert.That(viewResult.ViewData, Is.Not.Empty);
                foreach (KeyValuePair<string, string> viewDataElement in viewData)
                {
                    Assert.That(viewResult.ViewData[viewDataElement.Key], Is.Not.Null);
                    Assert.That(viewResult.ViewData[viewDataElement.Key], Is.Not.Empty);
                    Assert.That(viewResult.ViewData[viewDataElement.Key], Is.EqualTo(viewDataElement.Value));
                }
            }

            HouseholdModel householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel.Name, Is.Null);
            Assert.That(householdModel.Description, Is.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Null);
        }
    }
}
