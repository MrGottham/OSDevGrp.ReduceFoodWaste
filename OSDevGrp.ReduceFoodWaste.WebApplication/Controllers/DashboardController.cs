using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    /// <summary>
    /// Controller for a household members dashboard.
    /// </summary>
    [IsValidatedHouseholdMember]
    public class DashboardController : Controller
    {
        #region Methods

        /// <summary>
        /// Returns the dashboard view for the current household member.
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}