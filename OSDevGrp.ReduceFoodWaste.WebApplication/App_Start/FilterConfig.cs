using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Filters;

namespace OSDevGrp.ReduceFoodWaste.WebApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CultureAttribute());
        }
    }
}