using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Validations;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public static class AdapterConfig
    {
        public static void RegisterAdapters()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredLocalizedAttribute), typeof(RequiredAttributeAdapter));
        }
    }
}