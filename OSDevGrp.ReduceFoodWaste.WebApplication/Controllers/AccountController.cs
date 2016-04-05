using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Transactions;
using System.Web;
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

            string mailAddress = GetMailAddress(claimsIdentity);
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                return RedirectToAction("ExternalLoginFailure", new {reason = Texts.UnableToObtainEmailAddressFromService});
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                Response.Cookies.Add(CreateAuthenticationTicket(claimsIdentity));
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in then add the new account.
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, mailAddress);
                Response.Cookies.Add(CreateAuthenticationTicket(claimsIdentity));
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
                if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
                {
                    Response.Cookies.Add(CreateAuthenticationTicket(claimsIdentity));
                }
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
            return RedirectToAction("Index", "Home");
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
                new Claim(ClaimTypes.Name, authenticationResult.UserName, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider),
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", authenticationResult.Provider, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider)
            };
            foreach (var extraData in authenticationResult.ExtraData.Where(m => string.IsNullOrWhiteSpace(m.Value) == false))
            {
                switch (extraData.Key)
                {
                    case "id":
                        // This has already been handled.
                        break;

                    case "name":
                        switch (authenticationResult.Provider)
                        {
                            case "facebook":
                                // Remove the existing claim for the name (it contains the mail address).
                                var nameClaim = claimCollection.Single(claim => string.Compare(claim.Type, ClaimTypes.Name, StringComparison.Ordinal) == 0);
                                claimCollection.Remove(nameClaim);
                                // Add new claim for the name containing the users name.
                                claimCollection.Add(new Claim(ClaimTypes.Name, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                                break;
                        }
                        break;

                    case "link":
                        claimCollection.Add(new Claim(ClaimTypes.Webpage, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "gender":
                        claimCollection.Add(new Claim(ClaimTypes.Gender, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "username":
                        switch (authenticationResult.Provider)
                        {
                            case "facebook":
                                claimCollection.Add(new Claim(ClaimTypes.Email, extraData.Value, ClaimValueTypes.Email, authenticationResult.Provider, authenticationResult.Provider));
                                break;
                        }
                        break;

                    case "firstname":
                        claimCollection.Add(new Claim(ClaimTypes.GivenName, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "lastname":
                        claimCollection.Add(new Claim(ClaimTypes.Surname, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "birthday":
                        claimCollection.Add(new Claim(ClaimTypes.DateOfBirth, extraData.Value, ClaimValueTypes.Date, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "country":
                        claimCollection.Add(new Claim(ClaimTypes.Country, extraData.Value, ClaimValueTypes.String, authenticationResult.Provider, authenticationResult.Provider));
                        break;

                    case "email":
                    case "emails.preferred":
                    case "emails.account":
                    case "emails.personal":
                    case "emails.business":
                        claimCollection.Add(new Claim(ClaimTypes.Email, extraData.Value, ClaimValueTypes.Email, authenticationResult.Provider, authenticationResult.Provider));
                        break;
                }
            }
            return new ClaimsIdentity(claimCollection);
        }

        private static string GetMailAddress(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            return emailClaim == null ? null : emailClaim.Value;
        }

        private static HttpCookie CreateAuthenticationTicket(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }

            byte[] userData;
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof (IEnumerable<Claim>));
                serializer.WriteObject(memoryStream, claimsIdentity.Claims);

                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var compressMemoryStream = new MemoryStream())
                {
                    using (var deflateStream = new DeflateStream(compressMemoryStream, CompressionMode.Compress))
                    {
                        memoryStream.CopyTo(deflateStream);
                        deflateStream.Flush();
                        deflateStream.Close();
                    }
                    userData = compressMemoryStream.ToArray();

                    compressMemoryStream.Close();
                }
                memoryStream.Close();
            }

            var timeOut = FormsAuthentication.Timeout;
            var formsAuthenticationTicket = new FormsAuthenticationTicket(1, GetMailAddress(claimsIdentity), DateTime.Now, DateTime.Now.Add(timeOut), false, Convert.ToBase64String(userData));

            return new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(formsAuthenticationTicket));
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
