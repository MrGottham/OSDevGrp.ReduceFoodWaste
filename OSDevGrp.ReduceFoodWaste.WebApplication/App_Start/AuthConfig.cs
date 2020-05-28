using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.MicrosoftAccount;
using Owin;
using SameSiteMode = Microsoft.Owin.SameSiteMode;

[assembly: OwinStartup(typeof(OSDevGrp.ReduceFoodWaste.WebApplication.AuthConfig))]
namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public static class AuthConfig
    {
        public static void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/LogOff"),
                CookieSameSite = SameSiteMode.None,
                CookieSecure = CookieSecureOption.SameAsRequest
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.AddMicrosoftAccountAuthentication()
                .AddGoogleAuthentication()
                .AddFacebookAuthentication();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }

        private static IAppBuilder AddMicrosoftAccountAuthentication(this IAppBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            string clientId = GetRequiredAppSetting("MicrosoftClientId");
            string clientSecret = GetRequiredAppSetting("MicrosoftClientSecret");

            MicrosoftAccountAuthenticationOptions options = new MicrosoftAccountAuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            options.Scope.AddScopeValue("User.Read");

            options.Description.Properties.Add("Priority", 1);
            options.Description.Properties.Add("Icon", VirtualPathUtility.ToAbsolute("~/Images/microsoft.png"));

            builder.UseMicrosoftAccountAuthentication(options);

            return builder;
        }

        private static IAppBuilder AddGoogleAuthentication(this IAppBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            string clientId = GetRequiredAppSetting("GoogleClientId");
            string clientSecret = GetRequiredAppSetting("GoogleClientSecret");

            GoogleOAuth2AuthenticationOptions options = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            options.Scope.AddScopeValue("openid")
                .AddScopeValue("profile")
                .AddScopeValue("email");

            options.Description.Properties.Add("Priority", 2);
            options.Description.Properties.Add("Icon", VirtualPathUtility.ToAbsolute("~/Images/google.png"));

            builder.UseGoogleAuthentication(options);

            return builder;
        }

        private static IAppBuilder AddFacebookAuthentication(this IAppBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            string clientId = GetRequiredAppSetting("FacebookClientId");
            string clientSecret = GetRequiredAppSetting("FacebookClientSecret");

            FacebookAuthenticationOptions options = new FacebookAuthenticationOptions
            {
                AppId = clientId,
                AppSecret = clientSecret
            };

            options.Scope.AddScopeValue("public_profile")
                .AddScopeValue("email");

            options.Description.Properties.Add("Priority", 3);
            options.Description.Properties.Add("Icon", VirtualPathUtility.ToAbsolute("~/Images/facebook.png"));

            builder.UseFacebookAuthentication(options);

            return builder;
        }

        private static IList<string> AddScopeValue(this IList<string> scopeCollection, string scopeValue)
        {
            if (scopeCollection == null)
            {
                throw new ArgumentNullException(nameof(scopeCollection));
            }
            if (string.IsNullOrWhiteSpace(scopeValue))
            {
                throw new ArgumentNullException(nameof(scopeValue));
            }

            if (scopeCollection.Any(value => string.Compare(value, scopeValue, StringComparison.OrdinalIgnoreCase) == 0))
            {
                return scopeCollection;
            }

            scopeCollection.Add(scopeValue);

            return scopeCollection;
        }

        private static string GetRequiredAppSetting(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            string value = ConfigurationManager.AppSettings[name];

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception($"The application setting named '{name}' has not been defined.");
            }

            return value;
        }
    }
}