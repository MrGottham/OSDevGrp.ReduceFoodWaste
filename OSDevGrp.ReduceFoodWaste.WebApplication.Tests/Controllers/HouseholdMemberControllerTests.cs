using System;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
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
        private IClaimValueProvider _claimValueProviderMock;
        private ILocalClaimProvider _localClaimProviderMock;

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
        }

        /// <summary>
        /// Tests that the constructor initialize the controller for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberController()
        {
            var householdMemberController = new HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock);
            Assert.That(householdMemberController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberController(null, _claimValueProviderMock, _localClaimProviderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can get values from claims is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenClaimValueProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberController(_householdDataRepositoryMock, null, _localClaimProviderMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("claimValueProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can append local claims to a claims identity is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenLocalClaimProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localClaimProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Create without a model calls GetPrivacyPoliciesAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatCreateWithoutModelCallsGetPrivacyPoliciesAsyncOnHouseholdDataRepository()
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
        /// Tests that Create without a model calls IsPrivacyPoliciesAccepted on the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatCreateWithoutModelCallsIsPrivacyPoliciesAcceptedOnClaimValueProvider()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdMemberController.Create();

            _claimValueProviderMock.AssertWasCalled(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity)));
        }

        /// <summary>
        /// Tests that Create without a model returns a ViewResult with a model for creating a new household member.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateWithoutModelReturnsViewResultWithModelForCreatingHouseholdMember(bool isPrivacyPoliciesAccepted)
        {
            var privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            var householdMemberController = CreateHouseholdMemberController(privacyPolicyModel, isPrivacyPoliciesAccepted);
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
            Assert.That(model.PrivacyPolicy.IsAccepted, Is.EqualTo(isPrivacyPoliciesAccepted));
        }

        /// <summary>
        /// Tests that Create with a model throws an ArgumentNullException when the model is null.
        /// </summary>
        [Test]
        public void TestThatCreateWithModelThrowsArgumentNullExceptionWhenModelIsNull()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.Create(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Create with a model calls GetPrivacyPoliciesAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatCreateWithModelCallsGetPrivacyPoliciesAsyncOnHouseholdDataRepository()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Header, null)
                .With(m => m.Body, null)
                .With(m => m.IsAccepted, true)
                .Create();

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Description, Fixture.Create<string>())
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .Create();

            householdMemberController.Create(householdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetPrivacyPoliciesAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Create with a model updates Identifier, Header and Body in the privacy policy model with values from the reloaded privacy policy model.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateWithModelUpdatesValuesInPrivacyPolicyModelWithValuesFromReloadedPrivacyPolicyModel(bool isPrivacyPoliciesAccepted)
        {
            var reloadedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Header, Fixture.Create<string>())
                .With(m => m.Body, Fixture.Create<string>())
                .Create();
            Assert.That(reloadedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(reloadedPrivacyPolicyModel.Identifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(reloadedPrivacyPolicyModel.Header, Is.Not.Null);
            Assert.That(reloadedPrivacyPolicyModel.Header, Is.Not.Empty);
            Assert.That(reloadedPrivacyPolicyModel.Body, Is.Not.Null);
            Assert.That(reloadedPrivacyPolicyModel.Body, Is.Not.Empty);

            var householdMemberController = CreateHouseholdMemberController(privacyPolicyModel: reloadedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Header, null)
                .With(m => m.Body, null)
                .With(m => m.IsAccepted, isPrivacyPoliciesAccepted)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.Identifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(privacyPolicyModel.Identifier, Is.Not.EqualTo(reloadedPrivacyPolicyModel.Identifier));
            Assert.That(privacyPolicyModel.Header, Is.Null);
            Assert.That(privacyPolicyModel.Body, Is.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.EqualTo(isPrivacyPoliciesAccepted));

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Create(householdModel);

            Assert.That(privacyPolicyModel.Identifier, Is.EqualTo(reloadedPrivacyPolicyModel.Identifier));
            Assert.That(privacyPolicyModel.Header, Is.Not.Null);
            Assert.That(privacyPolicyModel.Header, Is.Not.Empty);
            Assert.That(privacyPolicyModel.Header, Is.EqualTo(reloadedPrivacyPolicyModel.Header));
            Assert.That(privacyPolicyModel.Body, Is.Not.Null);
            Assert.That(privacyPolicyModel.Body, Is.Not.Empty);
            Assert.That(privacyPolicyModel.Body, Is.EqualTo(reloadedPrivacyPolicyModel.Body));
            Assert.That(privacyPolicyModel.IsAccepted, Is.EqualTo(isPrivacyPoliciesAccepted));
        }

        /// <summary>
        /// Tests that Create with an ivalid model returns a ViewResult with a model for creating a new household member.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatCreateWithInvalidModelReturnsViewResultWithModelForCreatingHouseholdMember(bool isPrivacyPoliciesAccepted)
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.ModelState, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, isPrivacyPoliciesAccepted)
                .Create();
                
            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .Create();

            householdMemberController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdMemberController.ModelState.IsValid, Is.False);

            var result = householdMemberController.Create(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Create"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            var model = (HouseholdModel) viewResult.Model;
            Assert.That(model.PrivacyPolicy, Is.Not.Null);
            Assert.That(model.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(model.PrivacyPolicy.IsAccepted, Is.False);
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
        /// <param name="isPrivacyPoliciesAccepted">Sets whether the privacy policies has been accepted.</param>
        /// <returns>Controller for a household member for unit testing.</returns>
        private HouseholdMemberController CreateHouseholdMemberController(PrivacyPolicyModel privacyPolicyModel = null, bool isPrivacyPoliciesAccepted = false)
        {
            _householdDataRepositoryMock.Stub(m => m.GetPrivacyPoliciesAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(() => privacyPolicyModel ?? Fixture.Create<PrivacyPolicyModel>()))
                .Repeat.Any();

            _claimValueProviderMock.Stub(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Anything))
                .Return(isPrivacyPoliciesAccepted)
                .Repeat.Any();

            var householdMemberController = new HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock);
            householdMemberController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdMemberController);
            return householdMemberController;
        }
    }
}
