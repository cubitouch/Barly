using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
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

        public ActionResult Search(IList<string> zipcodes)
        {
            var model = new SearchResultModel(zipcodes);
            ViewBag.Filters = model.Filters;
            return View(model);
        }

        public ActionResult SearchFromLocation(double latitude, double longitude)
        {
            var model = new SearchResultModel(latitude, longitude);
            ViewBag.Filters = model.Filters;
            return View("Search", model);
        }
    }
}