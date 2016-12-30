using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Payments;
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
        /// Tests that the constructor initialize the controller which can handle payments.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentController()
        {
            var paymentController = new PaymentController(_householdDataRepositoryMock);
            Assert.That(paymentController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new PaymentController(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataRepository"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Pay throws an ArgumentNullException when the payable model is null.
        /// </summary>
        [Test]
        public void TestThatPayThrowsArgumentNullExceptionWhenPayableModelIsNull()
        {
            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController();
            Assert.That(paymentController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentController.Pay((IPayable) null, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("payableModel"));
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
            IPayable payableModel = MockRepository.GenerateMock<IPayable>();
            Assert.That(payableModel, Is.Not.Null);

            PaymentController paymentController = CreatePaymentController();
            Assert.That(paymentController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentController.Pay(payableModel, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("returnUrl"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Creates a controller which can handle payments for unit testing.
        /// </summary>
        /// <returns>Controller which can handle payments for unit testing.</returns>
        private PaymentController CreatePaymentController()
        {
            var paymentController = new PaymentController(_householdDataRepositoryMock);
            paymentController.ControllerContext = ControllerTestHelper.CreateControllerContext(paymentController);
            return paymentController;
        }
    }
}
