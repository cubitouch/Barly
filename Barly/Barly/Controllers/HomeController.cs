using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Barly.Models;

namespace Barly.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(IList<string> zipcodes)
        {
            var model = new SearchResultModel(zipcodes);

            return View(model);
        }
    }
}