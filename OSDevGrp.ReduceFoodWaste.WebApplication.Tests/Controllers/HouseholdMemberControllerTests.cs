using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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

            var householdMemberController = CreateHouseholdMemberController(privacyPolicyModel, isPrivacyPoliciesAccepted: isPrivacyPoliciesAccepted);
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
            if (isPrivacyPoliciesAccepted)
            {
                Assert.That(model.PrivacyPolicy.AcceptedTime, Is.Not.Null);
                Assert.That(model.PrivacyPolicy.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(model.PrivacyPolicy.AcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(model.PrivacyPolicy.AcceptedTime, Is.Null);
                Assert.That(model.PrivacyPolicy.AcceptedTime.HasValue, Is.False);
            }
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
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Description, Fixture.Create<string>())
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
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
                .With(m => m.AcceptedTime, isPrivacyPoliciesAccepted ? DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.Identifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(privacyPolicyModel.Identifier, Is.Not.EqualTo(reloadedPrivacyPolicyModel.Identifier));
            Assert.That(privacyPolicyModel.Header, Is.Null);
            Assert.That(privacyPolicyModel.Body, Is.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.EqualTo(isPrivacyPoliciesAccepted));
            if (isPrivacyPoliciesAccepted)
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
            }

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
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
            if (isPrivacyPoliciesAccepted)
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
            }
        }

        /// <summary>
        /// Tests that Create with an invalid model returns a ViewResult with a model for creating a new household member.
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
                .With(m => m.AcceptedTime, isPrivacyPoliciesAccepted ? DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .Create();
                
            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
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
            Assert.That(model.PrivacyPolicy.AcceptedTime, Is.Null);
            Assert.That(model.PrivacyPolicy.AcceptedTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that Create with an valid model calls CreateHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelCallsCreateHouseholdAsyncOnHouseholdDataRepository()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            var privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            householdMemberController.Create(householdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.CreateHouseholdAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<HouseholdModel>.Is.Equal(householdModel), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Create with an valid model calls GenerateCreatedHouseholdMemberClaim on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelCallsGenerateCreatedHouseholdMemberClaimOnLocalClaimProvider()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();

            householdMemberController.Create(householdModel);

            _localClaimProviderMock.AssertWasCalled(m => m.GenerateCreatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Create with an valid model calls AddLocalClaimAsync with the claim which indicates that the user has been created as a household member on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelCallsAddLocalClaimAsyncWithCreatedHouseholdMemberClaimOnLocalClaimProvider()
        {
            var claimsIdentity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var createdHouseholdMemberClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            var householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, createdHouseholdMemberClaim: createdHouseholdMemberClaim);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.EqualTo(claimsPrincipal));
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.EqualTo(claimsIdentity));

            var privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();

            householdMemberController.Create(householdModel);

            _localClaimProviderMock.AssertWasCalled(m => m.AddLocalClaimAsync(Arg<ClaimsIdentity>.Is.Equal(claimsIdentity), Arg<Claim>.Is.Equal(createdHouseholdMemberClaim), Arg<HttpContext>.Is.Equal(HttpContext.Current)));
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has not been accepted does not calls AcceptPrivacyPolicyAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasNotBeenAcceptedDoesNotCallAcceptPrivacyPolicyAsyncOnHouseholdDataRepository()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Create(householdModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.AcceptPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything, Arg<PrivacyPolicyModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has not been accepted does not call GeneratePrivacyPoliciesAcceptedClaim on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasNotBeenAcceptedDoesNotCallGeneratePrivacyPoliciesAcceptedClaimOnLocalClaimProvider()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Create(householdModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GeneratePrivacyPoliciesAcceptedClaim());
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has not been accepted calls AddLocalClaimAsync on the provider which can append local claims to a claims identity one time.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasNotBeenAcceptedCallsAddLocalClaimAsyncOnLocalClaimProviderOneTime()
        {
            var claimsIdentity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Create(householdModel);

            _localClaimProviderMock.AssertWasCalled(m => m.AddLocalClaimAsync(Arg<ClaimsIdentity>.Is.Anything, Arg<Claim>.Is.Anything, Arg<HttpContext>.Is.Anything), opt => opt.Repeat.Times(1));
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has not been accepted returns a RedirectToRouteResult for preparing the created household member.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasNotBeenAcceptedReturnsRedirectToRouteResultForPreparingCreatedHouseholdMember()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            var result = householdMemberController.Create(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.Count, Is.EqualTo(1));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.EqualTo("action"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.EqualTo("Prepare"));
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has been accepted calls AcceptPrivacyPolicyAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasBeenAcceptedCallsAcceptPrivacyPolicyAsyncOnHouseholdDataRepository()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Create(householdModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.AcceptPrivacyPolicyAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<PrivacyPolicyModel>.Is.Equal(privacyPolicyModel)));
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has been accepted calls GeneratePrivacyPoliciesAcceptedClaim on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasBeenAcceptedCallsGeneratePrivacyPoliciesAcceptedClaimOnLocalClaimProvider()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Create(householdModel);

            _localClaimProviderMock.AssertWasCalled(m => m.GeneratePrivacyPoliciesAcceptedClaim());
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has been accepted calls AddLocalClaimAsync with the claim which indicates that the user has been accepted the privacy policies on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasBeenAcceptedCallsAddLocalClaimAsyncWithPrivacyPoliciesAcceptedClaimOnLocalClaimProvider()
        {
            var claimsIdentity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var privacyPoliciesAcceptedClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            var householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, privacyPoliciesAcceptedClaim: privacyPoliciesAcceptedClaim);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Create(householdModel);

            _localClaimProviderMock.AssertWasCalled(m => m.AddLocalClaimAsync(Arg<ClaimsIdentity>.Is.Equal(claimsIdentity), Arg<Claim>.Is.Equal(privacyPoliciesAcceptedClaim), Arg<HttpContext>.Is.Equal(HttpContext.Current)));
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has been accepted calls AddLocalClaimAsync on the provider which can append local claims to a claims identity two times.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasBeenAcceptedCallsAddLocalClaimAsyncOnLocalClaimProviderTwoTime()
        {
            var claimsIdentity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Create(householdModel);

            _localClaimProviderMock.AssertWasCalled(m => m.AddLocalClaimAsync(Arg<ClaimsIdentity>.Is.Anything, Arg<Claim>.Is.Anything, Arg<HttpContext>.Is.Anything), opt => opt.Repeat.Times(2));
        }

        /// <summary>
        /// Tests that Create with an valid model where privacy policies has been accepted returns a RedirectToRouteResult for preparing the created household member.
        /// </summary>
        [Test]
        public void TestThatCreateWithValidModelWherePrivacyPoliciesHasBeenAcceptedReturnsRedirectToRouteResultForPreparingCreatedHouseholdMember()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            var result = householdMemberController.Create(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult)result;
            Assert.That(redirectToRouteResult, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.Count, Is.EqualTo(1));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.EqualTo("action"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.EqualTo("Prepare"));
        }

        /// <summary>
        /// Tests that Prepare without a model calls IsActivatedHouseholdMember on the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatPrepareWithoutModelCallsIsActivatedHouseholdMemberOnClaimValueProvider()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            householdMemberController.Prepare();

            _claimValueProviderMock.AssertWasCalled(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity)));
        }

        /// <summary>
        /// Tests that Prepare without a model calls IsPrivacyPoliciesAccepted on the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatPrepareWithoutModelCallsIsPrivacyPoliciesAcceptedOnClaimValueProvider()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            householdMemberController.Prepare();

            _claimValueProviderMock.AssertWasCalled(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity)));
        }

        /// <summary>
        /// Tests that Prepare without a model calls GetPrivacyPoliciesAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatPrepareWithoutModelCallsCallsGetPrivacyPoliciesAsyncOnHouseholdDataRepository()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdMemberController.Prepare();

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetPrivacyPoliciesAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Prepare without a model returns a ViewResult with a model for preparing a household member.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestThatPrepareWithoutModelReturnsViewResultWithModelForPreparingHouseholdMember(bool isActivatedHouseholdMember, bool isPrivacyPoliciesAccepted)
        {
            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, !isPrivacyPoliciesAccepted)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel, Is.Not.EqualTo(isPrivacyPoliciesAccepted));

            var householdMemberController = CreateHouseholdMemberController(privacyPolicyModel: privacyPolicyModel, isActivatedHouseholdMember: isActivatedHouseholdMember, isPrivacyPoliciesAccepted: isPrivacyPoliciesAccepted);
            Assert.That(householdMemberController, Is.Not.Null);

            var result = householdMemberController.Prepare();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Prepare"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdMemberModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            var householdMemberModel = (HouseholdMemberModel) viewResult.Model;
            Assert.That(householdMemberModel.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            if (isActivatedHouseholdMember)
            {
                Assert.That(householdMemberModel.IsActivated, Is.True);
                Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
                Assert.That(householdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(householdMemberModel.IsActivated, Is.False);
                Assert.That(householdMemberModel.ActivatedTime, Is.Null);
                Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            }
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicy.IsAccepted, Is.EqualTo(isPrivacyPoliciesAccepted));
            if (isPrivacyPoliciesAccepted)
            {
                Assert.That(householdMemberModel.HasAcceptedPrivacyPolicy, Is.True);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(householdMemberModel.HasAcceptedPrivacyPolicy, Is.False);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            }
        }

        /// <summary>
        /// Tests that Prepare with a model throws an ArgumentNullException when the model is null.
        /// </summary>
        [Test]
        public void TestThatPrepareWithModelThrowsArgumentNullExceptionWhenModelIsNull()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.Prepare((HouseholdMemberModel) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMemberModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Prepare with a model calls GetPrivacyPoliciesAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatPrepareWithModelCallsGetPrivacyPoliciesAsyncOnHouseholdDataRepository()
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
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetPrivacyPoliciesAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Prepare with a model updates Identifier, Header and Body in the privacy policy model with values from the reloaded privacy policy model.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatPrepareWithModelUpdatesValuesInPrivacyPolicyModelWithValuesFromReloadedPrivacyPolicyModel(bool isPrivacyPoliciesAccepted)
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
                .With(m => m.AcceptedTime, isPrivacyPoliciesAccepted ? DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.Identifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(privacyPolicyModel.Identifier, Is.Not.EqualTo(reloadedPrivacyPolicyModel.Identifier));
            Assert.That(privacyPolicyModel.Header, Is.Null);
            Assert.That(privacyPolicyModel.Body, Is.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.EqualTo(isPrivacyPoliciesAccepted));
            if (isPrivacyPoliciesAccepted)
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
            }

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(privacyPolicyModel.Identifier, Is.EqualTo(reloadedPrivacyPolicyModel.Identifier));
            Assert.That(privacyPolicyModel.Header, Is.Not.Null);
            Assert.That(privacyPolicyModel.Header, Is.Not.Empty);
            Assert.That(privacyPolicyModel.Header, Is.EqualTo(reloadedPrivacyPolicyModel.Header));
            Assert.That(privacyPolicyModel.Body, Is.Not.Null);
            Assert.That(privacyPolicyModel.Body, Is.Not.Empty);
            Assert.That(privacyPolicyModel.Body, Is.EqualTo(reloadedPrivacyPolicyModel.Body));
            Assert.That(privacyPolicyModel.IsAccepted, Is.EqualTo(isPrivacyPoliciesAccepted));
            if (isPrivacyPoliciesAccepted)
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
            }
        }

        /// <summary>
        /// Tests that Prepare with an invalid model calls IsPrivacyPoliciesAccepted on the provider which can get values from claims.
        /// </summary>
        [Test]
        public void TestThatPrepareWithInvalidModelCallsIsPrivacyPoliciesAcceptedOnClaimValueProvider()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);
            Assert.That(householdMemberController.ModelState, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();

            householdMemberController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdMemberController.ModelState.IsValid, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _claimValueProviderMock.AssertWasCalled(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity)));
        }

        /// <summary>
        /// Tests that Prepare with an invalid model returns a ViewResult with a model for preparing a household members account.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestThatPrepareWithInvalidModelReturnsViewResultWithModelForPreparingHouseholdMember(bool isPrivacyPoliciesAccepted, bool privacyPoliciesHasAlreadyBeenAccepted)
        {
            var householdMemberController = CreateHouseholdMemberController(isPrivacyPoliciesAccepted: privacyPoliciesHasAlreadyBeenAccepted);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.ModelState, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, isPrivacyPoliciesAccepted)
                .With(m => m.AcceptedTime, privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            if (privacyPoliciesHasAlreadyBeenAccepted)
            {
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.Not.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime.HasValue, Is.True);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.Null);
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime.HasValue, Is.False);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            }

            householdMemberController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdMemberController.ModelState.IsValid, Is.False);

            var result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Prepare"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdMemberModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            var model = (HouseholdMemberModel) viewResult.Model;
            Assert.That(model.PrivacyPolicy, Is.Not.Null);
            Assert.That(model.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(model.PrivacyPolicy.IsAccepted, Is.EqualTo(privacyPoliciesHasAlreadyBeenAccepted));
            if (privacyPoliciesHasAlreadyBeenAccepted)
            {
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime.HasValue, Is.True);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.Null);
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime.HasValue, Is.False);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            }
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call ActivateHouseholdMemberAsync on the repository which can access household data when the household member has been activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotCallActivateHouseholdMemberAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasBeenActivated()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.True);

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.ActivateHouseholdMemberAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdMemberModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call GenerateActivatedHouseholdMemberClaim on the provider which can append local claims to a claims identity when the household member has been activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotCallGenerateActivatedHouseholdMemberClaimOnLocalClaimProviderWhenHouseholdMemberHasBeenActivated()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, DateTime.Now.AddDays(Random.Next(1, 7) * -1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.True);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GenerateActivatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not update ActivatedTime on the household member when the household member has been activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotUpdateActivatedTimeOnHouseholdMemberWhenHouseholdMemberHasBeenActivated()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var activationTime = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));
            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, activationTime)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.True);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.EqualTo(activationTime));
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.True);
        }

        /// <summary>
        /// Tests that Prepare with a valid model without an activation code does not call ActivateHouseholdMemberAsync on the repository which can access household data when the household member has been not activated.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatPrepareWithValidModelWithoutActivationCodeDoesNotCallActivateHouseholdMemberAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasNotBeenActivated(string noActicationCode)
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, noActicationCode)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(noActicationCode));
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.ActivateHouseholdMemberAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdMemberModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that Prepare with a valid model without an activation code does not call GenerateActivatedHouseholdMemberClaim on the provider which can append local claims to a claims identity when the household member has been not activated.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatPrepareWithValidModelWithoutActivationCodeDoesNotCallGenerateActivatedHouseholdMemberClaimOnLocalClaimProviderWhenHouseholdMemberHasNotBeenActivated(string noActicationCode)
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, noActicationCode)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(noActicationCode));
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GenerateActivatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model without an activation code does not update ActivatedTime on the household member when the household member has been not activated.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatPrepareWithValidModelWithoutActivationCodeDoesNotUpdateActivatedTimeOnHouseholdMemberWhenHouseholdMemberHasNotBeenActivated(string noActicationCode)
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, noActicationCode)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(noActicationCode));
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GenerateActivatedHouseholdMemberClaim());

            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that Prepare with a valid model with an activation code calls ActivateHouseholdMemberAsync on the repository which can access household data when the household member has been not activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationCodeCallsActivateHouseholdMemberAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasNotBeenActivated()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.ActivateHouseholdMemberAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<HouseholdMemberModel>.Is.Equal(householdMemberModel)));
        }

        /// <summary>
        /// Tests that Prepare with a valid model with an activation code calls GenerateActivatedHouseholdMemberClaim on the provider which can append local claims to a claims identity when the household member has been not activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationCodeCallsGenerateActivatedHouseholdMemberClaimOnLocalClaimProviderWhenHouseholdMemberHasNotBeenActivated()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasCalled(m => m.GenerateActivatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model with an activation code calls AddLocalClaimAsync with the claim which indicates that the users household member account has been activated on the provider which can append local claims to a claims identity when the household member has been not activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationCodeCallsAddLocalClaimAsyncWithActivatedHouseholdMemberClaimOnLocalClaimProviderWhenHouseholdMemberHasNotBeenActivated()
        {
            var claimsIdentity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var activatedHouseholdMemberClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            var householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, activatedHouseholdMemberClaim: activatedHouseholdMemberClaim);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasCalled(m => m.AddLocalClaimAsync(Arg<ClaimsIdentity>.Is.Equal(claimsIdentity), Arg<Claim>.Is.Equal(activatedHouseholdMemberClaim), Arg<HttpContext>.Is.Equal(HttpContext.Current)));
        }

        /// <summary>
        /// Tests that Prepare with a valid model with an activation code updates ActivatedTime on the household member when the household member has been not activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationCodeUpdatesActivatedTimeOnHouseholdMemberWhenHouseholdMemberHasNotBeenActivated()
        {
            var activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime.HasValue, Is.True);
                
            var householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, null)
                .Create();

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.EqualTo(activatedHouseholdMemberModel.ActivatedTime));
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.True);
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call AcceptPrivacyPolicyAsync on the repository which can access household data when the household member has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotCallAcceptPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasAcceptedPrivacyPolicies()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.AcceptPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything, Arg<PrivacyPolicyModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call GeneratePrivacyPoliciesAcceptedClaim on the provider which can append local claims to a claims identity when the household member has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotCallGeneratePrivacyPoliciesAcceptedClaimOnLocalClaimProviderWhenHouseholdMemberHasAcceptedPrivacyPolicies()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GeneratePrivacyPoliciesAcceptedClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call update PrivacyPolicyAcceptedTime on the household member when the household member has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotUpdatePrivacyPolicyAcceptedTimeOnHouseholdMemberWhenHouseholdMemberHasAcceptedPrivacyPolicies()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyAcceptedTime = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));
            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, privacyPolicyAcceptedTime)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, privacyPolicyAcceptedTime)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.EqualTo(privacyPolicyAcceptedTime));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);
        }

        /// <summary>
        /// Tests that Prepare with a valid model without accept of the privacy policies does not call AcceptPrivacyPolicyAsync on the repository which can access household data when the household member has not accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutAcceptOfPrivacyPoliciesDoesNotCallAcceptPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasNotAcceptedPrivacyPolicies()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.AcceptPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything, Arg<PrivacyPolicyModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that Prepare with a valid model without accept of the privacy policies does not call GeneratePrivacyPoliciesAcceptedClaim on the provider which can append local claims to a claims identity when the household member has not accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutAcceptOfPrivacyPoliciesDoesNotCallGeneratePrivacyPoliciesAcceptedClaimOnLocalClaimProviderWhenHouseholdMemberHasNotAcceptedPrivacyPolicies()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GeneratePrivacyPoliciesAcceptedClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model without accept of the privacy policies does not update PrivacyPolicyAcceptedTime on the household member when the household member has not accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutAcceptOfPrivacyPoliciesDoesNotUpdatePrivacyPolicyAcceptedTimeOnHouseholdMemberWhenHouseholdMemberHasNotAcceptedPrivacyPolicies()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);
        }

        /// <summary>
        /// Tests that Prepare with a valid model with accept of the privacy policies calls AcceptPrivacyPolicyAsync on the repository which can access household data when the household member has not accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithAcceptOfPrivacyPoliciesCallsAcceptPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasNotAcceptedPrivacyPolicies()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasCalled(m => m.AcceptPrivacyPolicyAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<PrivacyPolicyModel>.Is.Equal(privacyPolicyModel)));
        }

        /// <summary>
        /// Tests that Prepare with a valid model with accept of the privacy policies calls GeneratePrivacyPoliciesAcceptedClaim on the provider which can append local claims to a claims identity when the household member has not accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithAcceptOfPrivacyPoliciesCallsGeneratePrivacyPoliciesAcceptedClaimOnLocalClaimProviderWhenHouseholdMemberHasNotAcceptedPrivacyPolicies()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasCalled(m => m.GeneratePrivacyPoliciesAcceptedClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model with accept of the privacy policies calls AddLocalClaimAsync with the claim which indicates that the user has been accepted the privacy policies on the provider which can append local claims to a claims identity when the household member has not accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithAcceptOfPrivacyPoliciesCallsAddLocalClaimAsyncWithPrivacyPoliciesAcceptedClaimOnLocalClaimProviderWhenHouseholdMemberHasNotAcceptedPrivacyPolicies()
        {
            var claimsIdentity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var privacyPoliciesAcceptedClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            var householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, privacyPoliciesAcceptedClaim: privacyPoliciesAcceptedClaim);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasCalled(m => m.AddLocalClaimAsync(Arg<ClaimsIdentity>.Is.Equal(claimsIdentity), Arg<Claim>.Is.Equal(privacyPoliciesAcceptedClaim), Arg<HttpContext>.Is.Equal(HttpContext.Current)));
        }

        /// <summary>
        /// Tests that Prepare with a valid model with accept of the privacy policies updates PrivacyPolicyAcceptedTime on the household member when the household member has not accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithAcceptOfPrivacyPoliciesUpdatesPrivacyPolicyAcceptedTimeOnHouseholdMemberWhenHouseholdMemberHasNotAcceptedPrivacyPolicies()
        {
            var acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberController = CreateHouseholdMemberController(acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.EqualTo(acceptedPrivacyPolicyModel.AcceptedTime));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.True);
        }

        /// <summary>
        /// Tests that Prepare with a valid model without activation and without accept of the privacy policies does not call GenerateValidatedHouseholdMemberClaim on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutActivationAndWithoutAcceptOfPrivacyPolicyDoesNotCallGenerateValidatedHouseholdMemberClaimOnLocalClaimProvider()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GenerateValidatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model without activation and without accept of the privacy policies returns a ViewResult with a model for preparing a household members account.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutActivationAndWithoutAcceptOfPrivacyPolicyReturnsViewResultWithModelForPreparingHouseholdMember()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            var result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Prepare"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdMemberModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);
        }

        /// <summary>
        /// Tests that Prepare with a valid model without activation and with accept of the privacy policies does not call GenerateValidatedHouseholdMemberClaim on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutActivationAndWithAcceptOfPrivacyPolicyDoesNotCallGenerateValidatedHouseholdMemberClaimOnLocalClaimProvider()
        {
            var acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberController = CreateHouseholdMemberController(acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GenerateValidatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model without activation and with accept of the privacy policies returns a ViewResult with a model for preparing a household members account.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutActivationAndWithAcceptOfPrivacyPolicyReturnsViewResultWithModelForPreparingHouseholdMember()
        {
            var acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberController = CreateHouseholdMemberController(acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, null)
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            var result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Prepare"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdMemberModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);
        }

        /// <summary>
        /// Tests that Prepare with a valid model with activation and without accept of the privacy policies does not call GenerateValidatedHouseholdMemberClaim on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationAndWithoutAcceptOfPrivacyPolicyDoesNotCallGenerateValidatedHouseholdMemberClaimOnLocalClaimProvider()
        {
            var activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime.HasValue, Is.True);

            var householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GenerateValidatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model with activation and without accept of the privacy policies returns a ViewResult with a model for preparing a household members account.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationAndWithoutAcceptOfPrivacyPolicyReturnsViewResultWithModelForPreparingHouseholdMember()
        {
            var activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime.HasValue, Is.True);

            var householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            var result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Prepare"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdMemberModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);
        }

        /// <summary>
        /// Tests that Prepare with a valid model with activation and with accept of the privacy policies calls GenerateValidatedHouseholdMemberClaim on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationAndWithAcceptOfPrivacyPolicyCallsGenerateValidatedHouseholdMemberClaimOnLocalClaimProvider()
        {
            var activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime.HasValue, Is.True);

            var acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel, acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasCalled(m => m.GenerateValidatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model with activation and with accept of the privacy policies calls calls AddLocalClaimAsync with the claim which indicates that the user is a validated household member on the provider which can append local claims to a claims identity
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationAndWithAcceptOfPrivacyPolicyCallsAddLocalClaimAsyncWithValidatedHouseholdMemberClaimOnLocalClaimProvider()
        {
            var claimsIdentity = new ClaimsIdentity();
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var validatedHouseholdMemberClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            var activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime.HasValue, Is.True);

            var acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, validatedHouseholdMemberClaim: validatedHouseholdMemberClaim, activatedHouseholdMemberModel: activatedHouseholdMemberModel, acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasCalled(m => m.AddLocalClaimAsync(Arg<ClaimsIdentity>.Is.Equal(claimsIdentity), Arg<Claim>.Is.Equal(validatedHouseholdMemberClaim), Arg<HttpContext>.Is.Equal(HttpContext.Current)));
        }

        /// <summary>
        /// Tests that Prepare with a valid model with activation and with accept of the privacy policies returns a RedirectToRouteResult for the dashboard.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithActivationAndWithAcceptOfPrivacyPolicyReturnsRedirectToRouteResultForDashboard()
        {
            var activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime.HasValue, Is.True);

            var acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime.HasValue, Is.True);

            var householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel, acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, null)
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            var result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.Count, Is.EqualTo(2));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Key, Is.EqualTo("action"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(0).Value, Is.EqualTo("Dashboard"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1), Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Key, Is.EqualTo("controller"));
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ElementAt(1).Value, Is.EqualTo("Dashboard"));
        }

        /// <summary>
        /// Tests that Manage without a model calls GetHouseholdMemberAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatManageWithoutModelCallsGetHouseholdMemberAsyncOnHouseholdDataRepository()
        {
            var householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdMemberController.Manage();

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Manage without a model returns a ViewResult with a model for manage the household member account.
        /// </summary>
        [Test]
        public void TestThatManageWithoutModelReturnsViewResultWithModelForManageHouseholdMember()
        {
            var householdMemberModel = MockRepository.GenerateMock<HouseholdMemberModel>();
            Assert.That(householdMemberModel, Is.Not.Null);

            var householdMemberController = CreateHouseholdMemberController(householdMemberModel: householdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            var result = householdMemberController.Manage();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdMemberModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);
        }

        /// <summary>
        /// Creates a controller for a household member for unit testing.
        /// </summary>
        /// <param name="privacyPolicyModel">Sets the privacy policy model which should be used by the controller.</param>
        /// <param name="isActivatedHouseholdMember">Sets whether the house member has been activated.</param>
        /// <param name="isPrivacyPoliciesAccepted">Sets whether the privacy policies has been accepted.</param>
        /// <param name="principal">Sets the user principal for the controller.</param>
        /// <param name="createdHouseholdMemberClaim">Sets the claim which indicates that the user has been created as a household member.</param>
        /// <param name="activatedHouseholdMemberClaim">Sets the claim which indicates that the users household member account has been activated.</param>
        /// <param name="privacyPoliciesAcceptedClaim">Sets the claim which indicates that the user has accepted the privacy policies.</param>
        /// <param name="validatedHouseholdMemberClaim">Sets the claim which indicates that the user is a validated household member.</param>
        /// <param name="activatedHouseholdMemberModel">Sets the model for an activated household member.</param>
        /// <param name="acceptedPrivacyPolicyModel">Sets the model for a privacy model which has been accepted.</param>
        /// <param name="householdMemberModel">Sets the model for the current users household member account.</param>
        /// <returns>Controller for a household member for unit testing.</returns>
        private HouseholdMemberController CreateHouseholdMemberController(PrivacyPolicyModel privacyPolicyModel = null, bool isActivatedHouseholdMember = false, bool isPrivacyPoliciesAccepted = false, IPrincipal principal = null, Claim createdHouseholdMemberClaim = null, Claim activatedHouseholdMemberClaim = null, Claim privacyPoliciesAcceptedClaim = null, Claim validatedHouseholdMemberClaim = null, HouseholdMemberModel activatedHouseholdMemberModel = null, PrivacyPolicyModel acceptedPrivacyPolicyModel = null, HouseholdMemberModel householdMemberModel = null)
        {
            Func<HouseholdModel> householdCreator = () =>
            {
                return Fixture.Build<HouseholdModel>()
                    .With(m => m.HouseholdMembers, null)
                    .Create();
            };
            Func<HouseholdMemberModel> householdMemberActivator = () =>
            {
                if (activatedHouseholdMemberModel != null)
                {
                    return activatedHouseholdMemberModel;
                }
                return Fixture.Build<HouseholdMemberModel>()
                    .With(m => m.Households, null)
                    .Create();
            };
            Func<HouseholdMemberModel> householdMemberGetter = () =>
            {
                if (householdMemberModel != null)
                {
                    return householdMemberModel;
                }
                return Fixture.Build<HouseholdMemberModel>()
                    .With(m => m.Households, null)
                    .Create();
            };
            _householdDataRepositoryMock.Stub(m => m.GetPrivacyPoliciesAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(() => privacyPolicyModel ?? Fixture.Create<PrivacyPolicyModel>()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.CreateHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(householdCreator))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.ActivateHouseholdMemberAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdMemberModel>.Is.Anything))
                .Return(Task.Run(householdMemberActivator))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.AcceptPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything, Arg<PrivacyPolicyModel>.Is.Anything))
                .Return(Task.Run(() => acceptedPrivacyPolicyModel ?? Fixture.Create<PrivacyPolicyModel>()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(householdMemberGetter))
                .Repeat.Any();

            _claimValueProviderMock.Stub(m => m.IsActivatedHouseholdMember(Arg<IIdentity>.Is.Anything))
                .Return(isActivatedHouseholdMember)
                .Repeat.Any();
            _claimValueProviderMock.Stub(m => m.IsPrivacyPoliciesAccepted(Arg<IIdentity>.Is.Anything))
                .Return(isPrivacyPoliciesAccepted)
                .Repeat.Any();

            _localClaimProviderMock.Stub(m => m.GenerateCreatedHouseholdMemberClaim())
                .Return(createdHouseholdMemberClaim ?? new Claim(Fixture.Create<string>(), Fixture.Create<string>()))
                .Repeat.Any();
            _localClaimProviderMock.Stub(m => m.GenerateActivatedHouseholdMemberClaim())
                .Return(activatedHouseholdMemberClaim ?? new Claim(Fixture.Create<string>(), Fixture.Create<string>()))
                .Repeat.Any();
            _localClaimProviderMock.Stub(m => m.GeneratePrivacyPoliciesAcceptedClaim())
                .Return(privacyPoliciesAcceptedClaim ?? new Claim(Fixture.Create<string>(), Fixture.Create<string>()))
                .Repeat.Any();
            _localClaimProviderMock.Stub(m => m.GenerateValidatedHouseholdMemberClaim())
                .Return(validatedHouseholdMemberClaim ?? new Claim(Fixture.Create<string>(), Fixture.Create<string>()))
                .Repeat.Any();

            _localClaimProviderMock.Stub(m => m.AddLocalClaimAsync(Arg<ClaimsIdentity>.Is.Anything, Arg<Claim>.Is.Anything, Arg<HttpContext>.Is.Anything))
                .Return(Task.Run(() => { }))
                .Repeat.Any();

            var householdMemberController = new HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock);
            householdMemberController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdMemberController, principal: principal);
            return householdMemberController;
        }
    }
}
