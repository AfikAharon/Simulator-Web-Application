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
            string lat = "/position/latitude-deg";
            string lon = "/position/longitude-deg";
            byte[] tempLon = encoding.GetBytes("get " + lon + "\r\n");
            byte[] tempLat = encoding.GetBytes("get " + lat + "\r\n");
            stream.Write(tempLon, 0, tempLon.Length);

            //---read back the text---
            model.Lon = readFromServer(stream);
            stream.Flush();


            stream.Write(tempLat, 0, tempLon.Length);
            //---read back the text---
            model.Lat = readFromServer(stream);
            stream.Flush();
            tcpClient.Close();
        }

        public double readFromServer(NetworkStream stream)
        {
            byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            return 0;
        }
    }
}
