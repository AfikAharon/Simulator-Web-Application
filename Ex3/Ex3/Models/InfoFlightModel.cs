using Ex3.Utils;
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

        public double Throttle
        {
            get { return _throttle; }
            set { _throttle = value; }
        }

        public double Rudder
        {
            get { return _rudder; }
            set { _rudder = value; }
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

        public void ToXml(XmlWriter writer)
        {
            writer.WriteStartElement("FlightValues");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Rudder", this._rudder.ToString());
            writer.WriteElementString("Throttle", this._throttle.ToString());
            writer.WriteEndElement();
        }

        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";

        public void SaveToFile(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, fileName));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false))
            {
                file.WriteLine(Lon.ToString() + "," + Lat.ToString() + ","  + Throttle.ToString() +","+ Rudder.ToString() );
            }
        }
    }
}