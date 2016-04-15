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

        public ActionResult Index()
        {
            if (User == null || User.Identity == null)
            {
                ViewBag.Message = string.Format(Texts.WelcomeTo, Texts.ReduceFoodWasteProject);
                return View();
            }

            var isAuthenticated = _claimValueProvider.IsAuthenticated(User.Identity);
            // TODO: Hvis falsk...

            var isValidatedHouseholdMember = _claimValueProvider.IsValidatedHouseholdMember(User.Identity);

            ViewBag.Message = string.Format(Texts.WelcomeTo, Texts.ReduceFoodWasteProject);
            return View();
        }
    }
}
