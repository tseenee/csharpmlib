using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace MLib.Networking
{
    /// <summary>
    /// Simple networking server
    /// </summary>
    public class Server
    {
        #region Events

        #region Event classes
        public class MessageArrivedArgs : EventArgs
        {
            private string message;
            private int num;
            private ConnectedClient cl;
            public MessageArrivedArgs(string message, int num, ConnectedClient cl)
            {
                this.message = message;
                this.num = num;
                this.cl = cl;
            }

            public ConnectedClient Client
            {
                get
                {
                    return cl;
                }
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
            private ConnectedClient cl;
            public ConnectedClient Client
            {
                get
                {
                    return cl;
                }
            }
            public ConnectionClosedArgs(ConnectedClient cl)
            {
                this.cl = cl;
            }
        }

        public class ClientConnectedArgs : EventArgs
        {
            private int num;
            
            public int ClientNumber
            {
                get
                {
                    return num;
                }
            }
            
            public ClientConnectedArgs(int num)
            {
                this.num = num;
                
            }
        }
        #endregion

        public delegate void MessageArrivedHandler(object myObject, MessageArrivedArgs myArgs);
        public delegate void ConnectionClosedHandler(object myObject, ConnectionClosedArgs myArgs);
        public delegate void ClientConnectedHandler(object myObject, ClientConnectedArgs myArgs);
        #endregion

        public event MessageArrivedHandler MessageArrived;
        public event ConnectionClosedHandler ConnectionClosed;
        public event ClientConnectedHandler ClientConnected;

        int maxClients = 0;
        int client_num = 0;
        int port = 2000;
        bool paused = false;
        bool stopped = false;
        TcpListener listener;
        List<ConnectedClient> clients;

        /// <summary>
        /// Simple networking server
        /// </summary>
        /// <param name="Port">Server listening port</param>
        /// <param name="MaxClients">Maximum allowed clients</param>
        public Server(int Port, int MaxClients)
        {
            port = Port;
            maxClients = MaxClients;

            listener = new TcpListener(IPAddress.Any, Port);
            clients = new List<ConnectedClient>(); 

            Thread listen = new Thread(Poslusanje);
            listen.IsBackground = true;
            listen.Start();
        }


        /// <summary>
        /// Disconects the given client
        /// </summary>
        /// <param name="cl">Client</param>
        public void StopClient(ConnectedClient cl)
        {
            try
            {
                cl.client.Close();
            }
            catch { }

            try
            {
                ConnectionClosedArgs cca = new ConnectionClosedArgs(cl);
                ConnectionClosed(this, cca);
            }
            catch { }
        }


        bool accepted = true;
        private void Poslusaj()
        {
            ConnectedClient cl = clients[clients.Count - 1];
            accepted = true;

            while ((!stopped) && cl.client.Connected)
            {
                string uk = "";
                try
                {
                    uk = cl.reader.ReadString();

                    try
                    {
                        MessageArrivedArgs maa = new MessageArrivedArgs(uk, cl.msgNum, cl);
                        MessageArrived(this, maa);
                    }
                    catch { }
                    cl.Messages.Add(uk);
                    cl.msgNum++;
                }
                catch
                {
                    StopClient(cl);
                }
            }
        }

        //Test comment

        /// <summary>
        /// Sends data to a client
        /// </summary>
        /// <param name="cl">Client</param>
        /// <param name="Message">Message string</param>
        public void SendToClient(ConnectedClient cl, string Message)
        {
            try
            {
                cl.writer.Write(Message);
            }
            catch { StopClient(cl); }
        }


        void Poslusanje()
        {
            listener.Start();
            while (!stopped)
            {
                if ((paused) || (client_num >= maxClients))
                    Thread.Sleep(30);


                TcpClient client = listener.AcceptTcpClient();

                ClientConnectedArgs cca = new ClientConnectedArgs(client_num);
                ClientConnected(this, cca);

                client_num++;

                
                ConnectedClient cl = new ConnectedClient(client);
                clients.Add(cl);
                accepted = false;
                Thread thr = new Thread(Poslusaj);
                thr.IsBackground = true;
                thr.Start();


                while (!accepted)
                    Thread.Sleep(5);

            }
        }

        public class ConnectedClient
        {
            public TcpClient client;
            public BinaryReader reader;
            public BinaryWriter writer;
            public int msgNum = 0;
            public List<String> Messages;
            public ConnectedClient(TcpClient client)
            {
                this.client = client;
                reader = new BinaryReader(client.GetStream());
                writer = new BinaryWriter(client.GetStream());
                Messages = new List<string>();
            }
        }

    }
}
