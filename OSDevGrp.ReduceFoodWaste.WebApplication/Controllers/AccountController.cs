using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;
using WebMatrix.WebData;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new {ReturnUrl = returnUrl}));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new {ReturnUrl = returnUrl}));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure", new {reason = Texts.UnsuccessfulLoginWithService});
            }

            var claimsIdentity = ToClaimsIdentity(result);

            string mailAddress = null;
            var mailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            if (mailClaim != null)
            {
                mailAddress = mailClaim.Value;
            }
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                return RedirectToAction("ExternalLoginFailure", new {reason = Texts.UnableToObtainEmailAddressFromService});
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in then add the new account.
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, mailAddress);
                if (User.Identity as ClaimsIdentity == null)
                {
                }
                return RedirectToLocal(returnUrl);
            }

            using (var context = new UsersContext())
            {
                var userProfile = context.UserProfiles.FirstOrDefault(up => string.Compare(mailAddress, up.UserName, StringComparison.OrdinalIgnoreCase) == 0);
                if (userProfile == null)
                {
                    context.UserProfiles.Add(new UserProfile {UserName = mailAddress});
                    context.SaveChanges();
                }

                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, mailAddress);
                OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false);
            }

            return RedirectToLocal(returnUrl);

        }

        //
        // GET: /Account/ExternalLoginFailure
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
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            foreach (OAuthAccount account in accounts)
            {
                AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new ExternalLogin
                {
                    Provider = account.Provider,
                    ProviderDisplayName = clientData.DisplayName,
                    ProviderUserId = account.ProviderUserId,
                });
            }

            ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private static ClaimsIdentity ToClaimsIdentity(AuthenticationResult authenticationResult)
        {
            if (authenticationResult == null)
            {
                throw new ArgumentNullException("authenticationResult");
            }

            var claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authenticationResult.ProviderUserId, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider),
                new Claim(ClaimTypes.Name, authenticationResult.UserName, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider)
            };
            if (authenticationResult.ExtraData != null && authenticationResult.ExtraData.ContainsKey("link"))
            {
                claimCollection.Add(new Claim(ClaimTypes.Webpage, authenticationResult.ExtraData["link"], ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
            }
            if (authenticationResult.ExtraData != null && authenticationResult.ExtraData.ContainsKey("gender"))
            {
                claimCollection.Add(new Claim(ClaimTypes.Gender, authenticationResult.ExtraData["gender"], ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
            }
            switch (authenticationResult.Provider.ToLower())
            {
                case "microsoft":
                    if (authenticationResult.ExtraData != null && authenticationResult.ExtraData.ContainsKey("firstname"))
                    {
                        claimCollection.Add(new Claim(ClaimTypes.GivenName, authenticationResult.ExtraData["firstname"], ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                    }
                    if (authenticationResult.ExtraData != null && authenticationResult.ExtraData.ContainsKey("lastname"))
                    {
                        claimCollection.Add(new Claim(ClaimTypes.Surname, authenticationResult.ExtraData["lastname"], ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                    }
                    if (authenticationResult.ExtraData != null && authenticationResult.ExtraData.ContainsKey("emails.account"))
                    {
                        claimCollection.Add(new Claim(ClaimTypes.Email, authenticationResult.ExtraData["emails.account"], ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                    }
                    break;
            }
            return new ClaimsIdentity(claimCollection);
        }


        // TODO Check whether we should use these...

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        #endregion
    }
}
