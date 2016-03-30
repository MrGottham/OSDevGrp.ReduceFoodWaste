using System;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Controllers
{
    public class LanguageController : Controller
    {
        public void Set(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                throw new ArgumentNullException("language");
            }

            // Set the preffered language to use next.
            CultureAttribute.SavePrefferedCulture(HttpContext.Response, language);

            // Return to the calling URL.
            HttpContext.Response.Redirect(HttpContext.Request.UrlReferrer.AbsolutePath);
        }
    }
}