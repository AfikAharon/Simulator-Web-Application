using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
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
            SampleFlightValues(ip, port, true);
            return View();
        }


        [Route("display/{ip}/{port}/{seconds}")]
        public ActionResult display(string ip, int port, int seconds)
        {
            SampleFlightValues(ip, port, false);
            ViewBag.seconds = seconds;
            return View("displaySeconds");
        }


        public void SampleFlightValues(string ip, int port, bool disconnectFlag)
        {
            var flightValues = FlightValues.Instance;
            flightValues.ip = ip;
            flightValues.Port = port;
            flightValues.SampleValues(disconnectFlag);
            ViewBag.Lat = flightValues.Lat;
            ViewBag.Lon = flightValues.Lon;
        }

        [HttpPost]
        public string GetValues()
        {
            var flightValues = FlightValues.Instance;
            flightValues.SampleValues(false);
            return ToXml(flightValues);
        }

        private string ToXml(FlightValues flightValues)
        {
            //Initiate XML stuff
            flightValues.SampleValues(false);
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Values");

            flightValues.ToXml(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

    }
}