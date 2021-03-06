﻿using System;
using System.Threading;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    /// <summary>
    /// Controller for the sidebar.
    /// </summary>
    [Authorize]
    [IsValidatedHouseholdMember]
    public class SidebarController : Controller
    {
        #region Private variables

        private readonly IHouseholdDataRepository _householdDataRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a controller for the sidebar.
        /// </summary>
        /// <param name="householdDataRepository">Implementation of a repository which can access household data.</param>
        public SidebarController(IHouseholdDataRepository householdDataRepository)
        {
            _householdDataRepository = householdDataRepository ?? throw new ArgumentNullException(nameof(householdDataRepository));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a partial view containing the household identification collection for the current identity.
        /// </summary>
        /// <returns>Partial view containing the household identification collection for the current identity.</returns>
        public ActionResult HouseholdIdentificationCollection()
        {
            try
            {
                Models.HouseholdIdentificationCollectionModel householdIdentificationCollection = _householdDataRepository.GetHouseholdIdentificationCollectionAsync(User.Identity, Thread.CurrentThread.CurrentUICulture)
                    .GetAwaiter()
                    .GetResult();

                return PartialView("_HouseholdIdentificationCollection", householdIdentificationCollection);
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