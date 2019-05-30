using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class FlightValues
    {
        private double _Lat;
        private double _Lon;



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


        public void SampleValues(string ip, int port, bool disconnectFlag)
        {
            Utils.Client client = Utils.Client.Instance;
            if (! client.IsConnected)
            {
                client.connect(ip, port);
            }
            Lon = client.request("Lon");
            Lat = client.request("Lat");
            if (disconnectFlag)
            {
                client.disconnect();
            }
        }
        


    }
}