﻿using System;
using System.Globalization;
using System.Security.Claims;
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
        private readonly ILocalClaimProvider _localClaimProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller for a household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        /// <param name="claimValueProvider">Implementation of a provider which can get values from claims.</param>
        /// <param name="localClaimProvider">Implementation of a provider which can append local claims to a claims identity.</param>
        public HouseholdMemberController(IHouseholdDataRepository householdDataRepository, IClaimValueProvider claimValueProvider, ILocalClaimProvider localClaimProvider)
        {
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException("householdDataRepository");
            }
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException("claimValueProvider");
            }
            if (localClaimProvider == null)
            {
                throw new ArgumentNullException("localClaimProvider");
            }
            _householdDataRepository = householdDataRepository;
            _claimValueProvider = claimValueProvider;
            _localClaimProvider = localClaimProvider;
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

                var isPrivacyPoliciesAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);

                var privacyPolicyModel = task.Result;
                privacyPolicyModel.IsAccepted = isPrivacyPoliciesAccepted;
                privacyPolicyModel.AcceptedTime = isPrivacyPoliciesAccepted ? DateTime.Now : (DateTime?) null;

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

                var createHouseholdTask = _householdDataRepository.CreateHouseholdAsync(User.Identity, householdModel, CultureInfo.CurrentUICulture);
                createHouseholdTask.Wait();

                AddClaim(_localClaimProvider.GenerateCreatedHouseholdMemberClaim());

                if (householdModel.PrivacyPolicy == null || householdModel.PrivacyPolicy.IsAccepted == false)
                {
                    return RedirectToAction("Prepare");
                }

                try
                {
                    var acceptPrivacyPolicyTask = _householdDataRepository.AcceptPrivacyPolicyAsync(User.Identity, householdModel.PrivacyPolicy);
                    acceptPrivacyPolicyTask.Wait();

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

                var isActivatedHouseholdMember = _claimValueProvider.IsActivatedHouseholdMember(User.Identity);
                var isPrivacyPoliciesAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);

                var task = _householdDataRepository.GetPrivacyPoliciesAsync(User.Identity, CultureInfo.CurrentUICulture);
                task.Wait();

                var privacyPolicyModel = task.Result;
                privacyPolicyModel.IsAccepted = isPrivacyPoliciesAccepted;
                privacyPolicyModel.AcceptedTime = isPrivacyPoliciesAccepted ? DateTime.Now : (DateTime?) null;

                var householdMemberModel = new HouseholdMemberModel
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
                throw new ArgumentNullException("householdMemberModel");
            }
            try
            {
                var privacyPolicyGetTask = _householdDataRepository.GetPrivacyPoliciesAsync(User.Identity, Thread.CurrentThread.CurrentUICulture);
                privacyPolicyGetTask.Wait();

                var reloadedPrivacyPolicyModel = privacyPolicyGetTask.Result;
                if (householdMemberModel.PrivacyPolicy == null)
                {
                    var privacyPoliciesHasAlreadyBeenAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);
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
                    var privacyPoliciesHasAlreadyBeenAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);
                    householdMemberModel.PrivacyPolicy.IsAccepted = privacyPoliciesHasAlreadyBeenAccepted;
                    householdMemberModel.PrivacyPolicy.AcceptedTime = privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now : (DateTime?) null;
                    householdMemberModel.PrivacyPolicyAcceptedTime = privacyPoliciesHasAlreadyBeenAccepted ? DateTime.Now : (DateTime?) null;
                    return View("Prepare", householdMemberModel);
                }

                if (householdMemberModel.IsActivated == false && string.IsNullOrWhiteSpace(householdMemberModel.ActivationCode) == false)
                {
                    var activateHouseholdMemberTask = _householdDataRepository.ActivateHouseholdMemberAsync(User.Identity, householdMemberModel);
                    activateHouseholdMemberTask.Wait();

                    var activatedHouseholdMemberModel = activateHouseholdMemberTask.Result;

                    AddClaim(_localClaimProvider.GenerateActivatedHouseholdMemberClaim());

                    householdMemberModel.ActivatedTime = activatedHouseholdMemberModel.ActivatedTime;
                }

                if (householdMemberModel.HasAcceptedPrivacyPolicy == false && householdMemberModel.PrivacyPolicy != null && householdMemberModel.PrivacyPolicy.IsAccepted)
                {
                    var acceptPrivacyPolicyTask = _householdDataRepository.AcceptPrivacyPolicyAsync(User.Identity, householdMemberModel.PrivacyPolicy);
                    acceptPrivacyPolicyTask.Wait();

                    var acceptedPrivacyPolicy = acceptPrivacyPolicyTask.Result;

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
        /// <returns>View for manage a household member account.</returns>
        [IsValidatedHouseholdMember]
        public ActionResult Manage()
        {
            try
            {
                var task = _householdDataRepository.GetHouseholdMemberAsync(User.Identity, Thread.CurrentThread.CurrentUICulture);
                task.Wait();

                return View("Manage", task.Result);
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
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
                throw new ArgumentNullException("claim");
            }
            
            if (User == null || User.Identity is ClaimsIdentity == false)
            {
                return;
            }

            try
            {
                var task = _localClaimProvider.AddLocalClaimAsync((ClaimsIdentity) User.Identity, claim, System.Web.HttpContext.Current);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        #endregion
    }
}