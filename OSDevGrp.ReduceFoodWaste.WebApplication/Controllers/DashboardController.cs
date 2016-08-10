using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    /// <summary>
    /// Controller for a household members dashboard.
    /// </summary>
    [Authorize]
    [IsValidatedHouseholdMember]
    public class DashboardController : Controller
    {
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
                throw new NotImplementedException();
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