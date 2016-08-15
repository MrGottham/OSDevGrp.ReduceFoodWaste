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
        /// Returns the view for manage a given household.
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
        /// Returns the partial view for a given household's information.
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

                var householdModel = new HouseholdModel
                {
                    Identifier = householdIdentifier.Value
                };

                var task = _householdDataRepository.GetHouseholdAsync(User.Identity, householdModel, Thread.CurrentThread.CurrentUICulture);
                task.Wait();

                return PartialView("_HouseholdInformation", task.Result);
            }
            catch (AggregateException ex)
            {
                ViewBag.ErrorMessage = ex.ToReduceFoodWasteException().Message;
                return PartialView("_Empty");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return PartialView("_Empty");
            }
        }

        #endregion
    }
}