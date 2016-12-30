using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller which can handle payments.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        public PaymentController(IHouseholdDataRepository householdDataRepository)
        {
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException(nameof(householdDataRepository));
            }
            _householdDataRepository = householdDataRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute payment on a given payable model.
        /// </summary>
        /// <param name="payableModel">Payable model on which to execute the payment.</param>
        /// <param name="returnUrl">Url on which to return to when the payment process has finished.</param>
        /// <returns>View on which to pay for the given payable model.</returns>
        public ActionResult Pay(PayableModel payableModel, string returnUrl)
        {
            if (payableModel == null)
            {
                throw new ArgumentNullException(nameof(payableModel));
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }

            throw new NotImplementedException();
        }

        #endregion
    }
}