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

        /*
         * The function operation is setting new TcpClient and connecting
         */ 
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
         * The function gets the information from simulator given the request string
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

        /*
         * The function reads from the given strean the values, and  uses the extractNumber to get the actual
         * value without the string
         */ 
        public double readFromServer(NetworkStream stream)
        {

            byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            string temp = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            if(temp == "")
            {
                throw new Exception("No data could be read");
            }
            double number = extractNumber(temp);
            return number;
        }

        /*
         * The function extracts the desried number from the string and converts to double
         */ 
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

        /*
         * Locks the mutex
         */ 
        public void Lock() {
            // put try and catch 
            try
            {
                mut.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /*
         * Unblocks the mutex
         */ 
        public void Unlock()
        {
            try
            {
                mut.ReleaseMutex();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
