using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    /// <summary>
    /// Controller for a household.
    /// </summary>
    [Authorize]
    [IsValidatedHouseholdMember]
    public class HouseholdController : Controller
    {
        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller for a household.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        public HouseholdController(IHouseholdDataRepository householdDataRepository)
        {
            if (householdDataRepository == null)
            {
                throw new ArgumentNullException("householdDataRepository");
            }
            _householdDataRepository = householdDataRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a view for manage a given household.
        /// </summary>
        /// <param name="householdIdentifier">Identification for the household to manage.</param>
        /// <param name="statusMessage">Status message to show in the view.</param>
        /// <param name="errorMessage">Error message to show in the view.</param>
        /// <returns>View for manage the given household.</returns>
        public ActionResult Manage(Guid? householdIdentifier = null, string statusMessage = null, string errorMessage = null)
        {
            if (householdIdentifier.HasValue == false)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            var householdModel = new HouseholdModel
            {
                Identifier = householdIdentifier.Value
            };

            if (string.IsNullOrWhiteSpace(statusMessage) == false)
            {
                ViewBag.StatusMessage = statusMessage;
            }
            if (string.IsNullOrWhiteSpace(errorMessage) == false)
            {
                ViewBag.ErrorMessage = errorMessage;
            }

            return View("Manage", householdModel);
        }

        /// <summary>
        /// Returns a partial view for a given household's information.
        /// </summary>
        /// <param name="householdIdentifier">Identification for the household on which to view the household's information.</param>
        /// <returns>Partial view for the given household's information.</returns>
        public ActionResult HouseholdInformation(Guid? householdIdentifier = null)
        {
            try
            {
                if (householdIdentifier.HasValue == false)
                {
                    return PartialView("_Empty");
                }

                var householdModel = GetHouseholdModel(householdIdentifier.Value);

                ViewBag.EditMode = false;

                return PartialView("_HouseholdInformation", householdModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return PartialView("_Empty");
            }
        }

        /// <summary>
        /// Returns a partial view for editing a information on a given household.
        /// </summary>
        /// <param name="householdIdentifier">Identification for the household on which to edit information.</param>
        /// <returns>Partial view for editing a information on the given household.</returns>
        public ActionResult Edit(Guid? householdIdentifier = null)
        {
            try
            {
                if (householdIdentifier.HasValue == false)
                {
                    return PartialView("_Empty");
                }

                var householdModel = GetHouseholdModel(householdIdentifier.Value);

                ViewBag.EditMode = true;

                return PartialView("Edit", householdModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return PartialView("_Empty");
            }
        }

        /// <summary>
        /// Updates a given household with values in the household model and redirect to the management of the updated household.
        /// </summary>
        /// <param name="householdModel">Household model containing the values to update.</param>
        /// <returns>Redirect to the management of the updated household.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HouseholdModel householdModel)
        {
            if (householdModel == null)
            {
                throw new ArgumentNullException("householdModel");
            }

            try
            {
                if (ModelState.IsValid == false)
                {
                    var reloadedHouseholdModel = GetHouseholdModel(householdModel.Identifier);
                    householdModel.HouseholdMembers = reloadedHouseholdModel.HouseholdMembers;

                    ViewBag.EditMode = true;

                    return View("Manage", householdModel);
                }
                
                var task = _householdDataRepository.UpdateHouseholdAsync(User.Identity, householdModel);
                task.Wait();

                return RedirectToAction("Manage", "Household", new {householdIdentifier = task.Result.Identifier});
            }
            catch (AggregateException ex)
            {
                return RedirectToAction("Manage", "Household", new {householdIdentifier = householdModel.Identifier, errorMessage = ex.ToReduceFoodWasteException().Message});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Household", new {householdIdentifier = householdModel.Identifier, errorMessage = ex.Message});
            }
        }

        /// <summary>
        /// Returns a partial view for adding another household member to a given household.
        /// </summary>
        /// <param name="householdIdentifier">Identification for the household on which to add another household member.</param>
        /// <returns>Partial view for adding another household member to the given household.</returns>
        public ActionResult AddHouseholdMember(Guid? householdIdentifier = null)
        {
            try
            {
                if (householdIdentifier.HasValue == false)
                {
                    return PartialView("_Empty");
                }

                var memberOfHouseholdModel = new MemberOfHouseholdModel
                {
                    HouseholdIdentifier = householdIdentifier.Value
                };

                return PartialView("_AddHouseholdMember", memberOfHouseholdModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return PartialView("_Empty");
            }
        }

        /// <summary>
        /// Adds a household member to a given household.
        /// </summary>
        /// <param name="memberOfHouseholdModel">Model for the household member who should be added to the household.</param>
        /// <returns>Redirect to the management of the household on which the household member was added.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddHouseholdMember(MemberOfHouseholdModel memberOfHouseholdModel)
        {
            if (memberOfHouseholdModel == null)
            {
                throw new ArgumentNullException("memberOfHouseholdModel");
            }

            try
            {
                if (ModelState.IsValid == false)
                {
                    var householdModel = GetHouseholdModel(memberOfHouseholdModel.HouseholdIdentifier);

                    IList<MemberOfHouseholdModel> householdMembers = householdModel.HouseholdMembers == null ? new List<MemberOfHouseholdModel>() : householdModel.HouseholdMembers.ToList();
                    householdMembers.Add(memberOfHouseholdModel);
                    householdModel.HouseholdMembers = householdMembers;

                    ViewBag.AddingHouseholdMemberMode = true;

                    return View("Manage", householdModel);
                }

                var task = _householdDataRepository.AddHouseholdMemberToHouseholdAsync(User.Identity, memberOfHouseholdModel, Thread.CurrentThread.CurrentUICulture);
                task.Wait();

                return RedirectToAction("Manage", "Household", new {householdIdentifier = task.Result.HouseholdIdentifier});
            }
            catch (AggregateException ex)
            {
                return RedirectToAction("Manage", "Household", new {householdIdentifier = memberOfHouseholdModel.HouseholdIdentifier, errorMessage = ex.ToReduceFoodWasteException().Message});
            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Household", new {householdIdentifier = memberOfHouseholdModel.HouseholdIdentifier, errorMessage = ex.Message});
            }
        }

        /// <summary>
        /// Removes a household member from a given household.
        /// </summary>
        /// <param name="memberOfHouseholdModel">Model for the household member who should be removed to the household.</param>
        /// <returns>Redirect to the management of the household on which the household member was removed.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveHouseholdMember(MemberOfHouseholdModel memberOfHouseholdModel)
        {
            if (memberOfHouseholdModel == null)
            {
                throw new ArgumentNullException("memberOfHouseholdModel");
            }

            try
            {
                var task = _householdDataRepository.RemoveHouseholdMemberFromHouseholdAsync(User.Identity, memberOfHouseholdModel);
                task.Wait();

                return RedirectToAction("Manage", "Household", new { householdIdentifier = task.Result.HouseholdIdentifier });
            }
            catch (AggregateException ex)
            {
                return RedirectToAction("Manage", "Household", new { householdIdentifier = memberOfHouseholdModel.HouseholdIdentifier, errorMessage = ex.ToReduceFoodWasteException().Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Household", new { householdIdentifier = memberOfHouseholdModel.HouseholdIdentifier, errorMessage = ex.Message });
            }
        }

        /// <summary>
        /// Gets the household model for a given household.
        /// </summary>
        /// <param name="householdIdentifier">Identification for the household to get.</param>
        /// <returns>Household model for the given household.</returns>
        private HouseholdModel GetHouseholdModel(Guid householdIdentifier)
        {
            try
            {
                var householdModel = new HouseholdModel
                {
                    Identifier = householdIdentifier
                };

                var task = _householdDataRepository.GetHouseholdAsync(User.Identity, householdModel, Thread.CurrentThread.CurrentUICulture);
                task.Wait();

                return task.Result;
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        #endregion
    }
}