﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Display(string ip="127.0.0.1", int port=5400)
        {
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

        [HttpGet]
        public ActionResult SaveFlightDeatils(string ip, int port, int seconds, int timer, string fileName)
        {
            SampleFlightDeatils(ip, port, false);
            ViewBag.seconds = seconds;
            ViewBag.timer = timer;
            return View("SaveFlightDeatils");
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

        [HttpPost]
        public string GetValues()
        {
            var infoFlightModel = InfoFlightModel.Instance;
            infoFlightModel.SampleValues(false);
            return ToXml(infoFlightModel);
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