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
        private static Mutex mut;

        #region Singleton
        private static Client c_Instance = null;
        public static Client Instance
        {
            get
            {
                if (c_Instance == null)
                {
                    c_Instance = new Client();
                    mut = new Mutex();
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
            _currentThread = null;
            tcpClient = new TcpClient();
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


        public void connect(string ip, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
        }

        public void disconnect()
        {
            tcpClient.Close();
        }

        /*
         * The function reads the commans given to him and then writes them, then sleeps for 2 seconds
         */
        public double request(string reqComm)
        {
            NetworkStream stream = tcpClient.GetStream();
            ASCIIEncoding encoding = new ASCIIEncoding();

            Lock();
            byte[] concatenationReq = encoding.GetBytes("get " + reqComm + "\r\n");
            stream.Write(concatenationReq, 0, concatenationReq.Length);
            //---read back the text---
            double returnValue = readFromServer(stream);
            stream.Flush();
            Unlock();
            return returnValue;
        }

        public double readFromServer(NetworkStream stream)
        {

            byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            string temp = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            double number = extractNumber(temp);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
            return number;
        }

        public double extractNumber(string givenString)
        {
            int len = givenString.Length;
            string[] splits = givenString.Split('=');
            string temp = splits[1];
            string number = "";
            double result;
            for (int i = 0; i < temp.Length; i++)
            {
                char c = temp[i];
                if ((c >= '0' && c <= '9') || c == '-' || c == '.')
                {
                    number += c;
                }
            }
            result = Convert.ToDouble(number);
            return result;
        }

        public void Lock() {
            mut.WaitOne();

        }

        public void Unlock()
        {
            mut.ReleaseMutex();
        }
    }
}
