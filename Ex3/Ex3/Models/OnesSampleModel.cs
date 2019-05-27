using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ex3.Utils;

namespace Ex3.Models
{
    public class OnesSampleModel : Model
    {
        private double _lat;
        private double _lon;

        public OnesSampleModel(string ip , int port)
        {
           Client client = Client.Instance;
           //client.setLonLat(this, ip, port);
        }

        public override double Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        public override double Lon
        {
            get { return _lon; }
            set { _lon = value; }
        }

    }
}