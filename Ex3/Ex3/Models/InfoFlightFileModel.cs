using Ex3.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Ex3.Models
{
    public class InfoFlightFileModel
    {
        private double _Lat;
        private double _Lon;
        private double _throttle;
        private double _rudder;
        private double _counter;
        private string[] _data;


        #region Singleton
        private static InfoFlightFileModel infoFlightFileModel = null;
        public static InfoFlightFileModel Instance
        {
            get
            {
                if (_flightValues == null)
                {
                    _flightValues = new InfoFlightFileModel();
                }
                return _flightValues;
            }
        }
        #endregion
        private InfoFlightModel()
        {
            _Lat = 0;
            _Lon = 0;
            _throttle = 0;
            _rudder = 0;
            _counter = 0;
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

        public double Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        public void Read(string FileName)
        {
            _data = System.IO.File.ReadAllLines(FileName);
        }

    }
}