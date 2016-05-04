using System.ComponentModel.DataAnnotations;
using System.Web;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Validations
{
    public class MaxLengthLocalizedAttribute : MaxLengthAttribute
    {
        #region Constructors

        public MaxLengthLocalizedAttribute(int length)
            : this(length, HttpContext.Current)
        {
        }

        private MaxLengthLocalizedAttribute(int length, HttpContext httpContext) 
            : base(length)
        {
            if (httpContext != null)
            {
                var httpContextBase = new HttpContextWrapper(httpContext);
                var cultureName = CultureAttribute.GetSavedCultureOrDefault(httpContextBase.Request);
                CultureAttribute.SetCultureOnThread(cultureName);
            }
            ErrorMessageResourceType = typeof(Texts);
            ErrorMessageResourceName = "ExceedsMaximumLength";
        }
        
        #endregion
    }
}