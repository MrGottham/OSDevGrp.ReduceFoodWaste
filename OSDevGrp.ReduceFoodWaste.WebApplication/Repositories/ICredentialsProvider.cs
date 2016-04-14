using System.Security.Principal;
using System.ServiceModel.Security;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Interface for a provider which can creates credentials.
    /// </summary>
    public interface ICredentialsProvider
    {
        /// <summary>
        /// Creates a user name and password client credential for a given identity.
        /// </summary>
        /// <param name="identity">Identity for which to create a user name and password client credential.</param>
        /// <returns>User name and password client credential for the given identity.</returns>
        UserNamePasswordClientCredential CreateUserNamePasswordClientCredential(IIdentity identity);
    }
}
