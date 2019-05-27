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

        [HttpGet]
        public ActionResult display(string ip, int port)
        {

            Client client = Client.Instance;
            client.connect(ip, port);
            ViewBag.lon = client.request("Lon");
            ViewBag.lat = client.request("Lat");
            client.disconnect();
            return View();
        }

        [HttpGet]
        public ActionResult save(string ip, int port, int refresh, int seconds, string name)
        {
            var model = new OnesSampleModel(ip, port);
            return View(model);
        }
    }
}