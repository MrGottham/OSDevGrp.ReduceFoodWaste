using System;
using System.Threading;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    /// <summary>
    /// Controller for a household member.
    /// </summary>
    [Authorize]
    [IsAuthenticated]
    public class HouseholdMemberController : Controller
    {
        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;
        private readonly IClaimValueProvider _claimValueProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller for a household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        /// <param name="claimValueProvider">Implementaton of a provider which can get values from claims.</param>
        public HouseholdMemberController(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider)
        {
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException("householdDataRepository");
            }
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException("claimValueProvider");
            }
            _householdDataRepository = householdDataRepository;
            _claimValueProvider = claimValueProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new household member.
        /// </summary>
        /// <returns>View for creating a new household member.</returns>
        public ActionResult Create()
        {
            try
            {
                var task = _householdDataRepository.GetPrivacyPoliciesAsync(User.Identity, Thread.CurrentThread.CurrentUICulture);
                task.Wait();

                var privacyPolicyModel = task.Result;
                privacyPolicyModel.IsAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);

                var householdModel = new HouseholdModel
                {
                    PrivacyPolicy = privacyPolicyModel
                };

                return View("Create", householdModel);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        /// <summary>
        /// Creates a new household member.
        /// </summary>
        /// <param name="householdModel">Model for the household members first household.</param>
        /// <returns>View for the next step.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HouseholdModel householdModel)
        {
            if (householdModel == null)
            {
                throw new ArgumentNullException("householdModel");
            }
            try
            {
                var privacyPolicyGetTask = _householdDataRepository.GetPrivacyPoliciesAsync(User.Identity, Thread.CurrentThread.CurrentUICulture);
                privacyPolicyGetTask.Wait();

                var reloadedPrivacyPolicyModel = privacyPolicyGetTask.Result;
                if (householdModel.PrivacyPolicy == null)
                {
                    householdModel.PrivacyPolicy = reloadedPrivacyPolicyModel;
                }
                else
                {
                    householdModel.PrivacyPolicy.Identifier = reloadedPrivacyPolicyModel.Identifier;
                    householdModel.PrivacyPolicy.Header = reloadedPrivacyPolicyModel.Header;
                    householdModel.PrivacyPolicy.Body = reloadedPrivacyPolicyModel.Body;
                }

                if (ModelState.IsValid == false)
                {
                    return View(householdModel);
                }

                return null;
            }
            catch (AggregateException ex)
            {
                ViewBag.StatusMessage = ex.ToReduceFoodWasteException().Message;
                return View(householdModel);
            }
            catch (Exception ex)
            {
                ViewBag.StatusMessage = ex.Message;
                return View(householdModel);
            }
        }

        /// <summary>
        /// Prepares a household member.
        /// </summary>
        /// <returns>View for preparing a household member.</returns>
        [IsCreatedHouseholdMember]
        public ActionResult Prepare()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}