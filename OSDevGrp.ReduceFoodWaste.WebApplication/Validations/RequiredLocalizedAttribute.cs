using System.ComponentModel.DataAnnotations;
using System.Web;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Validations
{
    public class RequiredLocalizedAttribute : RequiredAttribute
    {
        #region Constructors

        public RequiredLocalizedAttribute() 
            : this(HttpContext.Current)
        {
        }

        private RequiredLocalizedAttribute(HttpContext httpContext)
        {
            if (httpContext != null)
            {
                var httpContextBase = new HttpContextWrapper(httpContext);
                var cultureName = CultureAttribute.GetSavedCultureOrDefault(httpContextBase.Request);
                CultureAttribute.SetCultureOnThread(cultureName);
            }
            ErrorMessageResourceType = typeof(Texts);
            ErrorMessageResourceName = "ValueIsRequired";
        }

        #endregion
    }
}