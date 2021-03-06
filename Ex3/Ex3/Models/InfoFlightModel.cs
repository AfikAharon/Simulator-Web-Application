﻿using Ex3.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class InfoFlightModel
    {
        private double _Lat;
        private double _Lon;
        private string _ip;
        private int _Port;
        private double _throttle;
        private double _rudder;


        /*
         * Sets one Instance of InfoFlightModel
         */
        #region Singleton
        private static InfoFlightModel _flightValues = null;
        public static InfoFlightModel Instance
        {
            get
            {
                if (_flightValues == null)
                {
                    _flightValues = new InfoFlightModel();

                }
                return _flightValues;
            }
        }
        #endregion
        private InfoFlightModel()
        {
            _Lat = 0;
            _Lon = 0;
            _ip = "127.0.0.1";
            _Port = 5400;
            _throttle = 0;
            _rudder = 0;
        }

        /*
         * Lat property, returns and sets the value
         */
        public double Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        /*
         * Lon property, returns and sets the value
         */
        public double Lon
        {
            get { return _Lon; }
            set { _Lon = value; }
        }

        /*
         * Throttle property, returns and sets the value
         */
        public double Throttle
        {
            get { return _throttle; }
            set { _throttle = value; }
        }

        /*
         * Rudder property, returns and sets the value
         */
        public double Rudder
        {
            get { return _rudder; }
            set { _rudder = value; }
        }

        /*
         * ip property, returns and sets the value
         */
        public string ip
        {
            get { return ip; }
            set { _ip = value; }
        }

        /*
         * Port property, returns and sets the value
         */
        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }

        /*
         * The function gets the values from the simulator
         */
        public void SampleValues(bool disconnectFlag)
        {

            Client client = Client.Instance;
            if (!client.IsConnected || disconnectFlag)
            {
                client.connect(_ip, _Port);
            }
            Lon = client.request("/position/longitude-deg");
            Lat = client.request("/position/latitude-deg");
            Rudder = client.request("/controls/flight/rudder");
            Throttle = client.request("/controls/engines/current-engine/throttle");
            if (disconnectFlag)
            {
                client.disconnect();
            }
        }

        /*
         * The function writes to XML file the values
         */
        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("FlightValues");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Rudder", this._rudder.ToString());
            writer.WriteElementString("Throttle", this._throttle.ToString());
            writer.WriteEndElement();
        }

        /*The function split the given string,cast the splits values to double in insert to the property*/
        public void CastValues(string values)
        {
            string[] splitFlightDeatils = values.Split(',');
            Lon = Convert.ToDouble(splitFlightDeatils[0]);
            Lat = Convert.ToDouble(splitFlightDeatils[1]);
            Throttle = Convert.ToDouble(splitFlightDeatils[2]);
            Rudder = Convert.ToDouble(splitFlightDeatils[3]);
        }
    }
}