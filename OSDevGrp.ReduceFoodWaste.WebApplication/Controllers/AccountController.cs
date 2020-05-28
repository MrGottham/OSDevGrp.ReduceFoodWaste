using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Private variables

        private readonly IConfigurationProvider _configurationProvider;
        private readonly IClaimValueProvider _claimValueProvider;
        private readonly ILocalClaimProvider _localClaimProvider;

        #endregion

        #region Constructor

        public AccountController(IConfigurationProvider configurationProvider, IClaimValueProvider claimValueProvider, ILocalClaimProvider localClaimProvider)
        {
            _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _claimValueProvider = claimValueProvider ?? throw new ArgumentNullException(nameof(claimValueProvider));
            _localClaimProvider = localClaimProvider ?? throw new ArgumentNullException(nameof(localClaimProvider));
        }

        #endregion

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Manage()
        {
            if (User?.Identity == null || User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (_claimValueProvider.IsValidatedHouseholdMember(User.Identity) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Manage", "HouseholdMember");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }

            return new ChallengeResult(AuthenticationManager, provider, GetReturnUrlForExternalLogin(returnUrl));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            ExternalLoginInfo externalLoginInfo = AuthenticationManager.GetExternalLoginInfo();
            if (externalLoginInfo == null)
            {
                return RedirectToAction("ExternalLoginFailure", new {reason = Texts.UnsuccessfulLoginWithService});
            }

            try
            {
                string mailAddress = _claimValueProvider.GetMailAddress(externalLoginInfo.ExternalIdentity);
                if (string.IsNullOrWhiteSpace(mailAddress))
                {
                    return RedirectToAction("ExternalLoginFailure", new {reason = Texts.UnableToObtainEmailAddressFromService});
                }

                ClaimsIdentity identity = new ClaimsIdentity(externalLoginInfo.ExternalIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie);
                AddLocalClaims(identity);

                identity.SignIn(AuthenticationManager);

                return RedirectToLocal(returnUrl);
            }
            finally
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            }
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure(string reason)
        {
            ViewBag.Message = reason;

            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            IEnumerable<AuthenticationDescription> loginProviders = AuthenticationManager.GetExternalAuthenticationTypes().OrderBy(loginProvider => (int) loginProvider.Properties["Priority"]);

            ViewBag.ReturnUrl = returnUrl;

            return PartialView("_ExternalLoginsListPartial", loginProviders);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private string GetReturnUrlForExternalLogin(string returnUrl)
        {
            Uri callbackAddress = _configurationProvider.SiteConfiguration.CallbackAddress;

            string action = Url.Action("ExternalLoginCallback", "Account", string.IsNullOrWhiteSpace(returnUrl) ? null : new {ReturnUrl = returnUrl});
            if (string.IsNullOrWhiteSpace(action))
            {
                return callbackAddress.AbsoluteUri.ToLower();
            }

            int startIndex = action.IndexOf("/Account/", StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0)
            {
                action = action.Substring(startIndex);
            }

            if (callbackAddress.AbsoluteUri.EndsWith("/") && action.StartsWith("/"))
            {
                return $"{callbackAddress.AbsoluteUri}{action.Substring(1)}".ToLower();
            }

            return $"{callbackAddress.AbsoluteUri}/{action}".ToLower();
        }

        private void AddLocalClaims(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException(nameof(claimsIdentity));
            }

            _localClaimProvider.AddLocalClaimsAsync(claimsIdentity)
                .GetAwaiter()
                .GetResult();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home", new { redirectToDashboard = true });
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(IAuthenticationManager authenticationManager, string provider, string returnUrl)
            {
                if (string.IsNullOrWhiteSpace(provider))
                {
                    throw new ArgumentNullException(nameof(provider));
                }

                AuthenticationManager = authenticationManager ?? throw new ArgumentNullException(nameof(authenticationManager));
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            private IAuthenticationManager AuthenticationManager { get; }
            
            private string Provider { get; }
            
            private string ReturnUrl { get; }

            public override void ExecuteResult(ControllerContext context)
            {
                AuthenticationProperties authenticationProperties = new AuthenticationProperties
                {
                    RedirectUri = ReturnUrl
                };
                AuthenticationManager.Challenge(authenticationProperties, Provider);
            }
        }

        #endregion
    }
}