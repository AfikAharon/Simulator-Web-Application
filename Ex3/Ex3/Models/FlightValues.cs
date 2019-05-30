using Ex3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class FlightValues
    {
        private double _Lat;
        private double _Lon;
        private string _ip;
        private int _Port;



        #region Singleton
        private static FlightValues _flightValues = null;
        public static FlightValues Instance
        {
            get
            {
                if (_flightValues == null)
                {
                    _flightValues = new FlightValues();
                }
                return _flightValues;
            }
        }
        #endregion
        private FlightValues()
        {
            _Lon = 0;
            _Lat = 0;
        }

        public double Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        public double Lon
        {
            get { return _Lon; }
            set { _Lon = value; }
        }

        public string ip
        {
            get { return ip; }
            set { _ip = value; }
        }

        public int Port
        {
            get { return _Port; }
            set { _Port = value; }
        }


        public void SampleValues(bool disconnectFlag)
        {

            Client client = Client.Instance;
            if (! client.IsConnected || disconnectFlag)
            client.connect(_ip, _Port);
            Lon = client.request("Lon");
            Lat = client.request("Lat");
            if (disconnectFlag)
            {
                client.disconnect();
            }
        }

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("FlightValues");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteEndElement();
        }



    }
}