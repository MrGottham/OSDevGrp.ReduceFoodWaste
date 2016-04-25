using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller for a household member.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of the repository which can access household data.</param>
        public HouseholdMemberController(IHouseholdDataRepository householdDataRepository)
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