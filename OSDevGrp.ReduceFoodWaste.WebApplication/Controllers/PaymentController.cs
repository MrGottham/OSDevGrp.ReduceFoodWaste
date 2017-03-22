using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models.Enums;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    /// <summary>
    /// Controller which can handle payments.
    /// </summary>
    [Authorize]
    [IsValidatedHouseholdMember]
    public class PaymentController : Controller
    {
        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;
        private readonly IModelHelper _modelHelper;
        private readonly IUtilities _utilities;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller which can handle payments.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        /// <param name="modelHelper">Implementation of a model helper.</param>
        /// <param name="utilities">Implementation of the utilities which support the infrastructure.</param>
        public PaymentController(IHouseholdDataRepository householdDataRepository, IModelHelper modelHelper, IUtilities utilities)
        {
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException(nameof(householdDataRepository));
            }
            if (modelHelper == null)
            {
                throw new ArgumentNullException(nameof(modelHelper));
            }
            if (utilities == null)
            {
                throw new ArgumentNullException(nameof(utilities));
            }
            _householdDataRepository = householdDataRepository;
            _modelHelper = modelHelper;
            _utilities = utilities;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute payment on a given payable model.
        /// </summary>
        /// <param name="payableModelAsBase64">Base64 encoded value for the payable model on which to execute the payment.</param>
        /// <param name="returnUrl">Url on which to return to when the payment process has finished.</param>
        /// <returns>View on which to pay for the given payable model.</returns>
        public ActionResult Pay(string payableModelAsBase64, string returnUrl)
        {
            if (string.IsNullOrEmpty(payableModelAsBase64))
            {
                throw new ArgumentNullException(nameof(payableModelAsBase64));
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }

            try
            {
                PayableModel payableModel = (PayableModel) _modelHelper.ToModel(payableModelAsBase64);
                if (payableModel.IsFreeOfCost)
                {
                    return Redirect(returnUrl);
                }

                Task<IEnumerable<PaymentHandlerModel>> task = _householdDataRepository.GetPaymentHandlersAsync(User.Identity, Thread.CurrentThread.CurrentUICulture);
                task.Wait();

                payableModel.PaymentHandlerIdentifier = null;
                payableModel.PaymentHandlers = task.Result;
                payableModel.PaymentStatus = PaymentStatus.Unpaid;
                payableModel.PaymentReceipt = null;

                ViewBag.ReturnUrl = returnUrl;
                ViewBag.BillingInformationWithoutHTMLTags = _utilities.StripHtml(payableModel.BillingInformation);

                return View("Pay", payableModel);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        /// <summary>
        /// Executes the payment process when the payable model should be paid by Paypal.
        /// </summary>
        /// <param name="payableModel">Payable model which should be paid by Paypal.</param>
        /// <param name="returnUrl">Url on which to return to when the payment process has finished.</param>
        /// <returns>Action the execute when the payable model should be paid by Paypal.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Action PayWithPaypal(PayableModel payableModel, string returnUrl)
        {
            if (payableModel == null)
            {
                throw new ArgumentNullException(nameof(payableModel));
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }
            payableModel.PaymentStatus = PaymentStatus.Paid;

            string payment = _modelHelper.ToBase64(payableModel);

            return null;
        }

        #endregion
    }
}