using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    abstract public class Model
    {

        abstract public double Lat
        {
            get; set;
        }

        abstract public double Lon
        {
            get; set;
        }
    }
}