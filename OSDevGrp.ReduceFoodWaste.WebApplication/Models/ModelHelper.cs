using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Helper for models.
    /// </summary>
    public static class ModelHelper
    {
        public static string ToBase64(this Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException();
            }
        }
    }
}