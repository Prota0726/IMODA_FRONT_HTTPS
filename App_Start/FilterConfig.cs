using System.Web;
using System.Web.Mvc;

namespace IMODA_FRONT_HTTPS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
