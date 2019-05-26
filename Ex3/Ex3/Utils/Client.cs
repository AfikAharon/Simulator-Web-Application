using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using Ex3.Models;

namespace Ex3.Utils
{
    //<img src="~/Map/export-map-share.png" style="position:absolute; width:100%;height:100%; top:0px; left:0px"/>
    class Client
    {
        private TcpClient tcpClient;
        private volatile Thread _currentThread;

        #region Singleton
        private static Client c_Instance = null;
        public static Client Instance
        {
            get
            {
                if (c_Instance == null)
                {
                    c_Instance = new Client();
                }
                return c_Instance;
            }
        }
        #endregion
        /*
         * Constructs a new Client
         */
        private Client()
        {
            tcpClient = new TcpClient();
            _currentThread = null;
        }

        /*
         * IsConnected property, returns the value
         */
        public bool IsConnected
        {
            get { return tcpClient.Connected; }
        }

   

        /*
         * GetCurrentThread property, returns the value , and sets value
         */
        public Thread GetCurrentThread
        {
            get { return _currentThread; }
            set { this._currentThread = value; }
        }

        /*
         * The function reads the commans given to him and then writes them, then sleeps for 2 seconds
         */
         
         
        public void setLonLat(Model model, string ip, int port)
        {

            tcpClient.Connect(ip, port);
            NetworkStream stream = tcpClient.GetStream();
            ASCIIEncoding encoding = new ASCIIEncoding();
            int delete;
           
            string lat = "/position/latitude-deg";
            string lon = "/position/longitude-deg";
            byte[] tempLon = encoding.GetBytes("get " + lon + "\r\n");
            byte[] tempLat = encoding.GetBytes("get " + lat + "\r\n");
            stream.Write(tempLon, 0, tempLon.Length);
            delete = lon.Length;
            //---read back the text---
            model.Lon = readFromServer(stream, delete);
            double lon1 = model.Lon;
            stream.Flush();


            stream.Write(tempLat, 0, tempLat.Length);
            //---read back the text---
            delete = lat.Length;
            model.Lat = readFromServer(stream,delete);
            stream.Flush();
            tcpClient.Close();
        }
        

        public double readFromServer(NetworkStream stream, int delete)
        {
            byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            string temp =Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            double number = extractNumber(temp, delete);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            return number;
        }

        public double extractNumber(string givenString, int delete)
        {
            int len = givenString.Length;
            string temp = givenString.Substring(delete + 4);
            string number = "";
            double result;
            for (int i =0; i<temp.Length; i++)
            {
                char c = temp[i];
                if ((c >= '0' && c <= '9') || c == '-'|| c == '.')
                {
                    number += c;
                }
            }
            result = Convert.ToDouble(number);
            return result;
        }
    }
}
