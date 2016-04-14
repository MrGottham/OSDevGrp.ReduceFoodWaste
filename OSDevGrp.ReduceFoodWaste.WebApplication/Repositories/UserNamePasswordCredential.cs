using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// User name and password credential.
    /// </summary>
    public class UserNamePasswordCredential
    {
        #region Constructor

        /// <summary>
        /// Creates an user name and password credential.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        public UserNamePasswordCredential(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }
            UserName = userName;
            Password = password;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the user name.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; private set; }

        #endregion
    }
}