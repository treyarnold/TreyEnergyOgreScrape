using System.Web.Mvc;
using System.Collections.Generic;
using System;
using EO.Web.Models;
using System.Threading.Tasks;

namespace EO.Web.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomeModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(HomeModel model)
        {
            // accept Zip Code
            var GetRates = Task.Factory.StartNew(() => {
                model.GetRates();
                return View(model);
            });


            // send Zip Code to REP web site, get back List<Rates>
            //var rates = new List<Rates>();
            // output results

            return View(model);
        }

       
    }
}