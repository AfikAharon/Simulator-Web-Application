﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;


namespace Ex3.Models
{
    public class InfoFileModel
    {
        public const string SCENARIO_FILE = "~/App_Data/{0}.txt";

        private int _counter;
        private int _size;
        private string[] _information;
        private static Mutex mut;
        private string _fileName;
        private string _prev;


        /*
         * Sets one Instance of InfoFileModel
         */ 
        #region Singleton
        private static InfoFileModel _infoFileModel = null;
        public static InfoFileModel Instance
        {
            get
            {
                if (_infoFileModel == null)
                {
                    _infoFileModel = new InfoFileModel();
                    mut = new Mutex();
                }
                return _infoFileModel;
            }
        }
        #endregion
        
        private InfoFileModel()
        {
            _counter = 0;
            _information = null;
            _size = 0;
            _fileName = null;
            _prev = null;
        }

        /*
         * Counter property, returns and sets the value
         */ 
        public int Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        /*
         * Size property, returns and sets the value
         */
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /*
         * Information property, returns and sets the value
         */
        public string[] Information
        {
            get { return _information; }
            set {
                _information = value;
                Size = Information.Length;
            }
        }

        /*
         * InformationLength property, returns length of _information
         */
        public int InformationLength
        {
            get { return _information.Length; }
        }

        /*
         * FileName property, returns and sets the value
         */
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
            }
        }

        /*
         * Prev property, returns and sets the value
         */
        public string Prev
        {
            get { return _prev; }
            set { _prev = value; }
        }

        /*
         *The function reads all data of planes location to the array 
         */
        public void Read()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, FileName));
            _information = System.IO.File.ReadAllLines(path);
            Counter = 0;
            Size = _information.Length;
        }

        /*
         * The function gets the location from the string array
         */ 
        public string Get()
        {
            if (Counter > Size - 1)
            {        
                return Prev;
            }
            string retValue = Information[Counter];
            Prev = retValue;
            Counter++;
            return retValue;
        }

        /*
         * The function saves to file the samples
         */ 
        public void SaveToFile()
        {
            if (FileName != null)
            {
                try
                {
                    // put try and catch
                    mut.WaitOne();
                    string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, FileName));
                    InfoFlightModel flightModel = InfoFlightModel.Instance;
                    System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
                    file.WriteLine(flightModel.Lon.ToString() + "," + flightModel.Lat.ToString() + "," + flightModel.Throttle.ToString() + "," + flightModel.Rudder.ToString());
                    file.Close();
                    mut.ReleaseMutex();
                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
            }

        }
    }
}
