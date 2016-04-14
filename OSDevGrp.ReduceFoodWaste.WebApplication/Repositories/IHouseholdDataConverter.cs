namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Interface to a converter which can convert household data.
    /// </summary>
    public interface IHouseholdDataConverter
    {
        /// <summary>
        /// Convert a source object to a target object.
        /// </summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <param name="source">Source object which should be converted to a target object.</param>
        /// <returns>Target object.</returns>
        TTarget Convert<TSource, TTarget>(TSource source);
    }
}
