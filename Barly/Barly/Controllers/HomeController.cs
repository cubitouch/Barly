using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Barly.Business;
using Barly.Models;

namespace Barly.Controllers
{
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

        public ActionResult Filter()
        {
            return View();
        }

        public ActionResult Between()
        {
            return View();
        }

        public ActionResult Search(IList<string> zipcodes)
        {
            var model = new SearchResultModel(zipcodes);
            //ViewBag.Filters = model.Filters;
            return View(model);
        }

        public ActionResult SearchFromBetween(double latA, double lngA, double latB, double lngB)
        {
            var model = new SearchResultModel(latA, lngA, latB, lngB);
            //ViewBag.Filters = model.Filters;
            return View("Search", model);
        }

        public ActionResult SearchFromLocation(double latitude, double longitude)
        {
            var model = new SearchResultModel(latitude, longitude);
            //ViewBag.Filters = model.Filters;
            return View("Search", model);
        }

        public ActionResult SearchFromBarId(int id)
        {
            var model = new SearchResultModel(id);
            //ViewBag.Filters = model.Filters;
            return View("Search", model);
        }
        
        public JsonResult GetFoursquareInfos(int id)
        {
            var backOffice = new Business.BackOffice();

            Location location = backOffice.Locations.FirstOrDefault(l => l.Id == id);
            if (string.IsNullOrWhiteSpace(location.FoursquareID))
                return Json(new FoursquareVenue(), JsonRequestBehavior.AllowGet);

            string cacheKey = "foursquare-venue-" + location.FoursquareID;
            var cacheItem = (FoursquareVenue)WebCache.Get(cacheKey);
            if (cacheItem == null)
            {
                FoursquareVenue venue = new FoursquareVenue(location.FoursquareID);
                WebCache.Set(cacheKey, venue, 1440); // one day cache
                return Json(venue, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(cacheItem, JsonRequestBehavior.AllowGet);
            }

        }
    }
}