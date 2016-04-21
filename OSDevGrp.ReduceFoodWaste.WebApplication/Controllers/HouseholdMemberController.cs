using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    /// <summary>
    /// Controller for a household member.
    /// </summary>
    [Authorize]
    [IsAuthenticated]
    public class HouseholdMemberController : Controller
    {
        #region Methods

        /// <summary>
        /// Creates a new household member.
        /// </summary>
        /// <returns>View for creating a new household member.</returns>
        public ActionResult Create()
        {
            throw new NotImplementedException();
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