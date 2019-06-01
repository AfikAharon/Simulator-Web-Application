using System;
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

        public int Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string[] Information
        {
            get { return _information; }
            set {
                _information = value;
                Size = Information.Length;
            }
        }

       


        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
            }
        }

        public string Prev
        {
            get { return _prev; }
            set { _prev = value; }
        }

        public void Read()
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, FileName));
            _information = System.IO.File.ReadAllLines(path);
            Counter = 0;
        }


        public string Get()
        {
            if (Counter > Size - 1)
            {
                if (Prev == null)
                {
                    //throw exception
                }
                return Prev;
            }
            string retValue = Information[Counter];
            _prev = retValue;
            Counter++;
            return retValue;
        }

        public void SaveToFile()
        {
            if (FileName != null)
            {
                // put try and catch
                mut.WaitOne();
                string path = HttpContext.Current.Server.MapPath(String.Format(SCENARIO_FILE, FileName));
                InfoFlightModel flightModel = InfoFlightModel.Instance;
                System.IO.StreamWriter file = new System.IO.StreamWriter(path, true);
                file.WriteLine(flightModel.Lon.ToString() + "," + flightModel.Lat.ToString() + "," + flightModel.Throttle.ToString() + "," + flightModel.Rudder.ToString());
                file.Close();
                mut.ReleaseMutex();
            }

        }
    }
}
