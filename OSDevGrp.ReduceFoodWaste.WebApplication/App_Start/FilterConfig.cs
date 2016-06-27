using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;
using HandleErrorAttribute = OSDevGrp.ReduceFoodWaste.WebApplication.Filters.HandleErrorAttribute;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CultureAttribute());
            filters.Add(new CookieConsentAttribute());
        }
    }
}