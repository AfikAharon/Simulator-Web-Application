using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class InfoFileModel
    {
        private int _counter;
        private int _size;
        private string[] _information;


        #region Singleton
        private static InfoFileModel _infoFileModel = null;
        public static InfoFileModel Instance
        {
            get
            {
                if (_infoFileModel == null)
                {
                    _infoFileModel = new InfoFileModel();
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


        public void Read(string fileName)
        {
            // Read the file and init
            Counter = 0;
        }


        public string Get()
        {
            if(Counter > Size - 1)
            {
                /// throw Exception
            }
            string retValue = Information[Counter];
            Counter++;
            return retValue;
        }
    }
}