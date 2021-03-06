﻿using System;
using System.Security.Principal;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using Unity;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Filters
{
    /// <summary>
    /// Attribute which can insure that the user is a validated household member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IsValidatedHouseholdMemberAttribute : IsAuthenticatedAttribute
    {
        #region Private variables

        private readonly IClaimValueProvider _claimValueProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an attribute which can insure that the user is a validated household member.
        /// </summary>
        public IsValidatedHouseholdMemberAttribute()
            : this(UnityConfig.Container.Resolve<IClaimValueProvider>())
        {
        }

        /// <summary>
        /// Creates an attribute which can insure that the user is a validated household member.
        /// </summary>
        /// <param name="claimValueProvider">Implementation of a provider which can get values from claims.</param>
        public IsValidatedHouseholdMemberAttribute(IClaimValueProvider claimValueProvider)
        {
            if (claimValueProvider == null)
            {
                throw new ArgumentNullException("claimValueProvider");
            }
            _claimValueProvider = claimValueProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates whether a given identity is a validated household member.
        /// </summary>
        /// <param name="identity">Identity which should be examined.</param>
        /// <returns>True when the given identity is a validated household member otherwise false.</returns>
        protected override bool ValidateIdentity(IIdentity identity)
        {
            return identity != null && _claimValueProvider.IsValidatedHouseholdMember(identity);
        }

        #endregion
    }
}