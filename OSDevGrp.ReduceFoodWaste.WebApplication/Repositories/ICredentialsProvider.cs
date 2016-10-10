using System.Security.Principal;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Interface for a provider which can creates credentials.
    /// </summary>
    public interface ICredentialsProvider
    {
        /// <summary>
        /// Creates a user name and password credential for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to create a user name and password credential.</param>
        /// <returns>User name and password credential for the given identity.</returns>
        UserNamePasswordCredential CreateUserNamePasswordCredential(IIdentity identity);

        /// <summary>
        /// Calculates and returns a hash code for a given user name and password credential.
        /// </summary>
        /// <param name="userNamePasswordCredential">User name and password credential on which to calculate the hase code.</param>
        /// <returns>Hash code for a given user name and password credential.</returns>
        string CalculateHashForCredential(UserNamePasswordCredential userNamePasswordCredential);
    }
}
