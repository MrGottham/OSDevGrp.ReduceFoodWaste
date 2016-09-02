using System.ComponentModel.DataAnnotations;
using System.Web;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Validations
{
    public class MailAddressLocalizedAttribute : RegularExpressionAttribute
    {
        #region Constructor

        public MailAddressLocalizedAttribute() 
            : this(HttpContext.Current)
        {
        }

        public MailAddressLocalizedAttribute(HttpContext httpContext)
            : base(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$")
        {
            if (httpContext != null)
            {
                var httpContextBase = new HttpContextWrapper(httpContext);
                var cultureName = CultureAttribute.GetSavedCultureOrDefault(httpContextBase.Request);
                CultureAttribute.SetCultureOnThread(cultureName);
            }
            ErrorMessageResourceType = typeof(Texts);
            ErrorMessageResourceName = "ValueNotValidMailAddress";
        }

        #endregion
    }
}