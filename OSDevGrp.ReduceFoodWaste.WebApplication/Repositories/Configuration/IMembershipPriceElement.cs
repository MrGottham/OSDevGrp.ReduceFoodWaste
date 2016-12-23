namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories.Configuration
{
    /// <summary>
    /// Interface for a membership price configuration element.
    /// </summary>
    public interface IMembershipPriceElement
    {
        /// <summary>
        /// Gets the culture name for price of the membership.
        /// </summary>
        string CultureName { get; }

        /// <summary>
        /// Gets the price for the membership.
        /// </summary>
        decimal Price { get; }
    }
}