using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using MessagePack;

namespace RadFramework.Libraries.Telemetry.v2
{
    public static class Tcp
    {
        public static Socket CreateListenerSocket(IPEndPoint endPoint)
        {
            Socket listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            
            listener.Bind(endPoint);
            
            listener.Listen();

            return listener;
        }

        public static Socket CreateClientSocket(IPEndPoint endPoint)
        {
            Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            
            client.Connect(endPoint);

            return client;
        }
    }
}