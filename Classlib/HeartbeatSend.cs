using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Classlib
{
    public class HeartbeatSend
    {
        private volatile bool _shouldStop;
        public volatile string message = "default message";
        public volatile int waitinms = 500;
        public void DoWork()
        {
            while (!_shouldStop)
            {
                SendUDPMulticastMessage(message);
                Thread.Sleep(waitinms);
            }
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
        int port;
        public HeartbeatSend(int sendport)
        {
            port = sendport;
        }
        public void SendUDPMulticastMessage(string nachricht)
        {
            Socket mSocket1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPEndPoint mIPEP = new IPEndPoint(IPAddress.Parse("224.100.0.1"), port); //Multicast sendet an alle Mitglieder der Gruppe
            System.Threading.Thread.Sleep(3000);
            byte[] mSendBuffer = Encoding.ASCII.GetBytes(string.Format(nachricht));

            //SocketOption sind damit da, dass man über die Router kommt, Default-Wert von TTL wäre 1, wir setzen ihn auf 50
            mSocket1.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse("224.100.0.1")));
            mSocket1.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 50);
            mSocket1.SendTo(mSendBuffer, mIPEP);

            mSocket1.Close();
        }
    }
}
