using System;
using System.Globalization;
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
    /// Tests the controller which can handle payments.
    /// </summary>
    [TestFixture]
    public class PaymentControllerTests : TestBase
    {
        #region Private variables

        private IHouseholdDataRepository _householdDataRepositoryMock;
        private IModelHelper _modelHelperMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            _modelHelperMock = MockRepository.GenerateMock<IModelHelper>();
        }

        /// <summary>
        /// Tests that the constructor initialize the controller which can handle payments.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentController()
        {
            var paymentController = new PaymentController(_householdDataRepositoryMock, _modelHelperMock);
            Assert.That(paymentController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new PaymentController(null, _modelHelperMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the model helper is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenModelHelperIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new PaymentController(_householdDataRepositoryMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("modelHelper"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Pay throws an ArgumentNullException when the Base64 encoded value for the payable model is null or empty.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatPayThrowsArgumentNullExceptionWhenPayableModelAsBase64IsNullOrEmpty(string payableModelAsBase64)
        {
            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController();
            Assert.That(paymentController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentController.Pay(payableModelAsBase64, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("payableModelAsBase64"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Pay throws an ArgumentNullException when the url on which to return to when the payment process has finished is null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatPayThrowsArgumentNullExceptionWhenPayableReturnUrlIsNullOrEmpty(string returnUrl)
        {
            string payableModelAsBase64 = Fixture.Create<string>();
            Assert.That(payableModelAsBase64, Is.Not.Null);
            Assert.That(payableModelAsBase64, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController();
            Assert.That(paymentController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentController.Pay(payableModelAsBase64, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("returnUrl"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Pay calls ToModel with the base64 encoded value for the payable model on which to execute the payment on the model helper.
        /// </summary>
        [Test]
        public void TestThatPayCallsToModelWithPayableModelAsBase64OnModelHelper()
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .Create();
            Assert.That(payableModel, Is.Not.Null);

            string payableModelAsBase64 = Fixture.Create<string>();
            Assert.That(payableModelAsBase64, Is.Not.Null);
            Assert.That(payableModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController(toModel: payableModel);
            Assert.That(paymentController, Is.Not.Null);

            paymentController.Pay(payableModelAsBase64, returnUrl);
            
            _modelHelperMock.AssertWasCalled(m => m.ToModel(Arg<string>.Is.Equal(payableModelAsBase64)));
        }

        /// <summary>
        /// Creates a controller which can handle payments for unit testing.
        /// </summary>
        /// <param name="toModel">Sets the model which should be returned for a given base64 encoded model.</param>
        /// <returns>Controller which can handle payments for unit testing.</returns>
        private PaymentController CreatePaymentController(object toModel = null)
        {
            _modelHelperMock.Stub(m => m.ToModel(Arg<string>.Is.Anything))
                .Return(toModel)
                .Repeat.Any();

            var paymentController = new PaymentController(_householdDataRepositoryMock, _modelHelperMock);
            paymentController.ControllerContext = ControllerTestHelper.CreateControllerContext(paymentController);
            return paymentController;
        }
    }
}
