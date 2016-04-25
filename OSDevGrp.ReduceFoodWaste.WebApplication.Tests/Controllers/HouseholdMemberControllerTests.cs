using System;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Tests the controller for a household member.
    /// </summary>
    public class HouseholdMemberControllerTests : TestBase
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
        }

        /// <summary>
        /// Tests that the constructor initialize the controller for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberController()
        {
            var householdMemberController = new HouseholdMemberController(_householdDataRepositoryMock);
            Assert.That(householdMemberController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberController(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Create calls GetPrivacyPoliciesAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatCreateCallsGetPrivacyPoliciesAsyncOnHouseholdDataRepository()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdMemberController.Create();

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetPrivacyPoliciesAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Create returns a ViewResult with a model for creating a new household member.
        /// </summary>
        [Test]
        public void TestThatCreateReturnsViewResultWithModelForCreatingHouseholdMember()
        {
            var privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            var householdMemberController = CreateHouseholdMemberController(privacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var result = householdMemberController.Create();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Create"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            var model = (HouseholdModel) viewResult.Model;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(model.Name, Is.Null);
            Assert.That(model.Description, Is.Null);
            Assert.That(model.PrivacyPolicy, Is.Not.Null);
            Assert.That(model.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
        }

        /// <summary>
        /// Tests that Prepare throws NotImplementedException.
        /// </summary>
        [Test]
        public void TestThatPrepareThrowsNotImplementedException()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.Throws<NotImplementedException>(() => householdMemberController.Prepare());
        }

        /// <summary>
        /// Creates a controller for a household member for unit testing.
        /// </summary>
        /// <param name="privacyPolicyModel">Sets the privacy policy model which should be used by the controller.</param>
        /// <returns>Controller for a household member for unit testing.</returns>
        private HouseholdMemberController CreateHouseholdMemberController(PrivacyPolicyModel privacyPolicyModel = null)
        {
            _householdDataRepositoryMock.Stub(m => m.GetPrivacyPoliciesAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(() => privacyPolicyModel ?? Fixture.Create<PrivacyPolicyModel>()))
                .Repeat.Any();

            var householdMemberController = new HouseholdMemberController(_householdDataRepositoryMock);
            householdMemberController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdMemberController);
            return householdMemberController;
        }
    }
}
