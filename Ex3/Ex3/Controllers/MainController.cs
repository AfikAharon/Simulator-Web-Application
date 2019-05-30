using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ex3.Models;
using Ex3.Utils;

namespace Ex3.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        [Route("display/{ip}/{port}")]
        public ActionResult display(string ip, int port)
        {
            SampleValues(ip, port, true);
            return View();
        }


        [Route("display/{ip}/{port}/{seconds}")]
        public ActionResult display(string ip, int port,int seconds)
        {
            SampleValues(ip, port, false);
            ViewBag.seconds = seconds;
            return View("displaySeconds");
        }


        public void SampleValues(string ip, int port, bool flag)
        {
            FlightValues flightValues = FlightValues.Instance;
            flightValues.SampleValues(ip, port, flag);
            ViewBag.Lat = flightValues.Lat;
            ViewBag.Lon = flightValues.Lon;
        }
    }
}