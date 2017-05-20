﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Transactions;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Authentication;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;
using WebMatrix.WebData;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
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
            if (configurationProvider == null)
            {
                throw new ArgumentNullException(nameof(configurationProvider));
            }
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException(nameof(claimValueProvider));
            }
            if (localClaimProvider == null)
            {
                throw new ArgumentNullException(nameof(localClaimProvider));
            }
            _configurationProvider = configurationProvider;
            _claimValueProvider = claimValueProvider;
            _localClaimProvider = localClaimProvider;
        }

        #endregion

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
            if (string.IsNullOrEmpty(provider))
            {
                throw new ArgumentNullException(nameof(provider));
            }
            if (string.IsNullOrEmpty(providerUserId))
            {
                throw new ArgumentNullException(nameof(providerUserId));
            }

            var mailAddress = _claimValueProvider.GetMailAddress(User.Identity);
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                return RedirectToAction("Manage", "Account");
            }

            var accountOwner = OAuthWebSecurity.GetUserName(provider, providerUserId);
            if (string.Compare(accountOwner, mailAddress, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return RedirectToAction("Manage", "Account");
            }

            var userNameIdentifier = _claimValueProvider.GetUserNameIdentifier(User.Identity);
            if (string.IsNullOrWhiteSpace(userNameIdentifier) || string.Compare(providerUserId, userNameIdentifier, StringComparison.Ordinal) == 0)
            {
                return RedirectToAction("Manage", "Account");
            }

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    var hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(mailAddress));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(mailAddress).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                    }
                }

                return RedirectToAction("Manage", "Account");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Manage", "Account", new {errorMessage = ex.Message});
            }
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage()
        {
            if (User == null || User.Identity == null || User.Identity.IsAuthenticated == false)
            {
                return RedirectToAction("Index", "Home");
            }

            if (_claimValueProvider.IsValidatedHouseholdMember(User.Identity) == false)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Manage", "HouseholdMember");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, GetReturnUrlForExternalLogin(returnUrl));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            GooglePlusScopedClient.RewriteRequest(HttpContext);

            AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(GetReturnUrlForExternalLogin(returnUrl));
            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure", new { reason = Texts.UnsuccessfulLoginWithService });
            }

            var claimsIdentity = result.ToClaimsIdentity();

            string mailAddress = _claimValueProvider.GetMailAddress(claimsIdentity);
            if (string.IsNullOrWhiteSpace(mailAddress))
            {
                return RedirectToAction("ExternalLoginFailure", new { reason = Texts.UnableToObtainEmailAddressFromService });
            }

            if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                AddLocalClaims(claimsIdentity);
                Response.Cookies.Add(claimsIdentity.Claims.ToAuthenticationTicket(mailAddress));
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in then add the new account.
                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, mailAddress);
                AddLocalClaims(claimsIdentity);
                Response.Cookies.Add(claimsIdentity.Claims.ToAuthenticationTicket(mailAddress));
                return RedirectToLocal(returnUrl);
            }

            using (var context = new UsersContext())
            {
                var userProfile = context.UserProfiles.FirstOrDefault(up => string.Compare(mailAddress, up.UserName, StringComparison.OrdinalIgnoreCase) == 0);
                if (userProfile == null)
                {
                    context.UserProfiles.Add(new UserProfile { UserName = mailAddress });
                    context.SaveChanges();
                }

                OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, mailAddress);
                if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
                {
                    AddLocalClaims(claimsIdentity);
                    Response.Cookies.Add(claimsIdentity.Claims.ToAuthenticationTicket(mailAddress));
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

        public ActionResult RemoveExternalLogins()
        {
            try
            {
                var mailAddress = _claimValueProvider.GetMailAddress(User.Identity);
                if (string.IsNullOrWhiteSpace(mailAddress))
                {
                    return PartialView("_RemoveExternalLoginsPartial", new List<ExternalLogin>(0));
                }

                var userNameIdentifier = _claimValueProvider.GetUserNameIdentifier(User.Identity);
                if (string.IsNullOrWhiteSpace(userNameIdentifier))
                {
                    return PartialView("_RemoveExternalLoginsPartial", new List<ExternalLogin>(0));
                }

                var externalLogins = OAuthWebSecurity.GetAccountsFromUserName(mailAddress)
                    .Select(account =>
                    {
                        var clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);
                        return new ExternalLogin
                        {
                            Provider = account.Provider,
                            ProviderDisplayName = clientData.DisplayName,
                            ProviderUserId = account.ProviderUserId,
                            Removable = string.Compare(account.ProviderUserId, userNameIdentifier, StringComparison.Ordinal) != 0
                        };
                    })
                    .ToArray();

                return PartialView("_RemoveExternalLoginsPartial", externalLogins);
            }
            catch (ReduceFoodWasteExceptionBase ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return PartialView("_Empty");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return PartialView("_Empty");
            }
        }

        #region Helpers

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
            try
            {
                var addLocalClaimsTask = _localClaimProvider.AddLocalClaimsAsync(claimsIdentity);
                addLocalClaimsTask.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.ToReduceFoodWasteException();
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { redirectToDashboard = true });
        }

        private class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            private string Provider { get; }
            private string ReturnUrl { get; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        #endregion
    }
}
