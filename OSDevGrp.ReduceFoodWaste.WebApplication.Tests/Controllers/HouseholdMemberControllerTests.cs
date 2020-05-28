using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models.Enums;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
        /// Tests that the constructor initialize the controller for a household member.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdMemberController()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = new WebApplication.Controllers.HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock, _modelHelperMock, _utilitiesMock);
            Assert.That(householdMemberController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new WebApplication.Controllers.HouseholdMemberController(null, _claimValueProviderMock, _localClaimProviderMock, _modelHelperMock, _utilitiesMock));
            // ReSharper restore ObjectCreationAsStatement
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
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new WebApplication.Controllers.HouseholdMemberController(_householdDataRepositoryMock, null, _localClaimProviderMock, _modelHelperMock, _utilitiesMock));
            // ReSharper restore ObjectCreationAsStatement
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
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new WebApplication.Controllers.HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, null, _modelHelperMock, _utilitiesMock));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("localClaimProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the model helper is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenModelHelperIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new WebApplication.Controllers.HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock, null, _utilitiesMock));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("modelHelper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the utilities which support the infrastructure is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenUtilitiesIsNull()
        {
            // ReSharper disable ObjectCreationAsStatement
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new WebApplication.Controllers.HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock, _modelHelperMock, null));
            // ReSharper restore ObjectCreationAsStatement
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("utilities"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Create without a model calls GetPrivacyPoliciesAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatCreateWithoutModelCallsGetPrivacyPoliciesAsyncOnHouseholdDataRepository()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
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
            PrivacyPolicyModel privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(privacyPolicyModel, isPrivacyPoliciesAccepted: isPrivacyPoliciesAccepted);
            Assert.That(householdMemberController, Is.Not.Null);

            ActionResult result = householdMemberController.Create();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Create"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            HouseholdModel model = (HouseholdModel) viewResult.Model;
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.Create(null));
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Header, (string) null)
                .With(m => m.Body, (string) null)
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Description, Fixture.Create<string>())
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            PrivacyPolicyModel reloadedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
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

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(privacyPolicyModel: reloadedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Header, (string) null)
                .With(m => m.Body, (string) null)
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
            }
            else
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
            }

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.ModelState, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, isPrivacyPoliciesAccepted)
                .With(m => m.AcceptedTime, isPrivacyPoliciesAccepted ? DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .Create();
                
            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();

            householdMemberController.ModelState.AddModelError(Fixture.Create<string>(), Fixture.Create<string>());
            Assert.That(householdMemberController.ModelState.IsValid, Is.False);

            ActionResult result = householdMemberController.Create(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Create"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            HouseholdModel model = (HouseholdModel) viewResult.Model;
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Claim createdHouseholdMemberClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, createdHouseholdMemberClaim: createdHouseholdMemberClaim);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.EqualTo(claimsPrincipal));
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.EqualTo(claimsIdentity));

            PrivacyPolicyModel privacyPolicyModel = Fixture.Create<PrivacyPolicyModel>();

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            ActionResult result = householdMemberController.Create(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult) result;
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Claim privacyPoliciesAcceptedClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, privacyPoliciesAcceptedClaim: privacyPoliciesAcceptedClaim);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            HouseholdModel householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));

            ActionResult result = householdMemberController.Create(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult) result;
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
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
            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, !isPrivacyPoliciesAccepted)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel, Is.Not.EqualTo(isPrivacyPoliciesAccepted));

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(privacyPolicyModel: privacyPolicyModel, isActivatedHouseholdMember: isActivatedHouseholdMember, isPrivacyPoliciesAccepted: isPrivacyPoliciesAccepted);
            Assert.That(householdMemberController, Is.Not.Null);

            ActionResult result = householdMemberController.Prepare();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Prepare"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdMemberModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            HouseholdMemberModel householdMemberModel = (HouseholdMemberModel) viewResult.Model;
            Assert.That(householdMemberModel.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            if (isActivatedHouseholdMember)
            {
                Assert.That(householdMemberModel.IsActivated, Is.True);
                Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
                Assert.That(householdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.Prepare((HouseholdMemberModel) null));
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Header, (string) null)
                .With(m => m.Body, (string) null)
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            PrivacyPolicyModel reloadedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
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

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(privacyPolicyModel: reloadedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Header, (string) null)
                .With(m => m.Body, (string) null)
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
            }
            else
            {
                Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
                Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);
            }

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);
            Assert.That(householdMemberController.ModelState, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(isPrivacyPoliciesAccepted: privacyPoliciesHasAlreadyBeenAccepted);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.ModelState, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, isPrivacyPoliciesAccepted)
                .With(m => m.AcceptedTime, privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            if (privacyPoliciesHasAlreadyBeenAccepted)
            {
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.Not.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.EqualTo(DateTime.Now).Within(3).Seconds);
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

            ActionResult result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Prepare"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdMemberModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            HouseholdMemberModel model = (HouseholdMemberModel) viewResult.Model;
            Assert.That(model.PrivacyPolicy, Is.Not.Null);
            Assert.That(model.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(model.PrivacyPolicy.IsAccepted, Is.EqualTo(privacyPoliciesHasAlreadyBeenAccepted));
            if (privacyPoliciesHasAlreadyBeenAccepted)
            {
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicy.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.ActivateHouseholdMemberAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdMemberModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call GenerateActivatedHouseholdMemberClaim on the provider which can append local claims to a claims identity when the household member has been activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotCallGenerateActivatedHouseholdMemberClaimOnLocalClaimProviderWhenHouseholdMemberHasBeenActivated()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, DateTime.Now.AddDays(Random.Next(1, 7) * -1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GenerateActivatedHouseholdMemberClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not update ActivatedTime on the household member when the household member has been activated.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotUpdateActivatedTimeOnHouseholdMemberWhenHouseholdMemberHasBeenActivated()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            DateTime activationTime = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));
            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, activationTime)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.EqualTo(activationTime));
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
        public void TestThatPrepareWithValidModelWithoutActivationCodeDoesNotCallActivateHouseholdMemberAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasNotBeenActivated(string noActivationCode)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, noActivationCode)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(noActivationCode));
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
        public void TestThatPrepareWithValidModelWithoutActivationCodeDoesNotCallGenerateActivatedHouseholdMemberClaimOnLocalClaimProviderWhenHouseholdMemberHasNotBeenActivated(string noActivationCode)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, noActivationCode)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(noActivationCode));
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
        public void TestThatPrepareWithValidModelWithoutActivationCodeDoesNotUpdateActivatedTimeOnHouseholdMemberWhenHouseholdMemberHasNotBeenActivated(string noActivationCode)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, noActivationCode)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(noActivationCode));
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Claim activatedHouseholdMemberClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, activatedHouseholdMemberClaim: activatedHouseholdMemberClaim);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            HouseholdMemberModel activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);
                
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, Fixture.Create<bool>())
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.EqualTo(activatedHouseholdMemberModel.ActivatedTime));
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call AcceptPrivacyPolicyAsync on the repository which can access household data when the household member has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotCallAcceptPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasAcceptedPrivacyPolicies()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);

            householdMemberController.Prepare(householdMemberModel);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.AcceptPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything, Arg<PrivacyPolicyModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call GeneratePrivacyPoliciesAcceptedClaim on the provider which can append local claims to a claims identity when the household member has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotCallGeneratePrivacyPoliciesAcceptedClaimOnLocalClaimProviderWhenHouseholdMemberHasAcceptedPrivacyPolicies()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);

            householdMemberController.Prepare(householdMemberModel);

            _localClaimProviderMock.AssertWasNotCalled(m => m.GeneratePrivacyPoliciesAcceptedClaim());
        }

        /// <summary>
        /// Tests that Prepare with a valid model does not call update PrivacyPolicyAcceptedTime on the household member when the household member has accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelDoesNotUpdatePrivacyPolicyAcceptedTimeOnHouseholdMemberWhenHouseholdMemberHasAcceptedPrivacyPolicies()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            DateTime privacyPolicyAcceptedTime = DateTime.Now.AddDays(Random.Next(1, 7)*-1).AddMinutes(Random.Next(-120, 120));
            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, privacyPolicyAcceptedTime)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Not.Null);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, privacyPolicyAcceptedTime)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.EqualTo(privacyPolicyAcceptedTime));
        }

        /// <summary>
        /// Tests that Prepare with a valid model without accept of the privacy policies does not call AcceptPrivacyPolicyAsync on the repository which can access household data when the household member has not accepted the privacy policies.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutAcceptOfPrivacyPoliciesDoesNotCallAcceptPrivacyPolicyAsyncOnHouseholdDataRepositoryWhenHouseholdMemberHasNotAcceptedPrivacyPolicies()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Claim privacyPoliciesAcceptedClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, privacyPoliciesAcceptedClaim: privacyPoliciesAcceptedClaim);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            PrivacyPolicyModel acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            householdMemberController.Prepare(householdMemberModel);

            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.EqualTo(acceptedPrivacyPolicyModel.AcceptedTime));
        }

        /// <summary>
        /// Tests that Prepare with a valid model without activation and without accept of the privacy policies does not call GenerateValidatedHouseholdMemberClaim on the provider which can append local claims to a claims identity.
        /// </summary>
        [Test]
        public void TestThatPrepareWithValidModelWithoutActivationAndWithoutAcceptOfPrivacyPolicyDoesNotCallGenerateValidatedHouseholdMemberClaimOnLocalClaimProvider()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            ActionResult result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
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
            PrivacyPolicyModel acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            PrivacyPolicyModel acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, (string) null)
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime, Is.Null);
            Assert.That(householdMemberModel.ActivatedTime.HasValue, Is.False);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.Not.Null);
            Assert.That(householdMemberModel.PrivacyPolicy, Is.EqualTo(privacyPolicyModel));
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(householdMemberModel.PrivacyPolicyAcceptedTime.HasValue, Is.False);

            ActionResult result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
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
            HouseholdMemberModel activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            HouseholdMemberModel activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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

            ActionResult result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
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
            HouseholdMemberModel activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            PrivacyPolicyModel acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel, acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Claim validatedHouseholdMemberClaim = new Claim(Fixture.Create<string>(), Fixture.Create<string>());

            HouseholdMemberModel activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            PrivacyPolicyModel acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(principal: claimsPrincipal, validatedHouseholdMemberClaim: validatedHouseholdMemberClaim, activatedHouseholdMemberModel: activatedHouseholdMemberModel, acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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
            HouseholdMemberModel activatedHouseholdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivatedTime, DateTime.Now)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                .Create();
            Assert.That(activatedHouseholdMemberModel, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.Not.Null);
            Assert.That(activatedHouseholdMemberModel.ActivatedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            PrivacyPolicyModel acceptedPrivacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.AcceptedTime, DateTime.Now)
                .Create();
            Assert.That(acceptedPrivacyPolicyModel, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.Not.Null);
            Assert.That(acceptedPrivacyPolicyModel.AcceptedTime, Is.EqualTo(DateTime.Now).Within(3).Seconds);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(activatedHouseholdMemberModel: activatedHouseholdMemberModel, acceptedPrivacyPolicyModel: acceptedPrivacyPolicyModel);
            Assert.That(householdMemberController, Is.Not.Null);

            PrivacyPolicyModel privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .With(m => m.AcceptedTime, (DateTime?) null)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);
            Assert.That(privacyPolicyModel.AcceptedTime, Is.Null);
            Assert.That(privacyPolicyModel.AcceptedTime.HasValue, Is.False);

            HouseholdMemberModel householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Identifier, default(Guid))
                .With(m => m.ActivationCode, Fixture.Create<string>())
                .With(m => m.ActivatedTime, (DateTime?) null)
                .With(m => m.PrivacyPolicy, privacyPolicyModel)
                .With(m => m.PrivacyPolicyAcceptedTime, (DateTime?) null)
                .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
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

            ActionResult result = householdMemberController.Prepare(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult) result;
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
        /// Tests that Manage without a status message calls GetHouseholdMemberAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatManageWithoutStatusMessageCallsGetHouseholdMemberAsyncOnHouseholdDataRepository(string statusMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            householdMemberController.Manage(statusMessage: statusMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Manage with a status message calls GetHouseholdMemberAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatManageWithStatusMessageCallsGetHouseholdMemberAsyncOnHouseholdDataRepository()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            householdMemberController.Manage(statusMessage: statusMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Manage without an error message calls GetHouseholdMemberAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatManageWithoutErrorMessageCallsGetHouseholdMemberAsyncOnHouseholdDataRepository(string errorMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            householdMemberController.Manage(errorMessage: errorMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Manage with an error message calls GetHouseholdMemberAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatManageWithErrorMessageCallsGetHouseholdMemberAsyncOnHouseholdDataRepository()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            householdMemberController.Manage(errorMessage: errorMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Manage without a status message returns a ViewResult with a model for manage the household member account.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatManageWithoutStatusMessageReturnsViewResultWithModelForManageHouseholdMember(string statusMessage)
        {
            HouseholdMemberModel householdMemberModel = MockRepository.GenerateMock<HouseholdMemberModel>();
            Assert.That(householdMemberModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(householdMemberModel: householdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            ActionResult result = householdMemberController.Manage(statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
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
        /// Tests that Manage with a status message returns a ViewResult with a model for manage the household member account.
        /// </summary>
        [Test]
        public void TestThatManageWithStatusMessageReturnsViewResultWithModelForManageHouseholdMember()
        {
            HouseholdMemberModel householdMemberModel = MockRepository.GenerateMock<HouseholdMemberModel>();
            Assert.That(householdMemberModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(householdMemberModel: householdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            ActionResult result = householdMemberController.Manage(statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdMemberModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.Not.Null);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.EqualTo(statusMessage));
        }

        /// <summary>
        /// Tests that Manage without an error message returns a ViewResult with a model for manage the household member account.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatManageWithoutErrorMessageReturnsViewResultWithModelForManageHouseholdMember(string errorMessage)
        {
            HouseholdMemberModel householdMemberModel = MockRepository.GenerateMock<HouseholdMemberModel>();
            Assert.That(householdMemberModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(householdMemberModel: householdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            ActionResult result = householdMemberController.Manage(errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
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
        /// Tests that Manage with an error message returns a ViewResult with a model for manage the household member account.
        /// </summary>
        [Test]
        public void TestThatManageWithErrorMessageReturnsViewResultWithModelForManageHouseholdMember()
        {
            HouseholdMemberModel householdMemberModel = MockRepository.GenerateMock<HouseholdMemberModel>();
            Assert.That(householdMemberModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(householdMemberModel: householdMemberModel);
            Assert.That(householdMemberController, Is.Not.Null);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            ActionResult result = householdMemberController.Manage(errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(householdMemberModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.EqualTo(errorMessage));
        }

        /// <summary>
        /// Tests that UpgradeMembership throws an ArgumentNullException when the url on which to return when the upgrade process has finished is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatUpgradeMembershipThrowsArgumentNullExceptionWhenReturnUrlIsNullOrEmpty(string returnUrl)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.UpgradeMembership(returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("returnUrl"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeMembership without a status message calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatUpgradeMembershipWithoutStatusMessageCallsGetMembershipsAsyncOnHouseholdDataRepository(string statusMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            householdMemberController.UpgradeMembership(Fixture.Create<string>(), statusMessage: statusMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that UpgradeMembership with a status message calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatUpgradeMembershipWithStatusMessageCallsGetMembershipsAsyncOnHouseholdDataRepository()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            householdMemberController.UpgradeMembership(Fixture.Create<string>(), statusMessage: statusMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that UpgradeMembership without an error message calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatUpgradeMembershipWithoutErrorMessageCallsGetMembershipsAsyncOnHouseholdDataRepository(string errorMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            householdMemberController.UpgradeMembership(Fixture.Create<string>(), errorMessage: errorMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that UpgradeMembership with an error message calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatUpgradeMembershipWithErrorMessageCallsGetMembershipsAsyncOnHouseholdDataRepository()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            householdMemberController.UpgradeMembership(Fixture.Create<string>(), errorMessage: errorMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that UpgradeMembership without a status message throws an ReduceFoodWasteSystemException when upgrade is not possible.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatUpgradeMembershipWithoutStatusMessageThrowsReduceFoodWasteSystemExceptionWhenUpgradeIsNotPossible(string statusMessage)
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection(canUpgrade: false);
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanUpgrade), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            ReduceFoodWasteSystemException exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdMemberController.UpgradeMembership(Fixture.Create<string>(), statusMessage: statusMessage));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.MembershipUpgradeNotPossible));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeMembership without a status message throws an ReduceFoodWasteSystemException when upgrade is not possible.
        /// </summary>
        [Test]
        public void TestThatUpgradeMembershipWithStatusMessageThrowsReduceFoodWasteSystemExceptionWhenUpgradeIsNotPossible()
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection(canUpgrade: false);
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanUpgrade), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            ReduceFoodWasteSystemException exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdMemberController.UpgradeMembership(Fixture.Create<string>(), statusMessage: statusMessage));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.MembershipUpgradeNotPossible));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeMembership without an error message throws an ReduceFoodWasteSystemException when upgrade is not possible.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatUpgradeMembershipWithoutErrorMessageThrowsReduceFoodWasteSystemExceptionWhenUpgradeIsNotPossible(string errorMessage)
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection(canUpgrade: false);
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanUpgrade), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            ReduceFoodWasteSystemException exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdMemberController.UpgradeMembership(Fixture.Create<string>(), errorMessage: errorMessage));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.MembershipUpgradeNotPossible));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeMembership without an error message throws an ReduceFoodWasteSystemException when upgrade is not possible.
        /// </summary>
        [Test]
        public void TestThatUpgradeMembershipWithErrorMessageThrowsReduceFoodWasteSystemExceptionWhenUpgradeIsNotPossible()
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection(canUpgrade: false);
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanUpgrade), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            ReduceFoodWasteSystemException exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdMemberController.UpgradeMembership(Fixture.Create<string>(), errorMessage: errorMessage));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.MembershipUpgradeNotPossible));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeMembership without a status message returns a ViewResult with membership models for upgrading.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatUpgradeMembershipWithoutStatusMessageReturnsViewResultWithModelsForUpgrade(string statusMessage)
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanUpgrade), Is.True);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            ActionResult result = householdMemberController.UpgradeMembership(returnUrl, statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("UpgradeMembership"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(membershipModelCollection));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that UpgradeMembership without a status message returns a ViewResult with membership models for upgrading.
        /// </summary>
        [Test]
        public void TestThatUpgradeMembershipWithStatusMessageReturnsViewResultWithModelsForUpgrade()
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanUpgrade), Is.True);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            ActionResult result = householdMemberController.UpgradeMembership(returnUrl, statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("UpgradeMembership"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(membershipModelCollection));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
            Assert.That(viewResult.ViewData["StatusMessage"], Is.Not.Null);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.EqualTo(statusMessage));
        }

        /// <summary>
        /// Tests that UpgradeMembership without an error message returns a ViewResult with membership models for upgrading.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatUpgradeMembershipWithoutErrorMessageReturnsViewResultWithModelsForUpgrade(string errorMessage)
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanUpgrade), Is.True);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            ActionResult result = householdMemberController.UpgradeMembership(returnUrl, errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("UpgradeMembership"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(membershipModelCollection));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that UpgradeMembership without an error message returns a ViewResult with membership models for upgrading.
        /// </summary>
        [Test]
        public void TestThatUpgradeMembershipWithErrorMessageReturnsViewResultWithWithModelsForUpgrade()
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanUpgrade), Is.True);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            ActionResult result = householdMemberController.UpgradeMembership(returnUrl, errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("UpgradeMembership"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(membershipModelCollection));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.EqualTo(errorMessage));
        }

        /// <summary>
        /// Tests that RenewMembership throws an ArgumentNullException when the url on which to return when the renew process has finished is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatRenewMembershipThrowsArgumentNullExceptionWhenReturnUrlIsNullOrEmpty(string returnUrl)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.RenewMembership(returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("returnUrl"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RenewMembership without a status message calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatRenewMembershipWithoutStatusMessageCallsGetMembershipsAsyncOnHouseholdDataRepository(string statusMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            householdMemberController.RenewMembership(Fixture.Create<string>(), statusMessage: statusMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that RenewMembership with a status message calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatRenewMembershipWithStatusMessageCallsGetMembershipsAsyncOnHouseholdDataRepository()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            householdMemberController.RenewMembership(Fixture.Create<string>(), statusMessage: statusMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that RenewMembership without an error message calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatRenewMembershipWithoutErrorMessageCallsGetMembershipsAsyncOnHouseholdDataRepository(string errorMessage)
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            householdMemberController.RenewMembership(Fixture.Create<string>(), errorMessage: errorMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that RenewMembership with an error message calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatRenewMembershipWithErrorMessageCallsGetMembershipsAsyncOnHouseholdDataRepository()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            householdMemberController.RenewMembership(Fixture.Create<string>(), errorMessage: errorMessage);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that RenewMembership without a status message throws an ReduceFoodWasteSystemException when renew is not possible.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatRenewMembershipWithoutStatusMessageThrowsReduceFoodWasteSystemExceptionWhenRenewIsNotPossible(string statusMessage)
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection(canReview: false);
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanRenew), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            ReduceFoodWasteSystemException exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdMemberController.RenewMembership(Fixture.Create<string>(), statusMessage: statusMessage));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.MembershipRenewNotPossible));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RenewMembership without a status message throws an ReduceFoodWasteSystemException when renew is not possible.
        /// </summary>
        [Test]
        public void TestThatRenewMembershipWithStatusMessageThrowsReduceFoodWasteSystemExceptionWhenRenewIsNotPossible()
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection(canReview: false);
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanRenew), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            ReduceFoodWasteSystemException exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdMemberController.RenewMembership(Fixture.Create<string>(), statusMessage: statusMessage));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.MembershipRenewNotPossible));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RenewMembership without an error message throws an ReduceFoodWasteSystemException when renew is not possible.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatRenewMembershipWithoutErrorMessageThrowsReduceFoodWasteSystemExceptionWhenRenewIsNotPossible(string errorMessage)
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection(canReview: false);
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanRenew), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            ReduceFoodWasteSystemException exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdMemberController.RenewMembership(Fixture.Create<string>(), errorMessage: errorMessage));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.MembershipRenewNotPossible));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RenewMembership without an error message throws an ReduceFoodWasteSystemException when renew is not possible.
        /// </summary>
        [Test]
        public void TestThatRenewMembershipWithErrorMessageThrowsReduceFoodWasteSystemExceptionWhenRenewIsNotPossible()
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection(canReview: false);
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanRenew), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            ReduceFoodWasteSystemException exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdMemberController.RenewMembership(Fixture.Create<string>(), errorMessage: errorMessage));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.MembershipRenewNotPossible));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RenewMembership without a status message returns a ViewResult with a membership model for renewing.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatRenewMembershipWithoutStatusMessageReturnsViewResultWithWithModelForRenew(string statusMessage)
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanRenew), Is.True);

            MembershipModel membershipModel = membershipModelCollection.SingleOrDefault(m => m.CanRenew);
            Assert.That(membershipModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.True);

            ActionResult result = householdMemberController.RenewMembership(returnUrl, statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("RenewMembership"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(membershipModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that RenewMembership without a status message returns a ViewResult with a membership model for renewing.
        /// </summary>
        [Test]
        public void TestThatRenewMembershipWithStatusMessageReturnsViewResultWithWithModelForRenew()
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanRenew), Is.True);

            MembershipModel membershipModel = membershipModelCollection.SingleOrDefault(m => m.CanRenew);
            Assert.That(membershipModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            string statusMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(statusMessage), Is.False);

            ActionResult result = householdMemberController.RenewMembership(returnUrl, statusMessage: statusMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("RenewMembership"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(membershipModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
            Assert.That(viewResult.ViewData["StatusMessage"], Is.Not.Null);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["StatusMessage"], Is.EqualTo(statusMessage));
        }

        /// <summary>
        /// Tests that RenewMembership without an error message returns a ViewResult with a membership model for renewing.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatRenewMembershipWithoutErrorMessageReturnsViewResultWithWithModelForRenew(string errorMessage)
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanRenew), Is.True);

            MembershipModel membershipModel = membershipModelCollection.SingleOrDefault(m => m.CanRenew);
            Assert.That(membershipModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.True);

            ActionResult result = householdMemberController.RenewMembership(returnUrl, errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("RenewMembership"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(membershipModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that RenewMembership without an error message returns a ViewResult with a membership model for renewing.
        /// </summary>
        [Test]
        public void TestThatRenewMembershipWithErrorMessageReturnsViewResultWithWithModelForRenew()
        {
            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => m.CanRenew), Is.True);

            MembershipModel membershipModel = membershipModelCollection.SingleOrDefault(m => m.CanRenew);
            Assert.That(membershipModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            string errorMessage = Fixture.Create<string>();
            Assert.That(string.IsNullOrWhiteSpace(errorMessage), Is.False);

            ActionResult result = householdMemberController.RenewMembership(returnUrl, errorMessage: errorMessage);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("RenewMembership"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(membershipModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ErrorMessage"], Is.EqualTo(errorMessage));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model throws an ArgumentNullException when the model is null.
        /// </summary>
        [Test]
        public void TestThatUpgradeOrRenewMembershipWithModelThrowsArgumentNullExceptionWhenModelIsNull()
        {
            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.UpgradeOrRenewMembership(null, Fixture.Create<string>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("membershipModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model throws an ArgumentNullException when the url on which to return when the membership upgrade or renew process has finished is null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatUpgradeOrRenewMembershipWithModelThrowsArgumentNullExceptionWhenReturnUrlIsNull(string returnUrl)
        {
            MembershipModel membershipModel = Fixture.Create<MembershipModel>();
            Assert.That(membershipModel, Is.Not.Null);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("returnUrl"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is free of cost does not call GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase("Basic")]
        [TestCase("Deluxe")]
        [TestCase("Premium")]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsFreeOfCostDoesNotCallGetMembershipsAsyncOnHouseholdDataRepository(string membershipName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, 0M)
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, true)
                .With(m => m.CanUpgrade, true)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.EqualTo(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.True);
            Assert.That(membershipModel.CanUpgrade, Is.True);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is free of cost does not call ToBase64 with the model for the membership on the model helper.
        /// </summary>
        [Test]
        [TestCase("Basic")]
        [TestCase("Deluxe")]
        [TestCase("Premium")]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsFreeOfCostDoesNotCallToBase64WithMembershipModelOnModelHelper(string membershipName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, 0M)
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, true)
                .With(m => m.CanUpgrade, true)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.EqualTo(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.True);
            Assert.That(membershipModel.CanUpgrade, Is.True);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _modelHelperMock.AssertWasNotCalled(m => m.ToBase64(Arg<MembershipModel>.Is.TypeOf));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is free of cost does not call ActionToUrl on the utilities which support the infrastructure.
        /// </summary>
        [Test]
        [TestCase("Basic")]
        [TestCase("Deluxe")]
        [TestCase("Premium")]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsFreeOfCostDoesNotCallActionToUrlOnUtilities(string membershipName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, 0M)
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, true)
                .With(m => m.CanUpgrade, true)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.EqualTo(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.True);
            Assert.That(membershipModel.CanUpgrade, Is.True);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _utilitiesMock.AssertWasNotCalled(m => m.ActionToUrl(Arg<UrlHelper>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<RouteValueDictionary>.Is.Anything));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is free of cost returns a RedirectResult to the url on which to return when the membership upgrade or renew process has finished.
        /// </summary>
        [Test]
        [TestCase("Basic")]
        [TestCase("Deluxe")]
        [TestCase("Premium")]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsFreeOfCostReturnsRedirectResultToReturnUrl(string membershipName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, 0M)
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, true)
                .With(m => m.CanUpgrade, true)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.EqualTo(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.True);
            Assert.That(membershipModel.CanUpgrade, Is.True);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ActionResult result = householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectResult>());

            RedirectResult redirectResult = (RedirectResult) result;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Empty);
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but not renewable or upgradeable does not call GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase("Basic")]
        [TestCase("Deluxe")]
        [TestCase("Premium")]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostNotRenewableAndNotUpgradeableDoesNotCallGetMembershipsAsyncOnHouseholdDataRepository(string membershipName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, false)
                .With(m => m.CanUpgrade, false)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.False);
            Assert.That(membershipModel.CanUpgrade, Is.False);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but not renewable or upgradeable does not call ToBase64 with the model for the membership on the model helper.
        /// </summary>
        [Test]
        [TestCase("Basic")]
        [TestCase("Deluxe")]
        [TestCase("Premium")]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostNotRenewableAndNotUpgradeableDoesNotCallToBase64WithMembershipModelOnModelHelper(string membershipName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, false)
                .With(m => m.CanUpgrade, false)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.False);
            Assert.That(membershipModel.CanUpgrade, Is.False);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _modelHelperMock.AssertWasNotCalled(m => m.ToBase64(Arg<MembershipModel>.Is.TypeOf));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but not renewable or upgradeable does not call ActionToUrl on the utilities which support the infrastructure.
        /// </summary>
        [Test]
        [TestCase("Basic")]
        [TestCase("Deluxe")]
        [TestCase("Premium")]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostNotRenewableAndNotUpgradeableDoesNotCallActionToUrlOnUtilities(string membershipName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, false)
                .With(m => m.CanUpgrade, false)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.False);
            Assert.That(membershipModel.CanUpgrade, Is.False);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _utilitiesMock.AssertWasNotCalled(m => m.ActionToUrl(Arg<UrlHelper>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<RouteValueDictionary>.Is.Anything));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but not renewable or upgradeable returns a RedirectResult to the url on which to return when the membership upgrade or renew process has finished.
        /// </summary>
        [Test]
        [TestCase("Basic")]
        [TestCase("Deluxe")]
        [TestCase("Premium")]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostNotRenewableAndNotUpgradeableReturnsRedirectResultToReturnUrl(string membershipName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, false)
                .With(m => m.CanUpgrade, false)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.False);
            Assert.That(membershipModel.CanUpgrade, Is.False);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ActionResult result = householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectResult>());

            RedirectResult redirectResult = (RedirectResult) result;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Empty);
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but renewable or upgradeable calls GetMembershipsAsync on the repository which can access household data.
        /// </summary>
        [Test]
        [TestCase("Basic", true, true)]
        [TestCase("Basic", true, false)]
        [TestCase("Basic", false, true)]
        [TestCase("Deluxe", true, true)]
        [TestCase("Deluxe", true, false)]
        [TestCase("Deluxe", false, true)]
        [TestCase("Premium", true, true)]
        [TestCase("Premium", true, false)]
        [TestCase("Premium", false, true)]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostAndRenewableOrUpgradeableCallsGetMembershipsAsyncOnHouseholdDataRepository(string membershipName, bool canRenew, bool canUpgrade)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, canRenew)
                .With(m => m.CanUpgrade, canUpgrade)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.EqualTo(canRenew));
            Assert.That(membershipModel.CanUpgrade, Is.EqualTo(canUpgrade));

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but renewable or upgradeable does not call ToBase64 with the model for the membership on the model helper when the name of the membership is unknown.
        /// </summary>
        [Test]
        [TestCase("XXX", true, true)]
        [TestCase("XXX", true, false)]
        [TestCase("XXX", false, true)]
        [TestCase("YYY", true, true)]
        [TestCase("YYY", true, false)]
        [TestCase("YYY", false, true)]
        [TestCase("ZZZ", true, true)]
        [TestCase("ZZZ", true, false)]
        [TestCase("ZZZ", false, true)]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostAndRenewableOrUpgradeableDoesNotCallToBase64WithMembershipModelOnModelHelperWhenNameOfMembershipIsUnknown(string membershipName, bool canRenew, bool canUpgrade)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, canRenew)
                .With(m => m.CanUpgrade, canUpgrade)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.EqualTo(canRenew));
            Assert.That(membershipModel.CanUpgrade, Is.EqualTo(canUpgrade));

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => string.Compare(m.Name, membershipName, StringComparison.Ordinal) == 0), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _modelHelperMock.AssertWasNotCalled(m => m.ToBase64(Arg<MembershipModel>.Is.TypeOf));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but renewable or upgradeable does not call ActionToUrl on the utilities which support the infrastructure when the name of the membership is unknown.
        /// </summary>
        [Test]
        [TestCase("XXX", true, true)]
        [TestCase("XXX", true, false)]
        [TestCase("XXX", false, true)]
        [TestCase("YYY", true, true)]
        [TestCase("YYY", true, false)]
        [TestCase("YYY", false, true)]
        [TestCase("ZZZ", true, true)]
        [TestCase("ZZZ", true, false)]
        [TestCase("ZZZ", false, true)]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostAndRenewableOrUpgradeableDoesNotCallActionToUrlOnUtilitiesWhenNameOfMembershipIsUnknown(string membershipName, bool canRenew, bool canUpgrade)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, canRenew)
                .With(m => m.CanUpgrade, canUpgrade)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.EqualTo(canRenew));
            Assert.That(membershipModel.CanUpgrade, Is.EqualTo(canUpgrade));

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => string.Compare(m.Name, membershipName, StringComparison.Ordinal) == 0), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _utilitiesMock.AssertWasNotCalled(m => m.ActionToUrl(Arg<UrlHelper>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<RouteValueDictionary>.Is.Anything));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but renewable or upgradeable returns a RedirectResult to the url on which to return when the membership upgrade or renew process has finished when the name of the membership is unknown.
        /// </summary>
        [Test]
        [TestCase("XXX", true, true)]
        [TestCase("XXX", true, false)]
        [TestCase("XXX", false, true)]
        [TestCase("YYY", true, true)]
        [TestCase("YYY", true, false)]
        [TestCase("YYY", false, true)]
        [TestCase("ZZZ", true, true)]
        [TestCase("ZZZ", true, false)]
        [TestCase("ZZZ", false, true)]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostAndRenewableOrUpgradeableReturnsRedirectResultToReturnUrlWhenNameOfMembershipIsUnknown(string membershipName, bool canRenew, bool canUpgrade)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, canRenew)
                .With(m => m.CanUpgrade, canUpgrade)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.EqualTo(canRenew));
            Assert.That(membershipModel.CanUpgrade, Is.EqualTo(canUpgrade));

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => string.Compare(m.Name, membershipName, StringComparison.Ordinal) == 0), Is.False);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection);
            Assert.That(householdMemberController, Is.Not.Null);

            ActionResult result = householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectResult>());

            RedirectResult redirectResult = (RedirectResult) result;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Empty);
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but renewable or upgradeable calls ToBase64 with the model for the membership on the model helper when name of membership is known.
        /// </summary>
        [Test]
        [TestCase("Basic", true, true)]
        [TestCase("Basic", true, false)]
        [TestCase("Basic", false, true)]
        [TestCase("Deluxe", true, true)]
        [TestCase("Deluxe", true, false)]
        [TestCase("Deluxe", false, true)]
        [TestCase("Premium", true, true)]
        [TestCase("Premium", true, false)]
        [TestCase("Premium", false, true)]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostAndRenewableOrUpgradeableCallsToBase64WithMembershipModelOnModelHelperWhenNameOfMembershipIsKnown(string membershipName, bool canRenew, bool canUpgrade)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, canRenew)
                .With(m => m.CanUpgrade, canUpgrade)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.EqualTo(canRenew));
            Assert.That(membershipModel.CanUpgrade, Is.EqualTo(canUpgrade));

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => string.Compare(m.Name, membershipName, StringComparison.Ordinal) == 0), Is.True);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection, toBase64ForMembershipModelCallback: m =>
            {
                Assert.That(m, Is.Not.Null);
                Assert.That(m, Is.EqualTo(membershipModel));
                Assert.That(m.BillingInformation, Is.Not.Null);
                Assert.That(m.BillingInformation, Is.Not.Empty);
                Assert.That(m.BillingInformation, Is.EqualTo(membershipModelCollection.Single(n => string.Compare(n.Name, membershipName, StringComparison.Ordinal) == 0).BillingInformation));
                Assert.That(m.Description, Is.Not.Null);
                Assert.That(m.Description, Is.Not.Empty);
                Assert.That(m.Description, Is.EqualTo(membershipModelCollection.Single(n => string.Compare(n.Name, membershipName, StringComparison.Ordinal) == 0).Description));
            });
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _modelHelperMock.AssertWasCalled(m => m.ToBase64(Arg<MembershipModel>.Is.TypeOf));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but renewable or upgradeable calls ActionToUrl on the utilities which support the infrastructure when name of membership is known.
        /// </summary>
        [Test]
        [TestCase("Basic", true, true)]
        [TestCase("Basic", true, false)]
        [TestCase("Basic", false, true)]
        [TestCase("Deluxe", true, true)]
        [TestCase("Deluxe", true, false)]
        [TestCase("Deluxe", false, true)]
        [TestCase("Premium", true, true)]
        [TestCase("Premium", true, false)]
        [TestCase("Premium", false, true)]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostAndRenewableOrUpgradeableCallsActionToUrlOnUtilitiesWhenNameOfMembershipIsKnown(string membershipName, bool canRenew, bool canUpgrade)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, canRenew)
                .With(m => m.CanUpgrade, canUpgrade)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.EqualTo(canRenew));
            Assert.That(membershipModel.CanUpgrade, Is.EqualTo(canUpgrade));

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => string.Compare(m.Name, membershipName, StringComparison.Ordinal) == 0), Is.True);

            string toBase64ForMembershipModel = Fixture.Create<string>();
            Assert.That(toBase64ForMembershipModel, Is.Not.Null);
            Assert.That(toBase64ForMembershipModel, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection, toBase64ForMembershipModel: toBase64ForMembershipModel, actionToUrlCallback: routeValueDictionary =>
            {
                Assert.That(routeValueDictionary, Is.Not.Null);
                Assert.That(routeValueDictionary, Is.Not.Empty);
                Assert.That(routeValueDictionary.ContainsKey("membershipModelAsBase64"), Is.True);
                Assert.That(routeValueDictionary["membershipModelAsBase64"], Is.Not.Null);
                Assert.That(routeValueDictionary["membershipModelAsBase64"], Is.Not.Empty);
                Assert.That(routeValueDictionary["membershipModelAsBase64"], Is.EqualTo(toBase64ForMembershipModel));
                Assert.That(routeValueDictionary.ContainsKey("paymentModelAsBase64"), Is.True);
                Assert.That(routeValueDictionary["paymentModelAsBase64"], Is.Not.Null);
                Assert.That(routeValueDictionary["paymentModelAsBase64"], Is.Not.Empty);
                Assert.That(routeValueDictionary["paymentModelAsBase64"], Is.EqualTo("[PaymentModelAsBase64]"));
                Assert.That(routeValueDictionary.ContainsKey("returnUrl"), Is.True);
                Assert.That(routeValueDictionary["returnUrl"], Is.Not.Null);
                Assert.That(routeValueDictionary["returnUrl"], Is.Not.Empty);
                Assert.That(routeValueDictionary["returnUrl"], Is.EqualTo(returnUrl));
            });
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.Url, Is.Null);

            householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);

            _utilitiesMock.AssertWasCalled(m => m.ActionToUrl(Arg<UrlHelper>.Is.Equal(householdMemberController.Url), Arg<string>.Is.Equal("UpgradeOrRenewMembershipCallback"), Arg<string>.Is.Equal("HouseholdMember"), Arg<RouteValueDictionary>.Is.NotNull));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembership with a model where the membership is not free of cost but renewable or upgradeable returns a RedirectToRouteResult to the payment when name of membership is known.
        /// </summary>
        [Test]
        [TestCase("Basic", true, true)]
        [TestCase("Basic", true, false)]
        [TestCase("Basic", false, true)]
        [TestCase("Deluxe", true, true)]
        [TestCase("Deluxe", true, false)]
        [TestCase("Deluxe", false, true)]
        [TestCase("Premium", true, true)]
        [TestCase("Premium", true, false)]
        [TestCase("Premium", false, true)]
        public void TestThatUpgradeOrRenewMembershipWithModelWhereMembershipIsNotFreeOfCostAndRenewableOrUpgradeableReturnsRedirectToRouteResultToPayWhenNameOfMembershipIsKnown(string membershipName, bool canRenew, bool canUpgrade)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, membershipName)
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.CanRenew, canRenew)
                .With(m => m.CanUpgrade, canUpgrade)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Name, Is.EqualTo(membershipName));
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.CanRenew, Is.EqualTo(canRenew));
            Assert.That(membershipModel.CanUpgrade, Is.EqualTo(canUpgrade));

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            IList<MembershipModel> membershipModelCollection = CreateMembershipModelCollection();
            Assert.That(membershipModelCollection, Is.Not.Null);
            Assert.That(membershipModelCollection, Is.Not.Empty);
            Assert.That(membershipModelCollection.Any(m => string.Compare(m.Name, membershipName, StringComparison.Ordinal) == 0), Is.True);

            string toBase64ForMembershipModel = Fixture.Create<string>();
            Assert.That(toBase64ForMembershipModel, Is.Not.Null);
            Assert.That(toBase64ForMembershipModel, Is.Not.Empty);

            string actionToUrl = Fixture.Create<string>();
            Assert.That(actionToUrl, Is.Not.Null);
            Assert.That(actionToUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(membershipModelCollection: membershipModelCollection, toBase64ForMembershipModel: toBase64ForMembershipModel, actionToUrl: actionToUrl);
            Assert.That(householdMemberController, Is.Not.Null);

            var result = householdMemberController.UpgradeOrRenewMembership(membershipModel, returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            RedirectToRouteResult redirectToRouteResult = (RedirectToRouteResult) result;
            Assert.That(redirectToRouteResult, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues, Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("action"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Pay"));
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("controller"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["controller"], Is.EqualTo("Payment"));
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("payableModelAsBase64"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["payableModelAsBase64"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["payableModelAsBase64"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["payableModelAsBase64"], Is.EqualTo(toBase64ForMembershipModel));
            Assert.That(redirectToRouteResult.RouteValues.ContainsKey("returnUrl"), Is.True);
            Assert.That(redirectToRouteResult.RouteValues["returnUrl"], Is.Not.Null);
            Assert.That(redirectToRouteResult.RouteValues["returnUrl"], Is.Not.Empty);
            Assert.That(redirectToRouteResult.RouteValues["returnUrl"],Is.EqualTo(actionToUrl));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback throws an ArgumentNullException when the base64 encoded value for the model of the membership which should be upgraded or renewed is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatUpgradeOrRenewMembershipCallbackThrowsArgumentNullExceptionWhenMembershipModelAsBase64IsNullOrEmpty(string membershipModelAsBase64)
        {
            string paymentModelAsBase64 = Fixture.Create<string>();
            Assert.That(paymentModelAsBase64, Is.Not.Null);
            Assert.That(paymentModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("membershipModelAsBase64"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback throws an ArgumentNullException when the base64 encoded value for a payable model which contains information about the payment is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatUpgradeOrRenewMembershipCallbackThrowsArgumentNullExceptionWhenPaymentModelAsBase64IsNullOrEmpty(string paymentModelAsBase64)
        {
            string membershipModelAsBase64 = Fixture.Create<string>();
            Assert.That(membershipModelAsBase64, Is.Not.Null);
            Assert.That(membershipModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("paymentModelAsBase64"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback throws an ArgumentNullException when the url on which to return when the upgrade process has finished is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatUpgradeOrRenewMembershipCallbackThrowsArgumentNullExceptionWhenReturnUrlIsNullOrEmpty(string returnUrl)
        {
            string membershipModelAsBase64 = Fixture.Create<string>();
            Assert.That(membershipModelAsBase64, Is.Not.Null);
            Assert.That(membershipModelAsBase64, Is.Not.Empty);

            string paymentModelAsBase64 = Fixture.Create<string>();
            Assert.That(paymentModelAsBase64, Is.Not.Null);
            Assert.That(paymentModelAsBase64, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController();
            Assert.That(householdMemberController, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("returnUrl"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback calls ToModel with the base64 encoded value for the model of the membership which should be upgraded or renewed on the model helper.
        /// </summary>
        [Test]
        public void TestThatUpgradeOrRenewMembershipCallbackCallsToModelWithMembershipModelAsBase64OnModelHelper()
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, (Guid?) null)
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentTime, (DateTime?) null)
                .With(m => m.PaymentReference, (string) null)
                .With(m => m.PaymentReceipt, (string) null)
                .With(m => m.CanRenew, Fixture.Create<bool>())
                .With(m => m.CanUpgrade, Fixture.Create<bool>())
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(membershipModel.PaymentHandlers, Is.Null);
            Assert.That(membershipModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(membershipModel.PaymentTime, Is.Null);
            Assert.That(membershipModel.PaymentReference, Is.Null);
            Assert.That(membershipModel.PaymentReceipt, Is.Null);

            string membershipModelAsBase64 = Convert.ToString(membershipModel.GetHashCode());
            Assert.That(membershipModelAsBase64, Is.Not.Null);
            Assert.That(membershipModelAsBase64, Is.Not.Empty);

            PayableModel paymentModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Paid)
                .With(m => m.PaymentTime, DateTime.Now)
                .With(m => m.PaymentReference, Fixture.Create<string>())
                .With(m => m.PaymentReceipt, Fixture.Create<string>())
                .Create();
            Assert.That(paymentModel, Is.Not.Null);
            Assert.That(paymentModel.Price, Is.GreaterThan(0M));
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(paymentModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(paymentModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(paymentModel.PaymentHandlers, Is.Null);
            Assert.That(paymentModel.PaymentStatus, Is.EqualTo(PaymentStatus.Paid));
            Assert.That(paymentModel.PaymentTime, Is.Not.Null);
            Assert.That(paymentModel.PaymentReference, Is.Not.Null);
            Assert.That(paymentModel.PaymentReference, Is.Not.Empty);
            Assert.That(paymentModel.PaymentReceipt, Is.Not.Null);
            Assert.That(paymentModel.PaymentReceipt, Is.Not.Empty);

            string paymentModelAsBase64 = Convert.ToString(paymentModel.GetHashCode());
            Assert.That(paymentModelAsBase64, Is.Not.Null);
            Assert.That(paymentModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(toMembershipModel: membershipModel, toPaymentModel: paymentModel);
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl);

            _modelHelperMock.AssertWasCalled(m => m.ToModel(Arg<string>.Is.Equal(membershipModelAsBase64)));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback calls ToModel with the base64 encoded value for the model of the payment on the model helper.
        /// </summary>
        [Test]
        public void TestThatUpgradeOrRenewMembershipCallbackCallsToModelWithPaymentModelAsBase64OnModelHelper()
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, (Guid?) null)
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentTime, (DateTime?) null)
                .With(m => m.PaymentReference, (string) null)
                .With(m => m.PaymentReceipt, (string) null)
                .With(m => m.CanRenew, Fixture.Create<bool>())
                .With(m => m.CanUpgrade, Fixture.Create<bool>())
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(membershipModel.PaymentHandlers, Is.Null);
            Assert.That(membershipModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(membershipModel.PaymentTime, Is.Null);
            Assert.That(membershipModel.PaymentReference, Is.Null);
            Assert.That(membershipModel.PaymentReceipt, Is.Null);

            string membershipModelAsBase64 = Convert.ToString(membershipModel.GetHashCode());
            Assert.That(membershipModelAsBase64, Is.Not.Null);
            Assert.That(membershipModelAsBase64, Is.Not.Empty);

            PayableModel paymentModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Paid)
                .With(m => m.PaymentTime, DateTime.Now)
                .With(m => m.PaymentReference, Fixture.Create<string>())
                .With(m => m.PaymentReceipt, Fixture.Create<string>())
                .Create();
            Assert.That(paymentModel, Is.Not.Null);
            Assert.That(paymentModel.Price, Is.GreaterThan(0M));
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(paymentModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(paymentModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(paymentModel.PaymentHandlers, Is.Null);
            Assert.That(paymentModel.PaymentStatus, Is.EqualTo(PaymentStatus.Paid));
            Assert.That(paymentModel.PaymentTime, Is.Not.Null);
            Assert.That(paymentModel.PaymentReference, Is.Not.Null);
            Assert.That(paymentModel.PaymentReference, Is.Not.Empty);
            Assert.That(paymentModel.PaymentReceipt, Is.Not.Null);
            Assert.That(paymentModel.PaymentReceipt, Is.Not.Empty);

            string paymentModelAsBase64 = Convert.ToString(paymentModel.GetHashCode());
            Assert.That(paymentModelAsBase64, Is.Not.Null);
            Assert.That(paymentModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(toMembershipModel: membershipModel, toPaymentModel: paymentModel);
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl);

            _modelHelperMock.AssertWasCalled(m => m.ToModel(Arg<string>.Is.Equal(paymentModelAsBase64)));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback does not call UpgradeMembershipAsync on the repository which can access household data when the model of the payment has not been paid.
        /// </summary>
        [Test]
        [TestCase(PaymentStatus.Unpaid)]
        [TestCase(PaymentStatus.Cancelled)]
        public void TestThatUpgradeOrRenewMembershipCallbackDoesNotCallUpgradeMembershipAsyncOnHouseholdDataRepositoryWhenPaymentModelHasNotBeenPaid(PaymentStatus paymentStatus)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, (Guid?) null)
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentTime, (DateTime?) null)
                .With(m => m.PaymentReference, (string) null)
                .With(m => m.PaymentReceipt, (string) null)
                .With(m => m.CanRenew, Fixture.Create<bool>())
                .With(m => m.CanUpgrade, Fixture.Create<bool>())
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(membershipModel.PaymentHandlers, Is.Null);
            Assert.That(membershipModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(membershipModel.PaymentTime, Is.Null);
            Assert.That(membershipModel.PaymentReference, Is.Null);
            Assert.That(membershipModel.PaymentReceipt, Is.Null);

            string membershipModelAsBase64 = Convert.ToString(membershipModel.GetHashCode());
            Assert.That(membershipModelAsBase64, Is.Not.Null);
            Assert.That(membershipModelAsBase64, Is.Not.Empty);

            PayableModel paymentModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, paymentStatus)
                .With(m => m.PaymentTime, (DateTime?) null)
                .With(m => m.PaymentReference, (string) null)
                .With(m => m.PaymentReceipt, (string) null)
                .Create();
            Assert.That(paymentModel, Is.Not.Null);
            Assert.That(paymentModel.Price, Is.GreaterThan(0M));
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(paymentModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(paymentModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(paymentModel.PaymentHandlers, Is.Null);
            Assert.That(paymentModel.PaymentStatus, Is.EqualTo(paymentStatus));
            Assert.That(paymentModel.PaymentTime, Is.Null);
            Assert.That(paymentModel.PaymentReference, Is.Null);
            Assert.That(paymentModel.PaymentReceipt, Is.Null);

            string paymentModelAsBase64 = Convert.ToString(paymentModel.GetHashCode());
            Assert.That(paymentModelAsBase64, Is.Not.Null);
            Assert.That(paymentModelAsBase64, Is.Not.Empty);

            string  returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(toMembershipModel: membershipModel, toPaymentModel: paymentModel);
            Assert.That(householdMemberController, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.UpgradeMembershipAsync(Arg<IIdentity>.Is.Anything, Arg<MembershipModel>.Is.Anything));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback returns a RedirectResult to the url on which to return when the membership upgrade or renew process has finished when the model of the payment has not been paid.
        /// </summary>
        [Test]
        [TestCase(PaymentStatus.Unpaid)]
        [TestCase(PaymentStatus.Cancelled)]
        public void TestThatUpgradeOrRenewMembershipCallbackReturnsRedirectResultToReturnUrlWhenPaymentModelHasNotBeenPaid(PaymentStatus paymentStatus)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, (Guid?) null)
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentTime, (DateTime?) null)
                .With(m => m.PaymentReference, (string) null)
                .With(m => m.PaymentReceipt, (string) null)
                .With(m => m.CanRenew, Fixture.Create<bool>())
                .With(m => m.CanUpgrade, Fixture.Create<bool>())
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(membershipModel.PaymentHandlers, Is.Null);
            Assert.That(membershipModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(membershipModel.PaymentTime, Is.Null);
            Assert.That(membershipModel.PaymentReference, Is.Null);
            Assert.That(membershipModel.PaymentReceipt, Is.Null);

            string membershipModelAsBase64 = Convert.ToString(membershipModel.GetHashCode());
            Assert.That(membershipModelAsBase64, Is.Not.Null);
            Assert.That(membershipModelAsBase64, Is.Not.Empty);

            PayableModel paymentModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, paymentStatus)
                .With(m => m.PaymentTime, (DateTime?) null)
                .With(m => m.PaymentReference, (string) null)
                .With(m => m.PaymentReceipt, (string) null)
                .Create();
            Assert.That(paymentModel, Is.Not.Null);
            Assert.That(paymentModel.Price, Is.GreaterThan(0M));
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(paymentModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(paymentModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(paymentModel.PaymentHandlers, Is.Null);
            Assert.That(paymentModel.PaymentStatus, Is.EqualTo(paymentStatus));
            Assert.That(paymentModel.PaymentTime, Is.Null);
            Assert.That(paymentModel.PaymentReference, Is.Null);
            Assert.That(paymentModel.PaymentReceipt, Is.Null);

            string paymentModelAsBase64 = Convert.ToString(paymentModel.GetHashCode());
            Assert.That(paymentModelAsBase64, Is.Not.Null);
            Assert.That(paymentModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(toMembershipModel: membershipModel, toPaymentModel: paymentModel);
            Assert.That(householdMemberController, Is.Not.Null);

            ActionResult result = householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectResult>());

            RedirectResult redirectResult = (RedirectResult) result;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Empty);
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback calls UpgradeMembershipAsync on the repository which can access household data when the model of the payment has not been paid.
        /// </summary>
        [Test]
        public void TestThatUpgradeOrRenewMembershipCallbackCallsUpgradeMembershipAsyncOnHouseholdDataRepositoryWhenPaymentModelHasBeenPaid()
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, (Guid?) null)
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentTime, (DateTime?) null)
                .With(m => m.PaymentReference, (string) null)
                .With(m => m.PaymentReceipt, (string) null)
                .With(m => m.CanRenew, Fixture.Create<bool>())
                .With(m => m.CanUpgrade, Fixture.Create<bool>())
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(membershipModel.PaymentHandlers, Is.Null);
            Assert.That(membershipModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(membershipModel.PaymentTime, Is.Null);
            Assert.That(membershipModel.PaymentReference, Is.Null);
            Assert.That(membershipModel.PaymentReceipt, Is.Null);

            string membershipModelAsBase64 = Convert.ToString(membershipModel.GetHashCode());
            Assert.That(membershipModelAsBase64, Is.Not.Null);
            Assert.That(membershipModelAsBase64, Is.Not.Empty);

            PayableModel paymentModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Paid)
                .With(m => m.PaymentTime, DateTime.Now)
                .With(m => m.PaymentReference, Fixture.Create<string>())
                .With(m => m.PaymentReceipt, Fixture.Create<string>())
                .Create();
            Assert.That(paymentModel, Is.Not.Null);
            Assert.That(paymentModel.Price, Is.GreaterThan(0M));
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(paymentModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(paymentModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(paymentModel.PaymentHandlers, Is.Null);
            Assert.That(paymentModel.PaymentStatus, Is.EqualTo(PaymentStatus.Paid));
            Assert.That(paymentModel.PaymentTime, Is.Not.Null);
            Assert.That(paymentModel.PaymentReference, Is.Not.Null);
            Assert.That(paymentModel.PaymentReference, Is.Not.Empty);
            Assert.That(paymentModel.PaymentReceipt, Is.Not.Null);
            Assert.That(paymentModel.PaymentReceipt, Is.Not.Empty);

            string paymentModelAsBase64 = Convert.ToString(paymentModel.GetHashCode());
            Assert.That(paymentModelAsBase64, Is.Not.Null);
            Assert.That(paymentModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(toMembershipModel: membershipModel, toPaymentModel: paymentModel);
            Assert.That(householdMemberController, Is.Not.Null);
            Assert.That(householdMemberController.User, Is.Not.Null);
            Assert.That(householdMemberController.User.Identity, Is.Not.Null);

            householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl);

            _householdDataRepositoryMock.AssertWasCalled(m => m.UpgradeMembershipAsync(
                    Arg<IIdentity>.Is.Equal(householdMemberController.User.Identity),
                    Arg<MembershipModel>.Matches(model =>
                        model != null &&
                        model == membershipModel &&
                        model.PaymentHandlerIdentifier != null &&
                        model.PaymentHandlerIdentifier.Value == paymentModel.PaymentHandlerIdentifier.Value &&
                        model.PaymentStatus == PaymentStatus.Paid &&
                        model.PaymentTime != null &&
                        model.PaymentTime.Value == paymentModel.PaymentTime.Value &&
                        string.IsNullOrWhiteSpace(model.PaymentReference) == false &&
                        string.Compare(model.PaymentReference, paymentModel.PaymentReference, StringComparison.Ordinal) == 0 &&
                        string.IsNullOrWhiteSpace(model.PaymentReceipt) == false &&
                        string.Compare(model.PaymentReceipt, paymentModel.PaymentReceipt, StringComparison.Ordinal) == 0
                    )),
                opt => opt.Repeat.Once());
        }

        /// <summary>
        /// Tests that UpgradeOrRenewMembershipCallback returns a RedirectResult to the url on which to return when the membership upgrade or renew process has finished when the model of the payment has not been paid.
        /// </summary>
        [Test]
        public void TestThatUpgradeOrRenewMembershipCallbackReturnsRedirectResultToReturnUrlWhenPaymentModelHasBeenPaid()
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, (Guid?) null)
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentTime, (DateTime?) null)
                .With(m => m.PaymentReference, (string) null)
                .With(m => m.PaymentReceipt, (string) null)
                .With(m => m.CanRenew, Fixture.Create<bool>())
                .With(m => m.CanUpgrade, Fixture.Create<bool>())
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Price, Is.GreaterThan(0M));
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(membershipModel.PaymentHandlers, Is.Null);
            Assert.That(membershipModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(membershipModel.PaymentTime, Is.Null);
            Assert.That(membershipModel.PaymentReference, Is.Null);
            Assert.That(membershipModel.PaymentReceipt, Is.Null);

            string membershipModelAsBase64 = Convert.ToString(membershipModel.GetHashCode());
            Assert.That(membershipModelAsBase64, Is.Not.Null);
            Assert.That(membershipModelAsBase64, Is.Not.Empty);

            PayableModel paymentModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, (IEnumerable<PaymentHandlerModel>) null)
                .With(m => m.PaymentStatus, PaymentStatus.Paid)
                .With(m => m.PaymentTime, DateTime.Now)
                .With(m => m.PaymentReference, Fixture.Create<string>())
                .With(m => m.PaymentReceipt, Fixture.Create<string>())
                .Create();
            Assert.That(paymentModel, Is.Not.Null);
            Assert.That(paymentModel.Price, Is.GreaterThan(0M));
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(paymentModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(paymentModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(paymentModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(paymentModel.PaymentHandlers, Is.Null);
            Assert.That(paymentModel.PaymentStatus, Is.EqualTo(PaymentStatus.Paid));
            Assert.That(paymentModel.PaymentTime, Is.Not.Null);
            Assert.That(paymentModel.PaymentReference, Is.Not.Null);
            Assert.That(paymentModel.PaymentReference, Is.Not.Empty);
            Assert.That(paymentModel.PaymentReceipt, Is.Not.Null);
            Assert.That(paymentModel.PaymentReceipt, Is.Not.Empty);

            string paymentModelAsBase64 = Convert.ToString(paymentModel.GetHashCode());
            Assert.That(paymentModelAsBase64, Is.Not.Null);
            Assert.That(paymentModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            WebApplication.Controllers.HouseholdMemberController householdMemberController = CreateHouseholdMemberController(toMembershipModel: membershipModel, toPaymentModel: paymentModel);
            Assert.That(householdMemberController, Is.Not.Null);

            ActionResult result = householdMemberController.UpgradeOrRenewMembershipCallback(membershipModelAsBase64, paymentModelAsBase64, returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectResult>());

            RedirectResult redirectResult = (RedirectResult) result;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Empty);
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
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
        /// <param name="membershipModelCollection">Sets the collection of membership models.</param>
        /// <param name="toBase64ForMembershipModel">Sets the encoded base64 value for a given membership model.</param>
        /// <param name="toBase64ForMembershipModelCallback">Sets the callback method called when encoding the base64 value for a given membership model.</param>
        /// <param name="toMembershipModel">Sets the model for the membership which should be returned for a given encoded base64 value.</param>
        /// <param name="toPaymentModel">Sets the model for the payment which should be returned for a given encoded base64 value.</param>
        /// <param name="actionToUrl">Set the url which should be returned when an action should be converted to an url.</param>
        /// <param name="actionToUrlCallback">Sets the callback method called when converting an action to an url.</param>
        /// <returns>Controller for a household member for unit testing.</returns>
        private WebApplication.Controllers.HouseholdMemberController CreateHouseholdMemberController(PrivacyPolicyModel privacyPolicyModel = null, bool isActivatedHouseholdMember = false, bool isPrivacyPoliciesAccepted = false, IPrincipal principal = null, Claim createdHouseholdMemberClaim = null, Claim activatedHouseholdMemberClaim = null, Claim privacyPoliciesAcceptedClaim = null, Claim validatedHouseholdMemberClaim = null, HouseholdMemberModel activatedHouseholdMemberModel = null, PrivacyPolicyModel acceptedPrivacyPolicyModel = null, HouseholdMemberModel householdMemberModel = null, IEnumerable<MembershipModel> membershipModelCollection = null, string toBase64ForMembershipModel = null, Action<MembershipModel> toBase64ForMembershipModelCallback = null, MembershipModel toMembershipModel = null, PayableModel toPaymentModel = null, string actionToUrl = null, Action<RouteValueDictionary> actionToUrlCallback = null)
        {
            _householdDataRepositoryMock.Stub(m => m.GetPrivacyPoliciesAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(() => privacyPolicyModel ?? Fixture.Create<PrivacyPolicyModel>()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.CreateHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.FromResult(Fixture.Build<HouseholdModel>()
                    .With(m => m.HouseholdMembers, (IEnumerable<MemberOfHouseholdModel>) null)
                    .Create()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.ActivateHouseholdMemberAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdMemberModel>.Is.Anything))
                .Return(Task.FromResult(activatedHouseholdMemberModel ?? Fixture.Build<HouseholdMemberModel>()
                    .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                    .Create()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.AcceptPrivacyPolicyAsync(Arg<IIdentity>.Is.Anything, Arg<PrivacyPolicyModel>.Is.Anything))
                .Return(Task.Run(() => acceptedPrivacyPolicyModel ?? Fixture.Create<PrivacyPolicyModel>()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.GetHouseholdMemberAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.FromResult(householdMemberModel ?? Fixture.Build<HouseholdMemberModel>()
                    .With(m => m.Households, (IEnumerable<HouseholdModel>) null)
                    .Create()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.GetMembershipsAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.FromResult(membershipModelCollection ?? CreateMembershipModelCollection()))
                .Repeat.Any();
            _householdDataRepositoryMock.Stub(m => m.UpgradeMembershipAsync(Arg<IIdentity>.Is.Anything, Arg<MembershipModel>.Is.Anything))
                .Return(Task.Run(() => Fixture.Create<MembershipModel>()))
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

            _modelHelperMock.Stub(m => m.ToBase64(Arg<MembershipModel>.Is.TypeOf))
                .WhenCalled(e =>
                {
                    if (toBase64ForMembershipModelCallback == null)
                    {
                        return;
                    }
                    toBase64ForMembershipModelCallback.Invoke((MembershipModel) e.Arguments.ElementAt(0));
                })
                .Return(toBase64ForMembershipModel ?? Fixture.Create<string>())
                .Repeat.Any();
            _modelHelperMock.Stub(m => m.ToModel(Arg<string>.Is.Anything))
                .WhenCalled(e =>
                {
                    string modelAsBase64 = (string) e.Arguments.ElementAt(0);
                    if (toMembershipModel != null && string.Compare(modelAsBase64, Convert.ToString(toMembershipModel.GetHashCode()), StringComparison.Ordinal) == 0)
                    {
                        e.ReturnValue = toMembershipModel;
                        return;
                    }
                    if (toPaymentModel != null && string.Compare(modelAsBase64, Convert.ToString(toPaymentModel.GetHashCode()), StringComparison.Ordinal) == 0)
                    {
                        e.ReturnValue = toPaymentModel;
                        return;
                    }
                    e.ReturnValue = null;
                })
                .Return(null)
                .Repeat.Any();

            _utilitiesMock.Stub(m => m.ActionToUrl(Arg<UrlHelper>.Is.Anything, Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<RouteValueDictionary>.Is.Anything))
                .WhenCalled(e =>
                {
                    if (actionToUrlCallback == null)
                    {
                        e.ReturnValue = actionToUrl ?? Fixture.Create<string>();
                        return;
                    }
                    actionToUrlCallback.Invoke((RouteValueDictionary) e.Arguments.ElementAt(3));
                    e.ReturnValue = actionToUrl ?? Fixture.Create<string>();
                })
                .Return(null)
                .Repeat.Any();

            var householdMemberController = new WebApplication.Controllers.HouseholdMemberController(_householdDataRepositoryMock, _claimValueProviderMock, _localClaimProviderMock, _modelHelperMock, _utilitiesMock);
            householdMemberController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdMemberController, principal: principal);
            return householdMemberController;
        }

        /// <summary>
        /// Creates a default collection of membership models.
        /// </summary>
        /// <param name="canReview">Sets whether it should be possible to renew one of the memberships.</param>
        /// <param name="canUpgrade">Sets whether it should be possible to upgrade one or more memberships.</param>
        /// <returns>Default collection of membership models.</returns>
        private IList<MembershipModel> CreateMembershipModelCollection(bool canReview = true, bool canUpgrade = true)
        {
            return new List<MembershipModel>
            {
                Fixture.Build<MembershipModel>()
                    .With(m => m.Name, "Basic")
                    .With(m => m.Price, 0M)
                    .With(m => m.PriceCultureInfoName, "en-US")
                    .With(m => m.CanRenew, false)
                    .With(m => m.CanUpgrade, false)
                    .Create(),
                Fixture.Build<MembershipModel>()
                    .With(m => m.Name, "Deluxe")
                    .With(m => m.Price, 3M)
                    .With(m => m.PriceCultureInfoName, "en-US")
                    .With(m => m.CanRenew, canReview)
                    .With(m => m.CanUpgrade, canUpgrade)
                    .Create(),
                Fixture.Build<MembershipModel>()
                    .With(m => m.Name, "Premium")
                    .With(m => m.Price, 5M)
                    .With(m => m.PriceCultureInfoName, "en-US")
                    .With(m => m.CanRenew, false)
                    .With(m => m.CanUpgrade, canUpgrade)
                    .Create()
            };
        }
    }
}