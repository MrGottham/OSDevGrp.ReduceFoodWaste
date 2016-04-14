using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Provider which can append local claims to a claims identity.
    /// </summary>
    public class LocalClaimProvider : ILocalClaimProvider
    {
        #region Methods

        /// <summary>
        /// Add the local claims to a given claims identity.
        /// </summary>
        /// <param name="claimsIdentity">Claims identity on which to add the local claims.</param>
        /// <returns>Task which will adds the local claims to the given claims identity.</returns>
        public Task AddLocalClaimsAsync(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}