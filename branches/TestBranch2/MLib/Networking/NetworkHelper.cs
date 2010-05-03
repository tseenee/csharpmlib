using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;

namespace MLib.Networking
{
    class NetworkHelper
    {
        public bool PingHost(string Host)
        {
            Ping pingObj = new Ping();
            string data = "pinging";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            PingReply pReply = pingObj.Send(Host, 10000, buffer);

            if (pReply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }

    }
}
