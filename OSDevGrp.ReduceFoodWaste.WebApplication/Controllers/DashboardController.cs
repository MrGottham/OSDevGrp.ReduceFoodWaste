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
    /// Controller for a household members dashboard.
    /// </summary>
    [Authorize]
    [IsValidatedHouseholdMember]
    public class DashboardController : Controller
    {
        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller for a household members dashboard.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        public DashboardController(IHouseholdDataRepository householdDataRepository)
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
        /// Returns the dashboard view for the current household member.
        /// </summary>
        /// <returns>Dashboard view for the current household member.</returns>
        public ActionResult Dashboard()
        {
            var dashboardModel = new DashboardModel();
            return View(dashboardModel);
        }

        /// <summary>
        /// Returns the partial view for the household member information in the dashboard.
        /// </summary>
        /// <returns>Partial view for the household member information in the dashboard.</returns>
        public ActionResult HouseholdMemberInformation()
        {
            try
            {
                var task = _householdDataRepository.GetHouseholdMemberAsync(User.Identity, Thread.CurrentThread.CurrentUICulture);
                task.Wait();

                var dashboardModel = new DashboardModel
                {
                    HouseholdMember = task.Result
                };

                return PartialView("_HouseholdMemberInformation", dashboardModel);
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