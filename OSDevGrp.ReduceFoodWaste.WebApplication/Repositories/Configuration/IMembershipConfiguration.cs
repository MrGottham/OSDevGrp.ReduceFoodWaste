namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Interface for the configuration of memberships.
    /// </summary>
    public interface IMembershipConfiguration
    {
        /// <summary>
        /// Gets the membership configuration element for the membership named Basic.
        /// </summary>
        IMembershipElement BasicMembership { get; }

        /// <summary>
        /// Gets the membership configuration element for the membership named Deluxe.
        /// </summary>
        IMembershipElement DeluxeMembership { get; }

        /// <summary>
        /// Gets the membership configuration element for the membership named Premium.
        /// </summary>
        IMembershipElement PremiumMembership { get; }
    }
}
