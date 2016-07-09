﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Cookies;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    [AllowAnonymous]
    [CookieConsent]
    public class HomeController : Controller
    {
        #region Private variables

        private readonly IClaimValueProvider _claimValueProvider;
        private readonly ICookieHelper _cookieHelper;

        #endregion

        #region Constructor

        public HomeController(IClaimValueProvider claimValueProvider, ICookieHelper cookieHelper)
        {
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException("claimValueProvider");
            }
            if (cookieHelper == null)
            {
                throw new ArgumentNullException("cookieHelper");
            }
            _claimValueProvider = claimValueProvider;
            _cookieHelper = cookieHelper;
        }

        #endregion

        #region Properties

        public static IEnumerable<string> TopImages
        {
            get
            {
                return new List<string>
                {
                    "~/Images/FoodWaste01.png",
                    "~/Images/FoodWaste02.png",
                    "~/Images/FoodWaste03.png",
                    "~/Images/FoodWaste04.png",
                    "~/Images/FoodWaste05.png"
                };
            }
        }

        public static string TopImage
        {
            get
            {
                var random = new Random(DateTime.Now.Millisecond);
                var topImageNo = random.Next(1, 1000) % TopImages.Count();
                return TopImages.ElementAt(topImageNo);
            }
        }

        #endregion

        #region Methods

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User == null || User.Identity == null)
            {
                ViewBag.Message = string.Format(Texts.WelcomeTo, Texts.ReduceFoodWasteProject);
                return View();
            }

            var isAuthenticated = _claimValueProvider.IsAuthenticated(User.Identity);
            if (isAuthenticated == false)
            {
                ViewBag.Message = string.Format(Texts.WelcomeTo, Texts.ReduceFoodWasteProject);
                return View();
            }

            var isValidatedHouseholdMember = _claimValueProvider.IsValidatedHouseholdMember(User.Identity);
            var isCreatedHouseholdMember = _claimValueProvider.IsCreatedHouseholdMember(User.Identity);
            var isActivatedHouseholdMember = _claimValueProvider.IsActivatedHouseholdMember(User.Identity);
            var isPrivacyPoliciesAccepted = _claimValueProvider.IsPrivacyPoliciesAccepted(User.Identity);

            if (isValidatedHouseholdMember)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

            if (isCreatedHouseholdMember == false)
            {
                return RedirectToAction("Create", "HouseholdMember");
            }
            if (isActivatedHouseholdMember == false || isPrivacyPoliciesAccepted == false)
            {
                return RedirectToAction("Prepare", "HouseholdMember");
            }

            throw new NotSupportedException();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllowCookies(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                throw new ArgumentNullException("returnUrl");
            }

            _cookieHelper.SetCookieConsent(HttpContext.Response, true);

            return new RedirectResult(returnUrl);
        }

        #endregion
    }
}
