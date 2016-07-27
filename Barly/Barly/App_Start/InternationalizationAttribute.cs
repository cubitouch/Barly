using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Barly.App_Start
{
    public class InternationalizationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-FR");

        }
    }
}
