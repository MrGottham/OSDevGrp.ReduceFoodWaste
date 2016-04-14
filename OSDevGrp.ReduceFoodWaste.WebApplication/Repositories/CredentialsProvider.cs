using System;
using System.Reflection;
using System.Security.Principal;
using System.ServiceModel.Security;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Provider which can creates credentials.
    /// </summary>
    public class CredentialsProvider : ICredentialsProvider
    {
        #region Private variables

        private readonly IClaimValueProvider _claimValueProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the provider which can creates credentials.
        /// </summary>
        /// <param name="claimValueProvider">Implementation of the provider which can get values from claims.</param>
        public CredentialsProvider(IClaimValueProvider claimValueProvider)
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
        /// Creates a user name and password client credential for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to create a user name and password client credential.</param>
        /// <returns>User name and password client credential for the given identity.</returns>
        public UserNamePasswordClientCredential CreateUserNamePasswordClientCredential(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            var mailAddress = _claimValueProvider.GetMailAddress(identity);
            var userNameIdentifier = _claimValueProvider.GetUserNameIdentifier(identity);

            if (string.IsNullOrEmpty(mailAddress))
            {
                throw new ReduceFoodWasteRepositoryException(Texts.CannotResolveMailAddressFromIdentity, MethodBase.GetCurrentMethod());
            }
            if (string.IsNullOrEmpty(userNameIdentifier))
            {
                throw new ReduceFoodWasteRepositoryException(Texts.CannotResolveUserNameIdentifierFromIdentity, MethodBase.GetCurrentMethod());
            }

            return new UserNamePasswordClientCredential
            {
                UserName = mailAddress,
                Password = userNameIdentifier
            };
        }

        #endregion
    }
}