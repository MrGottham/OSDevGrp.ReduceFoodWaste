﻿using System.Security.Claims;
using System.Security.Principal;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Interface for a provider which can get values from claims.
    /// </summary>
    public interface IClaimValueProvider
    {
        /// <summary>
        /// Gets the user name identifier for a given identity.
        /// </summary>
        /// <param name="identity">Identity on which to get the user name identifier.</param>
        /// <returns>User name identifier for the given identity.</returns>
        string GetUserNameIdentifier(IIdentity identity);

        /// <summary>
        /// Gets the user name identifier for a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to get the user name identifier.</param>
        /// <returns>User name identifier for the given claims identity.</returns>
        string GetUserNameIdentifier(ClaimsIdentity claimsIdentity);

        /// <summary>
        /// Gets the mail address for a given identity.
        /// </summary>
        /// <param name="identity">Identity on which to get the mail address.</param>
        /// <returns>Mail address for the given identity.</returns>
        string GetMailAddress(IIdentity identity);

        /// <summary>
        /// Gets the mail address for a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to get the mail address.</param>
        /// <returns>Mail address for the given claims identity.</returns>
        string GetMailAddress(ClaimsIdentity claimsIdentity);
    }
}