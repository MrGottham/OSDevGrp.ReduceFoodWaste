using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Repositories
{
    /// <summary>
    /// Converter which can convert household data.
    /// </summary>
    public class HouseholdDataConverter : IHouseholdDataConverter
    {
        #region Methods

        /// <summary>
        /// Convert a source object to a target object.
        /// </summary>
        /// <typeparam name="TSource">Type of the source object.</typeparam>
        /// <typeparam name="TTarget">Type of the target object.</typeparam>
        /// <param name="source">Source object which should be converted to a target object.</param>
        /// <returns>Target object.</returns>
        public TTarget Convert<TSource, TTarget>(TSource source)
        {
            if (Equals(source, null))
            {
                throw new ArgumentNullException("source");
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}