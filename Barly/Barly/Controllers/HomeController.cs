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

        public JsonResult GetExternalInfos(int id)
        {
            var backOffice = new Business.BackOffice();
            Location location = backOffice.Locations.FirstOrDefault(l => l.Id == id);

            FoursquareVenue cacheItemFoursquare = null;
            if (!string.IsNullOrWhiteSpace(location.FoursquareID))
            {
                string cacheKeyFoursquare = "foursquare-venue-" + location.FoursquareID;
                cacheItemFoursquare = (FoursquareVenue)WebCache.Get(cacheKeyFoursquare);
                if (cacheItemFoursquare == null)
                {
                    cacheItemFoursquare = new FoursquareVenue(location.FoursquareID);
                    WebCache.Set(cacheKeyFoursquare, cacheItemFoursquare, 1440); // one day cache
                }
            }

            GoogleNearby cacheItemGoogle = null;
            string cacheKeyGoogle = "google-nearby-" + location.Id;
            cacheItemGoogle = (GoogleNearby)WebCache.Get(cacheKeyGoogle);
            if (cacheItemGoogle == null)
            {
                cacheItemGoogle = new GoogleNearby(location.Latitude,location.Longitude);
                WebCache.Set(cacheKeyGoogle, cacheItemGoogle, 1440); // one day cache
            }

            return Json(new LocationExternal(cacheItemFoursquare, cacheItemGoogle), JsonRequestBehavior.AllowGet);
        }
    }
}