using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Ex3.Models;

namespace Ex3.Controllers
{
    public class MainController : Controller
    {

        /*
         * The default view that is generated at the start of application
         */
        [HttpGet]
        public ActionResult DefaultView()
        {
            return View("DefaultView");
        }

        /*
         * The ActionResult directs to the "display" view which shows the planes current location on the map, 
         * as it samples once from the simulator
         */
        [HttpGet]
        public ActionResult Display(string ip, int port)
        {
            IPAddress adress;
            try
            {
                adress = IPAddress.Parse(ip);
            }
            catch
            {
                return RedirectToAction("DisplayFromFile", new { FileName = ip, seconds = port });
            }
            SampleFlightDeatils(ip, port, true);
            return View();
        }

        /*
         * The ActionResults directs to the "displayLine" view, which samples the planes location each given
         * number of seconds and shows its location on the map. also shows with a line the route which
         * the plane flew through
         */
        [HttpGet]
        public ActionResult DisplayLine(string ip, int port, int seconds)
        {
            SampleFlightDeatils(ip, port, false);
            ViewBag.seconds = seconds;
            return View("displayLine");
        }
        /*
         * The function gets the plane values from the simulator to the infoFlightModel instance
         */ 
        [HttpPost]
        public string GetValues()
        {
            var infoFlightModel = InfoFlightModel.Instance;
            infoFlightModel.SampleValues(false);
            return ToXml(infoFlightModel);
        }

        /*
         * The ActionResults directs to the "SaveFlightDetails" view, which samples the plane location each 
         * number of seconds, for a period of time (timer), and saves the route of the flight. also displays the plane
         */ 
        [HttpGet]
        public ActionResult SaveFlightDeatils(string ip, int port, int seconds, int timer, string fileName)
        {
            SampleFlightDeatils(ip, port, false);
            InfoFileModel infoFileModel = InfoFileModel.Instance;
            infoFileModel.FileName = fileName;
            ViewBag.seconds = seconds;
            ViewBag.timer = timer;
            return View("SaveFlightDeatils");
        }
        

        /*
         * The function saves to file the samples of plane location
         */ 
        [HttpPost]
        public string SaveValues()
        {
            var infoFlightModel = InfoFlightModel.Instance;
            infoFlightModel.SampleValues(false);
            InfoFileModel.Instance.SaveToFile();
            return ToXml(infoFlightModel);
        }

        /*
         * The ActionResults directs to "FlightDetailsFile" view, which displays the plane route on the map
         * given his coordinates from given file name, each given number of seconds
         */
        [HttpGet]
        public ActionResult DisplayFromFile(string FileName, int seconds)
        {
            var infoFileModel = InfoFileModel.Instance;
            infoFileModel.FileName = FileName;
            infoFileModel.Read();
            InfoFlightModel infoFlightModel = InfoFlightModel.Instance;
            string values = infoFileModel.Get();
            if (values != null)
            {
                infoFlightModel.CastValues(values);

            }
            
            ViewBag.Lon = infoFlightModel.Lon;
            ViewBag.Lat = infoFlightModel.Lat;
            ViewBag.seconds = seconds;
            ViewBag.numSamples = infoFileModel.Size;
            return View("FlightDetailsFile");
        }

        /*
         * The function gets the values of the plane from a given file and returns xml string
         */ 
        [HttpPost]
        public string GetValuesFromFile()
        {
            var infoFileModel = InfoFileModel.Instance;
            InfoFlightModel infoFlightModel = InfoFlightModel.Instance;
            string values = infoFileModel.Get();
            if (values != null)
            {
                infoFlightModel.CastValues(values);

            }
            return ToXml(infoFlightModel);
        }

        /*
         * The function samples the flight details from the simulator
         */ 
        public void SampleFlightDeatils(string ip, int port, bool disconnectFlag)
        {
            var infoFlightModel = InfoFlightModel.Instance;
            infoFlightModel.ip = ip;
            infoFlightModel.Port = port;
            infoFlightModel.SampleValues(disconnectFlag);
            ViewBag.Lat = infoFlightModel.Lat;
            ViewBag.Lon = infoFlightModel.Lon;
        }

        /*
         * The function turns the flight samples to xml
         */ 
        private string ToXml(InfoFlightModel flightValues)
        {
            //Initiate XML stuff
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