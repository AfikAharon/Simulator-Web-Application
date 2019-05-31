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

        [HttpGet]
        public ActionResult DisplayLine(string ip, int port, int seconds)
        {
            SampleFlightDeatils(ip, port, false);
            ViewBag.seconds = seconds;
            return View("displayLine");
        }
        // Interval function for cshtml DisplayLine
        [HttpPost]
        public string GetValues()
        {
            var infoFlightModel = InfoFlightModel.Instance;
            infoFlightModel.SampleValues(false);
            return ToXml(infoFlightModel);
        }

        [HttpGet]
        public ActionResult SaveFlightDeatils(string ip, int port, int seconds, int timer, string fileName)
        {
            SampleFlightDeatils(ip, port, false);
            InfoFlightModel infoFlightModel = InfoFlightModel.Instance;
            infoFlightModel.FileName = fileName;
            ViewBag.seconds = seconds;
            ViewBag.timer = timer;
            return View("SaveFlightDeatils");
        }
        

        // Interval function for cshtml SaveFlightDeatils
        [HttpPost]
        public string SaveValues()
        {
            var infoFlightModel = InfoFlightModel.Instance;
            infoFlightModel.SampleValues(false);
            infoFlightModel.SaveToFile();
            return ToXml(infoFlightModel);
        }

        [HttpGet]
        public ActionResult DisplayFromFile(string FileName, int seconds)
        {
            var infoFileModel = InfoFileModel.Instance;
            infoFileModel.Read(FileName);
            ViewBag.seconds = seconds;
            return View("FlightDetailsFile");
            
        }

        [HttpPost]
        public string GetValuesFromFile(string param)
        {
            var infoFileModel = InfoFileModel.Instance;
            InfoFlightModel infoFlightModel = InfoFlightModel.Instance;
            string[] splitParams;
            string values = infoFileModel.Get();
            if (values != null)
            {
                splitParams = values.Split(',');
                infoFlightModel.Lon = Convert.ToDouble(splitParams[0]);
                infoFlightModel.Lat = Convert.ToDouble(splitParams[1]);
                infoFlightModel.Throttle = Convert.ToDouble(splitParams[2]);
                infoFlightModel.Rudder = Convert.ToDouble(splitParams[3]);

            }
            return ToXml(infoFlightModel);
            // change to XNL

        }

        public void SampleFlightDeatils(string ip, int port, bool disconnectFlag)
        {
            var infoFlightModel = InfoFlightModel.Instance;
            infoFlightModel.ip = ip;
            infoFlightModel.Port = port;
            infoFlightModel.SampleValues(disconnectFlag);
            ViewBag.Lat = infoFlightModel.Lat;
            ViewBag.Lon = infoFlightModel.Lon;
        }


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