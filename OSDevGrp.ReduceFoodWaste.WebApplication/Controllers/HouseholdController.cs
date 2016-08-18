using System;
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
        /// <returns>View for manage the given household.</returns>
        public ActionResult Manage(Guid? householdIdentifier = null)
        {
            if (householdIdentifier.HasValue == false)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            var householdModel = new HouseholdModel
            {
                Identifier = householdIdentifier.Value
            };

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

            throw new NotImplementedException();
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