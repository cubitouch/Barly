using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Barly.App_Start;
using Barly.Business;
using Barly.Models;

namespace Barly.Controllers
{
    //[Internationalization]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //    if (ClaimsPrincipal.Current.FindFirst("email") != null)
            //    {
            //        var test = ClaimsPrincipal.Current.FindFirst("email").Value;
            //    }
            return View();
        }

        public ActionResult Filter(IList<string> zipcodes, string open)
        {
            var model = new FilterEditModel(zipcodes, open);
            return View(model);
        }

        public ActionResult Between()
        {
            return View();
        }

        public ActionResult Search(IList<string> zipcodes, string open)
        {
            var model = new SearchResultModel(new FilterEditModel(zipcodes, open));
            return View(model);
        }

        public ActionResult SearchFromBetween(double latA, double lngA, double latB, double lngB, string open)
        {
            var model = new SearchResultModel(new FilterEditModel(latA, lngA, latB, lngB, open));
            return View("Search", model);
        }

        public ActionResult SearchFromLocation(double latitude, double longitude, string open)
        {
            var model = new SearchResultModel(new FilterEditModel(latitude, longitude, open));
            return View("Search", model);
        }

        public ActionResult SearchFromBarId(int id)
        {
            var model = new SearchResultModel(id);
            return View("Search", model);
        }


        public ActionResult Tutorial()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
    }
}