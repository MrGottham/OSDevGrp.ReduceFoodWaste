using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Utilities;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models.Enums;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

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
        private readonly ILocalClaimProvider _localClaimProvider;
        private readonly IModelHelper _modelHelper;
        private readonly IUtilities _utilities;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller for a household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can get values from claims.</param>
        /// <param name="localClaimProvider">Implementation of a provider which can append local claims to a claims identity.</param>
        /// <param name="modelHelper">Implementation of a model helper.</param>
        /// <param name="utilities">Implementation of the utilities which support the infrastructure.</param>
        public HouseholdMemberController(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, ILocalClaimProvider localClaimProvider, IModelHelper modelHelper, IUtilities utilities)
        {
            _householdDataRepository = householdDataRepository ?? throw new ArgumentNullException(nameof(householdDataRepository));
            _claimValueProvider = claimValueProvider ?? throw new ArgumentNullException(nameof(claimValueProvider));
            _localClaimProvider = localClaimProvider ?? throw new ArgumentNullException(nameof(localClaimProvider));
            _modelHelper = modelHelper ?? throw new ArgumentNullException(nameof(modelHelper));
            _utilities = utilities ?? throw new ArgumentNullException(nameof(utilities));
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
                PrivacyPolicyModel privacyPolicyModel = _householdDataRepository
                    .GetPrivacyPoliciesAsync(User.Identity, Thread.CurrentThread.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult();

                bool isPrivacyPoliciesAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);

                privacyPolicyModel.IsAccepted = isPrivacyPoliciesAccepted;
                privacyPolicyModel.AcceptedTime = isPrivacyPoliciesAccepted ? DateTime.Now : (DateTime?) null;

                HouseholdModel householdModel = new HouseholdModel
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
                throw new ArgumentNullException(nameof(householdModel));
            }
            try
            {
                PrivacyPolicyModel reloadedPrivacyPolicyModel = _householdDataRepository.GetPrivacyPoliciesAsync(User.Identity, Thread.CurrentThread.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult();

                if (householdModel.PrivacyPolicy == null)
                {
                    householdModel.PrivacyPolicy = reloadedPrivacyPolicyModel;
                    householdModel.PrivacyPolicy.IsAccepted = false;
                    householdModel.PrivacyPolicy.AcceptedTime = null;
                }
                else
                {
                    householdModel.PrivacyPolicy.Identifier = reloadedPrivacyPolicyModel.Identifier;
                    householdModel.PrivacyPolicy.Header = reloadedPrivacyPolicyModel.Header;
                    householdModel.PrivacyPolicy.Body = reloadedPrivacyPolicyModel.Body;
                }

                if (ModelState.IsValid == false)
                {
                    householdModel.PrivacyPolicy.IsAccepted = false;
                    householdModel.PrivacyPolicy.AcceptedTime = null;
                    return View("Create", householdModel);
                }

                _householdDataRepository.CreateHouseholdAsync(User.Identity, householdModel, CultureInfo.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult();

                AddClaim(_localClaimProvider.GenerateCreatedHouseholdMemberClaim());

                if (householdModel.PrivacyPolicy == null || householdModel.PrivacyPolicy.IsAccepted == false)
                {
                    return RedirectToAction("Prepare");
                }

                try
                {
                    _householdDataRepository.AcceptPrivacyPolicyAsync(User.Identity, householdModel.PrivacyPolicy)
                        .GetAwaiter()
                        .GetResult();

                    AddClaim(_localClaimProvider.GeneratePrivacyPoliciesAcceptedClaim());

                    return RedirectToAction("Prepare");
                }
                catch (AggregateException ex)
                {
                    return RedirectToAction("Prepare", new {errorMessage = ex.ToReduceFoodWasteException().Message});
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Prepare", new {errorMessage = ex.Message});
                }
            }
            catch (AggregateException ex)
            {
                ViewBag.ErrorMessage = ex.ToReduceFoodWasteException().Message;
                return View("Create", householdModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Create", householdModel);
            }
        }

        /// <summary>
        /// Prepares a household member.
        /// </summary>
        /// <returns>View for preparing a household member.</returns>
        [IsCreatedHouseholdMember]
        public ActionResult Prepare(string errorMessage = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(errorMessage) == false)
                {
                    ViewBag.ErrorMessage = errorMessage;
                }

                bool isActivatedHouseholdMember = _claimValueProvider.IsActivatedHouseholdMember(User.Identity);
                bool isPrivacyPoliciesAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);

                PrivacyPolicyModel privacyPolicyModel = _householdDataRepository.GetPrivacyPoliciesAsync(User.Identity, CultureInfo.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult();

                privacyPolicyModel.IsAccepted = isPrivacyPoliciesAccepted;
                privacyPolicyModel.AcceptedTime = isPrivacyPoliciesAccepted ? DateTime.Now : (DateTime?) null;

                HouseholdMemberModel householdMemberModel = new HouseholdMemberModel
                {
                    ActivatedTime = isActivatedHouseholdMember ? DateTime.Now : (DateTime?) null,
                    PrivacyPolicy = privacyPolicyModel,
                    PrivacyPolicyAcceptedTime = isPrivacyPoliciesAccepted ? DateTime.Now : (DateTime?) null
                };

                return View("Prepare", householdMemberModel);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        /// <summary>
        /// Prepares a household member
        /// </summary>
        /// <param name="householdMemberModel">Model for the household member who to prepare.</param>
        /// <returns>View for the next step.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [IsCreatedHouseholdMember]
        public ActionResult Prepare(HouseholdMemberModel householdMemberModel)
        {
            if (householdMemberModel == null)
            {
                throw new ArgumentNullException(nameof(householdMemberModel));
            }
            try
            {
                PrivacyPolicyModel reloadedPrivacyPolicyModel = _householdDataRepository.GetPrivacyPoliciesAsync(User.Identity, Thread.CurrentThread.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult();

                if (householdMemberModel.PrivacyPolicy == null)
                {
                    bool privacyPoliciesHasAlreadyBeenAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);
                    householdMemberModel.PrivacyPolicy = reloadedPrivacyPolicyModel;
                    householdMemberModel.PrivacyPolicy.IsAccepted = privacyPoliciesHasAlreadyBeenAccepted;
                    householdMemberModel.PrivacyPolicy.AcceptedTime = privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now : (DateTime?) null;
                    householdMemberModel.PrivacyPolicyAcceptedTime = privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now : (DateTime?) null;
                }
                else
                {
                    householdMemberModel.PrivacyPolicy.Identifier = reloadedPrivacyPolicyModel.Identifier;
                    householdMemberModel.PrivacyPolicy.Header = reloadedPrivacyPolicyModel.Header;
                    householdMemberModel.PrivacyPolicy.Body = reloadedPrivacyPolicyModel.Body;
                }

                if (ModelState.IsValid == false)
                {
                    bool privacyPoliciesHasAlreadyBeenAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);
                    householdMemberModel.PrivacyPolicy.IsAccepted = privacyPoliciesHasAlreadyBeenAccepted;
                    householdMemberModel.PrivacyPolicy.AcceptedTime = privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now : (DateTime?) null;
                    householdMemberModel.PrivacyPolicyAcceptedTime = privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now : (DateTime?) null;
                    return View("Prepare", householdMemberModel);
                }

                if (householdMemberModel.IsActivated == false && string.IsNullOrWhiteSpace(householdMemberModel.ActivationCode) == false)
                {
                    HouseholdMemberModel activatedHouseholdMemberModel = _householdDataRepository.ActivateHouseholdMemberAsync(User.Identity, householdMemberModel)
                        .GetAwaiter()
                        .GetResult();

                    AddClaim(_localClaimProvider.GenerateActivatedHouseholdMemberClaim());

                    householdMemberModel.ActivatedTime = activatedHouseholdMemberModel.ActivatedTime;
                }

                if (householdMemberModel.HasAcceptedPrivacyPolicy == false && householdMemberModel.PrivacyPolicy != null && householdMemberModel.PrivacyPolicy.IsAccepted)
                {
                    PrivacyPolicyModel acceptedPrivacyPolicy = _householdDataRepository.AcceptPrivacyPolicyAsync(User.Identity, householdMemberModel.PrivacyPolicy)
                        .GetAwaiter()
                        .GetResult();

                    AddClaim(_localClaimProvider.GeneratePrivacyPoliciesAcceptedClaim());

                    householdMemberModel.PrivacyPolicyAcceptedTime = acceptedPrivacyPolicy.AcceptedTime;
                }

                if (householdMemberModel.IsActivated == false || householdMemberModel.HasAcceptedPrivacyPolicy == false)
                {
                    return View("Prepare", householdMemberModel);
                }

                AddClaim(_localClaimProvider.GenerateValidatedHouseholdMemberClaim());

                return RedirectToAction("Dashboard", "Dashboard");
            }
            catch (AggregateException ex)
            {
                ViewBag.ErrorMessage = ex.ToReduceFoodWasteException().Message;
                return View("Prepare", householdMemberModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Prepare", householdMemberModel);
            }
        }

        /// <summary>
        /// Manage a household member account.
        /// </summary>
        /// <param name="statusMessage">Status message to show in the view.</param>
        /// <param name="errorMessage">Error message to show in the view.</param>
        /// <returns>View for manage a household member account.</returns>
        [IsValidatedHouseholdMember]
        public ActionResult Manage(string statusMessage = null, string errorMessage = null)
        {
            try
            {
                HouseholdMemberModel householdMemberModel = _householdDataRepository.GetHouseholdMemberAsync(User.Identity, Thread.CurrentThread.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult();

                if (string.IsNullOrWhiteSpace(statusMessage) == false)
                {
                    ViewBag.StatusMessage = statusMessage;
                }
                if (string.IsNullOrWhiteSpace(errorMessage) == false)
                {
                    ViewBag.ErrorMessage = errorMessage;
                }

                return View("Manage", householdMemberModel);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        /// <summary>
        /// Upgrades a household members membership.
        /// </summary>
        /// <param name="returnUrl">Url on which to return when the membership upgrade process has finished.</param>
        /// <param name="statusMessage">Status message to show in the view.</param>
        /// <param name="errorMessage">Error message to show in the view.</param>
        /// <returns>View for upgrading a household members membership.</returns>
        [IsValidatedHouseholdMember]
        public ActionResult UpgradeMembership(string returnUrl, string statusMessage = null, string errorMessage = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }
            try
            {
                IList<MembershipModel> membershipModelCollection = _householdDataRepository.GetMembershipsAsync(User.Identity, Thread.CurrentThread.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult()
                    .ToList();

                if (membershipModelCollection.Any(m => m.CanUpgrade) == false)
                {
                    throw new ReduceFoodWasteSystemException(Texts.MembershipUpgradeNotPossible);
                }

                ViewBag.ReturnUrl = returnUrl;

                if (string.IsNullOrWhiteSpace(statusMessage) == false)
                {
                    ViewBag.StatusMessage = statusMessage;
                }
                if (string.IsNullOrWhiteSpace(errorMessage) == false)
                {
                    ViewBag.ErrorMessage = errorMessage;
                }

                return View("UpgradeMembership", membershipModelCollection);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        /// <summary>
        /// Renews a household members current membership.
        /// </summary>
        /// <param name="returnUrl">Url on which to return when the membership renew process has finished.</param>
        /// <param name="statusMessage">Status message to show in the view.</param>
        /// <param name="errorMessage">Error message to show in the view.</param>
        /// <returns>View for renewing a household members current membership.</returns>
        [IsValidatedHouseholdMember]
        public ActionResult RenewMembership(string returnUrl, string statusMessage = null, string errorMessage = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }
            try
            {
                MembershipModel membershipModel = _householdDataRepository.GetMembershipsAsync(User.Identity, Thread.CurrentThread.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult()
                    .SingleOrDefault(m => m.CanRenew);

                if (membershipModel == null)
                {
                    throw new ReduceFoodWasteSystemException(Texts.MembershipRenewNotPossible);
                }

                ViewBag.ReturnUrl = returnUrl;

                if (string.IsNullOrWhiteSpace(statusMessage) == false)
                {
                    ViewBag.StatusMessage = statusMessage;
                }
                if (string.IsNullOrWhiteSpace(errorMessage) == false)
                {
                    ViewBag.ErrorMessage = errorMessage;
                }

                return View("RenewMembership", membershipModel);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        /// <summary>
        /// Upgrade or renew a household members membership.
        /// </summary>
        /// <param name="membershipModel">Model for the membership which should be upgraded or renewed.</param>
        /// <param name="returnUrl">Url on which to return when the membership upgrade or renew process has finished.</param>
        /// <returns>View for the next step in the process of upgrading or renewing a membership.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [IsValidatedHouseholdMember]
        public ActionResult UpgradeOrRenewMembership(MembershipModel membershipModel, string returnUrl)
        {
            if (membershipModel == null)
            {
                throw new ArgumentNullException(nameof(membershipModel));
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }

            try
            {
                if (membershipModel.IsFreeOfCost || (membershipModel.CanRenew == false && membershipModel.CanUpgrade == false))
                {
                    return Redirect(returnUrl);
                }

                MembershipModel reloadedMembershipModel = _householdDataRepository.GetMembershipsAsync(User.Identity, Thread.CurrentThread.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult()
                    .SingleOrDefault(m => string.Compare(m.Name, membershipModel.Name, StringComparison.Ordinal) == 0);

                if (reloadedMembershipModel == null)
                {
                    return Redirect(returnUrl);
                }

                membershipModel.BillingInformation = reloadedMembershipModel.BillingInformation;
                membershipModel.Description = reloadedMembershipModel.Description;

                string membershipModelAsBase64 = _modelHelper.ToBase64(membershipModel);
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary
                {
                    {"membershipModelAsBase64", membershipModelAsBase64},
                    {"paymentModelAsBase64", "[PaymentModelAsBase64]"},
                    {"returnUrl", returnUrl}
                };
                string callbackUrl = _utilities.ActionToUrl(Url, "UpgradeOrRenewMembershipCallback", "HouseholdMember", routeValueDictionary);

                routeValueDictionary = new RouteValueDictionary
                {
                    {"payableModelAsBase64", membershipModelAsBase64},
                    {"returnUrl", callbackUrl}
                };
                return RedirectToAction("Pay", "Payment", routeValueDictionary);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        /// <summary>
        /// Callback action which finish the upgrade or renew process after the payment.
        /// </summary>
        /// <param name="membershipModelAsBase64">Base64 encoded value for the model of the membership which should be upgraded or renewed.</param>
        /// <param name="paymentModelAsBase64">Base64 encoded value for a payable model which contains information about the payment.</param>
        /// <param name="returnUrl">Url on which to return when the membership upgrade or renew process has finished.</param>
        /// <returns>Redirect to the url on which to return when the membership upgrade or renew process has finished.</returns>
        [IsValidatedHouseholdMember]
        public ActionResult UpgradeOrRenewMembershipCallback(string membershipModelAsBase64, string paymentModelAsBase64, string returnUrl)
        {
            if (string.IsNullOrEmpty(membershipModelAsBase64))
            {
                throw new ArgumentNullException(nameof(membershipModelAsBase64));
            }
            if (string.IsNullOrEmpty(paymentModelAsBase64))
            {
                throw new ArgumentNullException(nameof(paymentModelAsBase64));
            }
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new ArgumentNullException(nameof(returnUrl));
            }

            try
            {
                MembershipModel membershipModel = (MembershipModel) _modelHelper.ToModel(membershipModelAsBase64);
                PayableModel paymentModel = (PayableModel) _modelHelper.ToModel(paymentModelAsBase64);

                if (paymentModel.PaymentStatus != PaymentStatus.Paid)
                {
                    return Redirect(returnUrl);
                }

                membershipModel.PaymentHandlerIdentifier = paymentModel.PaymentHandlerIdentifier;
                membershipModel.PaymentStatus = paymentModel.PaymentStatus;
                membershipModel.PaymentTime = paymentModel.PaymentTime;
                membershipModel.PaymentReference = paymentModel.PaymentReference;
                membershipModel.PaymentReceipt = paymentModel.PaymentReceipt;

                _householdDataRepository.UpgradeMembershipAsync(User.Identity, membershipModel)
                    .GetAwaiter()
                    .GetResult();

                return Redirect(returnUrl);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        /// <summary>
        /// Adds a new household to the current household member.
        /// </summary>
        /// <param name="statusMessage">Status message to show in the view.</param>
        /// <param name="errorMessage">Error message to show in the view.</param>
        /// <returns>View for adding a new household to the current household member.</returns>
        [IsValidatedHouseholdMember]
        public ActionResult AddHousehold(string statusMessage = null, string errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(statusMessage) == false)
            {
                ViewBag.StatusMessage = statusMessage;
            }
            if (string.IsNullOrWhiteSpace(errorMessage) == false)
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            HouseholdModel householdModel = new HouseholdModel();

            return View("AddHousehold", householdModel);
        }

        /// <summary>
        /// Adds a new household to the current household member.
        /// </summary>
        /// <param name="householdModel">Model for the household to add.</param>
        /// <returns>Redirect to the dashboard.</returns>
        [IsValidatedHouseholdMember]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddHousehold(HouseholdModel householdModel)
        {
            if (householdModel == null)
            {
                throw new ArgumentNullException(nameof(householdModel));
            }
            try
            {
                if (ModelState.IsValid == false)
                {
                    return View("AddHousehold", householdModel);
                }

                _householdDataRepository.CreateHouseholdAsync(User.Identity, householdModel, CultureInfo.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult();

                return RedirectToAction("Dashboard", "Dashboard");
            }
            catch (AggregateException ex)
            {
                ViewBag.ErrorMessage = ex.ToReduceFoodWasteException().Message;
                return View("AddHousehold", householdModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("AddHousehold", householdModel);
            }
        }

        /// <summary>
        /// Adds a local claim to the current user's identity.
        /// </summary>
        /// <param name="claim">Local claim which should be added to the current user's identity.</param>
        private void AddClaim(Claim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            
            if (User == null || User.Identity is ClaimsIdentity == false)
            {
                return;
            }

            try
            {
                _localClaimProvider.AddLocalClaimAsync((ClaimsIdentity) User.Identity, claim, System.Web.HttpContext.Current)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        #endregion
    }
}