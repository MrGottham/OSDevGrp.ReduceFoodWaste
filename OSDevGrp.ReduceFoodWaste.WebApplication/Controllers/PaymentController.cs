using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller which can handle payments.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        /// <param name="modelHelper">Implementation of a model helper.</param>
        public PaymentController(IHouseholdDataRepository householdDataRepository, IModelHelper modelHelper)
        {
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException(nameof(householdDataRepository));
            }
            if (modelHelper == null)
            {
                throw new ArgumentNullException(nameof(modelHelper));
            }
            _householdDataRepository = householdDataRepository;
            _modelHelper = modelHelper;
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

                return null;
                //throw new NotImplementedException();
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        #endregion
    }
}