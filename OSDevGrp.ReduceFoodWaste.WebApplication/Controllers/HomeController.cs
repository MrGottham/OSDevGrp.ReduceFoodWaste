using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        #region Private variables

        private readonly IClaimValueProvider _claimValueProvider;

        #endregion

        #region Constructor

        public HomeController(IClaimValueProvider claimValueProvider)
        {
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException("claimValueProvider");
            }
            _claimValueProvider = claimValueProvider;
        }

        #endregion

        #region Methods

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
                throw new NotImplementedException();
            }
            if (isActivatedHouseholdMember == false || isPrivacyPoliciesAccepted == false)
            {
                throw new NotImplementedException();
            }

            ViewBag.Message = string.Format(Texts.WelcomeTo, Texts.ReduceFoodWasteProject);
            return View();
        }

        #endregion
    }
}
