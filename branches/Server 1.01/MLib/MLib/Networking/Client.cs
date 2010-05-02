using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace MLib.Networking
{
    public class Client
    {

        #region Events

        public class MessageArrivedArgs : EventArgs
        {
            private string message;
            private int num;
            public MessageArrivedArgs(string message, int num)
            {
                this.message = message;
                this.num = num;
            }

            public int MessageNumber
            {
                get
                {
                    return num;
                }
            }

            public string Message
            {
                get
                {
                    return message;
                }
            }
        }

        public class ConnectionClosedArgs : EventArgs
        {
            public ConnectionClosedArgs()
            {

            }
        }

        public delegate void MessageArrivedHandler(object myObject, MessageArrivedArgs myArgs);
        public delegate void ConnectionClosedHandler(object myObject, ConnectionClosedArgs myArgs);
        #endregion

        TcpClient client;
        /// <summary>
        /// Stops the connection
        /// </summary>
        public void Stop()
        {
            Stop(false);
        }

        int msgNum = 0;
        /// <summary>
        /// Stops the connection
        /// </summary>
        public void Stop(bool RaiseEvent)
        {
            stopped = true;
            try
            {
                client.Close();
            }
            catch { }

            try
            {
                ConnectionClosedArgs cca = new ConnectionClosedArgs();
                ConnectionClosed(this, cca);
            }
            catch { }
        }

        /// <summary>
        /// Sends a string message to the server
        /// </summary>
        /// <param name="Message">Message</param>
        public void SendToServer(string Message)
        {
            try
            {
                writer.Write(Message);
            }
            catch { Stop(); }
        }

        public List<string> Messages = new List<string>();

        private void Poslusaj()
        {
            while (!stopped)
            {
                string uk = "";
                try
                {
                    uk = reader.ReadString();

                    try
                    {
                        MessageArrivedArgs maa = new MessageArrivedArgs(uk, msgNum);
                        MessageArrived(this, maa);

                    }
                    catch { }

                    Messages.Add(uk);
                    msgNum++;
                }
                catch {Stop(); }
                
            }
        }


        public event MessageArrivedHandler MessageArrived;
        public event ConnectionClosedHandler ConnectionClosed;
        BinaryReader reader;
        BinaryWriter writer;
        bool stopped = false;

        /// <summary>
        /// Creates a new instance of the client and connects it to the server
        /// </summary>
        /// <param name="IP">IP of the server</param>
        /// <param name="port">Port of the server</param>
        /// <param name="Attempts">Number of attempts before stopping</param>
        public Client(string IP, int port, int Attempts)
        {
            Connect(IP, port, Attempts);
        }

        /// <summary>
        /// Connects to a server
        /// </summary>
        /// <param name="IP">IP of the server</param>
        /// <param name="port">Port of the server</param>
        /// <param name="Attempts">Number of attempts before stopping</param>
        public void Connect(string IP, int port, int Attempts)
        {
            if (client.Connected)
            {
                try
                {
                    for (int i = 0; ((i < Attempts) || (Attempts == -1)); i++)
                    {
                        if (stopped)
                            break;

                        try
                        {
                            client = new TcpClient(IP, port);
                            break;
                        }
                        catch { Thread.Sleep(250); }
                    }


                    if (client.Connected)
                    {
                        reader = new BinaryReader(client.GetStream());
                        writer = new BinaryWriter(client.GetStream());

                        Thread thr = new Thread(Poslusaj);
                        thr.IsBackground = true;
                        thr.Start();
                    }

                }
                catch { throw new Exception("Unable to connect to the server."); }
            }
        }


    }
}
