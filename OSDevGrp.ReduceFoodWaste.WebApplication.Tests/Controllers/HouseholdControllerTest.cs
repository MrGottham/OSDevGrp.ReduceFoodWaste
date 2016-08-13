using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using Ploeh.AutoFixture;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Controllers
{
    /// <summary>
    /// Tests the controller for a household.
    /// </summary>
    [TestFixture]
    public class HouseholdControllerTest : TestBase
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
        /// Tests that the constructor initialize the controller for a household.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdController()
        {
            var householdController = new HouseholdController(_householdDataRepositoryMock);
            Assert.That(householdController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdController(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Manage with identification for a given household returns a ViewResult with a model for manage the given household.
        /// </summary>
        [Test]
        public void TestThatManageWithHouseholdIdentifierReturnsViewResultWithModelForManageHousehold()
        {
            var householdController = CreateHouseholdController();
            Assert.That(householdController, Is.Not.Null);

            var householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.EqualTo(default(Guid)));

            var result = householdController.Manage(householdIdentifier);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            var viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Manage"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.TypeOf<HouseholdModel>());
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Empty);

            var householdModel = (HouseholdModel) viewResult.Model;
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier));
        }

        /// <summary>
        /// Tests that HouseholdInformation with identification for a given household calls GetHouseholdAsync on the repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatHouseholdInformationWithHouseholdIdentifierCallsGetHouseholdAsyncOnHouseholdDataRepository()
        {
            var householdIdentifier = Guid.NewGuid();
            Assert.That(householdIdentifier, Is.Not.EqualTo(default(Guid)));

            Action<object[]> householdGetterCallback = arguments =>
            {
                Assert.That(arguments, Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.Not.Null);
                Assert.That(arguments.ElementAt(1), Is.TypeOf<HouseholdModel>());

                var householdModel = (HouseholdModel) arguments.ElementAt(1);
                Assert.That(householdModel, Is.Not.Null);
                Assert.That(householdModel.Identifier, Is.EqualTo(householdIdentifier));
            };

            var householdController = CreateHouseholdController(householdGetterCallback: householdGetterCallback);
            Assert.That(householdController, Is.Not.Null);
            Assert.That(householdController.User, Is.Not.Null);
            Assert.That(householdController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            householdController.HouseholdInformation(householdIdentifier);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Equal(householdController.User.Identity), Arg<HouseholdModel>.Is.NotNull, Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that HouseholdInformation with identification for a given household returns a PartialViewResult with a model for the given household.
        /// </summary>
        [Test]
        public void TestThatHouseholdInformationWithHouseholdIdentifierReturnsPartialViewResultWithHouseholdModel()
        {
            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            var householdController = CreateHouseholdController(household: householdModel);
            Assert.That(householdController, Is.Not.Null);

            var result = householdController.HouseholdInformation(Guid.NewGuid());
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<PartialViewResult>());

            var partialViewResult = (PartialViewResult) result;
            Assert.That(partialViewResult, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Null);
            Assert.That(partialViewResult.ViewName, Is.Not.Empty);
            Assert.That(partialViewResult.ViewName, Is.EqualTo("_HouseholdInformation"));
            Assert.That(partialViewResult.ViewData, Is.Not.Null);
            Assert.That(partialViewResult.ViewData, Is.Empty);
            Assert.That(partialViewResult.Model, Is.Not.Null);
            Assert.That(partialViewResult.Model, Is.EqualTo(householdModel));
        }

        /// <summary>
        /// Creates a controller for a household for unit testing.
        /// </summary>
        /// <param name="household">Sets the model for the household which should be used.</param>
        /// <param name="householdGetterCallback">Sets a callback action which are executed when we get the household to use.</param>
        /// <returns>Controller for a household for unit testing.</returns>
        private HouseholdController CreateHouseholdController(HouseholdModel household = null, Action<object[]> householdGetterCallback = null)
        {
            Func<HouseholdModel> householdGetter = () =>
            {
                if (household != null)
                {
                    return household;
                }
                return Fixture.Build<HouseholdModel>()
                    .With(m => m.HouseholdMembers, null)
                    .Create();
            };

            _householdDataRepositoryMock.Stub(m => m.GetHouseholdAsync(Arg<IIdentity>.Is.Anything, Arg<HouseholdModel>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .WhenCalled(e =>
                {
                    if (householdGetterCallback == null)
                    {
                        return;
                    }
                    try
                    {
                        householdGetterCallback.Invoke(e.Arguments);
                    }
                    catch (TargetInvocationException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            throw ex.InnerException;
                        }
                        throw;
                    }
                })
                .Return(Task.Run(householdGetter))
                .Repeat.Any();

            var householdController = new HouseholdController(_householdDataRepositoryMock);
            householdController.ControllerContext = ControllerTestHelper.CreateControllerContext(householdController);
            return householdController;
        }
    }
}
