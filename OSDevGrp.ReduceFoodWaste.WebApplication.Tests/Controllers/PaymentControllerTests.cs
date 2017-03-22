using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Controllers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models.Enums;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;
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
        private IUtilities _utilitiesMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _householdDataRepositoryMock = MockRepository.GenerateMock<IHouseholdDataRepository>();
            _modelHelperMock = MockRepository.GenerateMock<IModelHelper>();
            _utilitiesMock = MockRepository.GenerateMock<IUtilities>();
        }

        /// <summary>
        /// Tests that the constructor initialize the controller which can handle payments.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializePaymentController()
        {
            var paymentController = new PaymentController(_householdDataRepositoryMock, _modelHelperMock, _utilitiesMock);
            Assert.That(paymentController, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the repository which can access household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new PaymentController(null, _modelHelperMock, _utilitiesMock));
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
            var exception = Assert.Throws<ArgumentNullException>(() => new PaymentController(_householdDataRepositoryMock, null, _utilitiesMock));
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
            var exception = Assert.Throws<ArgumentNullException>(() => new PaymentController(_householdDataRepositoryMock, _modelHelperMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("utilities"));
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
        public void TestThatPayThrowsArgumentNullExceptionWhenReturnUrlIsNullOrEmpty(string returnUrl)
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
                .With(m => m.PaymentHandlerIdentifier, null)
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentReceipt, null)
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.Price, Is.GreaterThan(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.False);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Null);

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
        /// Tests that Pay does not call GetPaymentHandlersAsync on the repository which can access household data when the payable model is free of cost.
        /// </summary>
        [Test]
        public void TestThatPayDoesNotCallGetPaymentHandlersAsyncOnHouseholdDataRepositoryWhenPayableModelIsFreeOfCost()
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, 0M)
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, null)
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentReceipt, null)
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.Price, Is.EqualTo(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.True);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Null);

            string payableModelAsBase64 = Fixture.Create<string>();
            Assert.That(payableModelAsBase64, Is.Not.Null);
            Assert.That(payableModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController(toModel: payableModel);
            Assert.That(paymentController, Is.Not.Null);

            paymentController.Pay(payableModelAsBase64, returnUrl);

            _householdDataRepositoryMock.AssertWasNotCalled(m => m.GetPaymentHandlersAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything));
        }

        /// <summary>
        /// Tests that Pay does not call StripHtml on the utilities which support the infrastructure when the payable model is free of cost.
        /// </summary>
        [Test]
        public void TestThatPayDoesNotCallStripHtmlOnUtilitiesWhenPayableModelIsFreeOfCost()
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, 0M)
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, null)
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentReceipt, null)
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.Price, Is.EqualTo(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.True);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Null);

            string payableModelAsBase64 = Fixture.Create<string>();
            Assert.That(payableModelAsBase64, Is.Not.Null);
            Assert.That(payableModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController(toModel: payableModel);
            Assert.That(paymentController, Is.Not.Null);

            paymentController.Pay(payableModelAsBase64, returnUrl);

            _utilitiesMock.AssertWasNotCalled(m => m.StripHtml(Arg<string>.Is.Anything));
        }

        /// <summary>
        /// Tests that Pay returns a RedirectResult to the url on which to return to when the payment process has finished when the payable model is free of cost.
        /// </summary>
        [Test]
        public void TestThatPayReturnsRedirectResultToReturnUrlWhenPayableModelIsFreeOfCost()
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, 0M)
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, null)
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentReceipt, null)
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.Price, Is.EqualTo(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.True);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Null);

            string payableModelAsBase64 = Fixture.Create<string>();
            Assert.That(payableModelAsBase64, Is.Not.Null);
            Assert.That(payableModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController(toModel: payableModel);
            Assert.That(paymentController, Is.Not.Null);

            var result = paymentController.Pay(payableModelAsBase64, returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectResult>());

            var redirectResult = (RedirectResult) result;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Null);
            Assert.That(redirectResult.Url, Is.Not.Empty);
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
        }

        /// <summary>
        /// Tests that Pay calls GetPaymentHandlersAsync on the repository which can access household data when the payable model is not free of cost.
        /// </summary>
        [Test]
        public void TestThatPayCallsGetPaymentHandlersAsyncOnHouseholdDataRepositoryWhenPayableModelIsNotFreeOfCost()
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, null)
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentReceipt, null)
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.Price, Is.GreaterThan(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.False);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Null);

            string payableModelAsBase64 = Fixture.Create<string>();
            Assert.That(payableModelAsBase64, Is.Not.Null);
            Assert.That(payableModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController(toModel: payableModel);
            Assert.That(paymentController, Is.Not.Null);
            Assert.That(paymentController.User, Is.Not.Null);
            Assert.That(paymentController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            paymentController.Pay(payableModelAsBase64, returnUrl);

            _householdDataRepositoryMock.AssertWasCalled(m => m.GetPaymentHandlersAsync(Arg<IIdentity>.Is.Equal(paymentController.User.Identity), Arg<CultureInfo>.Is.Equal(Thread.CurrentThread.CurrentUICulture)));
        }

        /// <summary>
        /// Tests that Pay calls StripHtml on the utilities which support the infrastructure when the payable model is not free of cost.
        /// </summary>
        [Test]
        public void TestThatPayCallsStripHtmlOnUtilitiesWhenPayableModelIsNotFreeOfCost()
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.BillingInformation, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, null)
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentReceipt, null)
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Empty);
            Assert.That(payableModel.Price, Is.GreaterThan(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.False);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Null);

            string payableModelAsBase64 = Fixture.Create<string>();
            Assert.That(payableModelAsBase64, Is.Not.Null);
            Assert.That(payableModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController(toModel: payableModel);
            Assert.That(paymentController, Is.Not.Null);
            Assert.That(paymentController.User, Is.Not.Null);
            Assert.That(paymentController.User.Identity, Is.Not.Null);

            Assert.That(Thread.CurrentThread, Is.Not.Null);
            Assert.That(Thread.CurrentThread.CurrentUICulture, Is.Not.Null);

            paymentController.Pay(payableModelAsBase64, returnUrl);

            _utilitiesMock.AssertWasCalled(m => m.StripHtml(Arg<string>.Is.Equal(payableModel.BillingInformation)));
        }

        /// <summary>
        /// Tests that Pay returns a ViewResult on which to pay for the payable model when the payable model is not free of cost.
        /// </summary>
        [Test]
        public void TestThatPayReturnsViewResultForPaymentWhenPayableModelIsNotFreeOfCost()
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Paid)
                .With(m => m.PaymentReceipt, Fixture.Create<string>())
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.Price, Is.GreaterThan(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.False);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.True);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentHandlers, Is.Not.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Not.Null);
            Assert.That(payableModel.PaymentReceipt, Is.Not.Empty);

            string payableModelAsBase64 = Fixture.Create<string>();
            Assert.That(payableModelAsBase64, Is.Not.Null);
            Assert.That(payableModelAsBase64, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            IEnumerable<PaymentHandlerModel> paymentHandlerModelCollection = Fixture.CreateMany<PaymentHandlerModel>(Random.Next(5, 10)).ToList();
            Assert.That(paymentHandlerModelCollection, Is.Not.Null);
            Assert.That(paymentHandlerModelCollection, Is.Not.Empty);

            string billingInformationWithoutHtmlTags = Fixture.Create<string>();
            Assert.That(billingInformationWithoutHtmlTags, Is.Not.Null);
            Assert.That(billingInformationWithoutHtmlTags, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController(paymentHandlerModelCollection: paymentHandlerModelCollection, toModel: payableModel, billingInformationWithoutHtmlTags: billingInformationWithoutHtmlTags);
            Assert.That(paymentController, Is.Not.Null);

            ActionResult result = paymentController.Pay(payableModelAsBase64, returnUrl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ViewResult>());

            ViewResult viewResult = (ViewResult) result;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Null);
            Assert.That(viewResult.ViewName, Is.Not.Empty);
            Assert.That(viewResult.ViewName, Is.EqualTo("Pay"));
            Assert.That(viewResult.Model, Is.Not.Null);
            Assert.That(viewResult.Model, Is.EqualTo(payableModel));
            Assert.That(viewResult.ViewData, Is.Not.Null);
            Assert.That(viewResult.ViewData, Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Null);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["ReturnUrl"], Is.EqualTo(returnUrl));
            Assert.That(viewResult.ViewData["BillingInformationWithoutHTMLTags"], Is.Not.Null);
            Assert.That(viewResult.ViewData["BillingInformationWithoutHTMLTags"], Is.Not.Empty);
            Assert.That(viewResult.ViewData["BillingInformationWithoutHTMLTags"], Is.EqualTo(billingInformationWithoutHtmlTags));

            PayableModel model = (PayableModel) viewResult.Model;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.PaymentHandlerIdentifier, Is.Null);
            Assert.That(model.PaymentHandlerIdentifier.HasValue, Is.False);
            Assert.That(model.PaymentHandlers, Is.Not.Null);
            Assert.That(model.PaymentHandlers, Is.Not.Empty);
            Assert.That(model.PaymentHandlers, Is.EqualTo(paymentHandlerModelCollection));
            Assert.That(model.PaymentStatus, Is.EqualTo(PaymentStatus.Unpaid));
            Assert.That(model.PaymentReceipt, Is.Null);
        }

        /// <summary>
        /// Tests that PayWithPaypal throws an ArgumentNullException when the payable model which should be paid by Paypal is null.
        /// </summary>
        [Test]
        public void TestThatPayWithPaypalThrowsArgumentNullExceptionWhenPayableModelIsNull()
        {
            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController();
            Assert.That(paymentController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentController.PayWithPaypal(null, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("payableModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that PayWithPaypal throws an ArgumentNullException when the url on which to return to when the payment process has finished is null.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatPayWithPaypalThrowsArgumentNullExceptionWhenReturnUrlIsNullOrEmpty(string returnUrl)
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.BillingInformation, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentReceipt, null)
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Empty);
            Assert.That(payableModel.Price, Is.GreaterThan(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.False);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.True);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentHandlers, Is.Not.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Null);
            Assert.That(payableModel.PaymentReceipt, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController();
            Assert.That(paymentController, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => paymentController.PayWithPaypal(payableModel, returnUrl));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("returnUrl"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that PayWithPaypal calls ToBase64 on the model helper with the paid model.
        /// </summary>
        [Test]
        public void TestThatPayWithPaypalCallsToBase64OnModelHelperWithPaidModel()
        {
            PayableModel payableModel = Fixture.Build<PayableModel>()
                .With(m => m.BillingInformation, Fixture.Create<string>())
                .With(m => m.Price, Math.Abs(Fixture.Create<decimal>()))
                .With(m => m.PriceCultureInfoName, CultureInfo.CurrentUICulture.Name)
                .With(m => m.PaymentHandlerIdentifier, Guid.NewGuid())
                .With(m => m.PaymentHandlers, null)
                .With(m => m.PaymentStatus, PaymentStatus.Unpaid)
                .With(m => m.PaymentReceipt, null)
                .Create();
            Assert.That(payableModel, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Null);
            Assert.That(payableModel.BillingInformation, Is.Not.Empty);
            Assert.That(payableModel.Price, Is.GreaterThan(0M));
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(payableModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(payableModel.PriceCultureInfoName, Is.EqualTo(CultureInfo.CurrentUICulture.Name));
            Assert.That(payableModel.IsFreeOfCost, Is.False);
            Assert.That(payableModel.PaymentHandlerIdentifier, Is.Not.Null);
            Assert.That(payableModel.PaymentHandlerIdentifier.HasValue, Is.True);
            Assert.That(payableModel.PaymentHandlers, Is.Null);
            Assert.That(payableModel.PaymentHandlers, Is.Not.EqualTo(PaymentStatus.Unpaid));
            Assert.That(payableModel.PaymentReceipt, Is.Null);
            Assert.That(payableModel.PaymentReceipt, Is.Not.Empty);

            string returnUrl = Fixture.Create<string>();
            Assert.That(returnUrl, Is.Not.Null);
            Assert.That(returnUrl, Is.Not.Empty);

            string payAtText = string.Format(Texts.PaidAt, DateTime.Today.ToLongDateString());
            string paymentReceiptText = $"{payableModel.BillingInformation}{Environment.NewLine}{payAtText}";
            string expectedPaymentReceipt;
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(paymentReceiptText)))
            {
                expectedPaymentReceipt = Convert.ToBase64String(memoryStream.ToArray());
                memoryStream.Close();
            }
            Assert.That(expectedPaymentReceipt, Is.Not.Null);
            Assert.That(expectedPaymentReceipt, Is.Not.Empty);

            PaymentController paymentController = CreatePaymentController();
            Assert.That(paymentController, Is.Not.Null);

            paymentController.PayWithPaypal(payableModel, returnUrl);

            _modelHelperMock.AssertWasCalled(m => m.ToBase64(Arg<PayableModel>.Matches(model =>
                model != null &&
                model == payableModel &&
                model.PaymentStatus == PaymentStatus.Paid &&
                model.PaymentReceipt != null &&
                model.PaymentReceipt != string.Empty &&
                string.Compare(model.PaymentReceipt, expectedPaymentReceipt, StringComparison.Ordinal) == 0)));
        }

        /// <summary>
        /// Creates a controller which can handle payments for unit testing.
        /// </summary>
        /// <param name="paymentHandlerModelCollection">Sets the collection of models for the data providers who handles payments.</param>
        /// <param name="toModel">Sets the model which should be returned for a given base64 encoded model.</param>
        /// <param name="billingInformationWithoutHtmlTags">Sets the billing information where all the HTML tags has been removed.</param>
        /// <param name="toBase64">Sets the base64 encoded value which sould be returned for a given model.</param>
        /// <returns>Controller which can handle payments for unit testing.</returns>
        private PaymentController CreatePaymentController(IEnumerable<PaymentHandlerModel> paymentHandlerModelCollection = null, object toModel = null, string billingInformationWithoutHtmlTags = null, string toBase64 = null)
        {
            _householdDataRepositoryMock.Stub(m => m.GetPaymentHandlersAsync(Arg<IIdentity>.Is.Anything, Arg<CultureInfo>.Is.Anything))
                .Return(Task.Run(() => paymentHandlerModelCollection ?? Fixture.CreateMany<PaymentHandlerModel>(Random.Next(5, 10)).ToList()))
                .Repeat.Any();

            _modelHelperMock.Stub(m => m.ToModel(Arg<string>.Is.Anything))
                .Return(toModel)
                .Repeat.Any();
            _modelHelperMock.Stub(m => m.ToBase64(Arg<object>.Is.Anything))
                .Return(toBase64 ?? Fixture.Create<string>())
                .Repeat.Any();

            _utilitiesMock.Stub(m => m.StripHtml(Arg<string>.Is.Anything))
                .Return(billingInformationWithoutHtmlTags ?? Fixture.Create<string>())
                .Repeat.Any();

            var paymentController = new PaymentController(_householdDataRepositoryMock, _modelHelperMock, _utilitiesMock);
            paymentController.ControllerContext = ControllerTestHelper.CreateControllerContext(paymentController);
            return paymentController;
        }
    }
}
