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
            Client client = Client.Instance;
            client.connect(ip, port);
            ViewBag.lon = client.request("Lon");
            ViewBag.lat = client.request("Lat");
            client.disconnect();
            return View();
        }
        [Route("display/{ip}/{port}/{seconds}")]
        public ActionResult display(string ip, int port,int seconds)
        {
            Client client = Client.Instance;
            client.connect(ip, port);
            ViewBag.lon = client.request("Lon");
            ViewBag.lat = client.request("Lat");
            ViewBag.sec = seconds;
            client.disconnect();
            return View("displaySeconds");
        }



    }
}