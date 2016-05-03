using System.ComponentModel.DataAnnotations;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Validations
{
    public class RequiredLocalizedAttribute : RequiredAttribute
    {
        public RequiredLocalizedAttribute()
        {
            ErrorMessageResourceType = typeof(Texts);
            ErrorMessageResourceName = "ValueIsRequired";
        }
    }
}