using System;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
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
        /// Creates a user name and password credential for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to create a user name and password credential.</param>
        /// <returns>User name and password credential for the given identity.</returns>
        public UserNamePasswordCredential CreateUserNamePasswordCredential(IIdentity identity)
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

            return new UserNamePasswordCredential(mailAddress, userNameIdentifier);
        }

        /// <summary>
        /// Calculates and returns a hash code for a given user name and password credential.
        /// </summary>
        /// <param name="userNamePasswordCredential">User name and password credential on which to calculate the hase code.</param>
        /// <returns>Hash code for a given user name and password credential.</returns>
        public string CalculateHashForCredential(UserNamePasswordCredential userNamePasswordCredential)
        {
            if (userNamePasswordCredential == null)
            {
                throw new ArgumentNullException("userNamePasswordCredential");
            }

            using (var md5 = MD5.Create())
            {
                return Convert.ToBase64String(md5.ComputeHash(Encoding.Default.GetBytes(string.Format("{0}:{1}", userNamePasswordCredential.UserName, userNamePasswordCredential.Password))));
            }
        }

        #endregion
    }
}