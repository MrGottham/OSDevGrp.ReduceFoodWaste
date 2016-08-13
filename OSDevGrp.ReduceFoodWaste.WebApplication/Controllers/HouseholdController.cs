﻿using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
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
        /// Returns the view for manage a household.
        /// </summary>
        /// <param name="householdIdentifier">Identification for the household to manage.</param>
        /// <returns>View for manage a household.</returns>
        public ActionResult Manage(Guid householdIdentifier)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}