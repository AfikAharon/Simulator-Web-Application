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
            SampleValues(ip, port);
            return View();
        }
    

        [Route("display/{ip}/{port}/{seconds}")]
        public ActionResult display(string ip, int port,int seconds)
        {
            SampleValues(ip, port);
            ViewBag.seconds = seconds;
            return View("displaySeconds");
        }


        public string SampleValues(string ip, int port)
        {
            var flightValues = FlightValues.Instance;
            flightValues.SampleValues(ip, port);
                       
            return ToXml(flightValues);
        }

        private string ToXml(FlightValues flightValues)
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