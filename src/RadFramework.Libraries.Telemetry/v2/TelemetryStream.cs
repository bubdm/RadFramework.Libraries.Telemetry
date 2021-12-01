using System.IO;
using System.Net;
using System.Net.Sockets;

namespace RadFramework.Libraries.Telemetry.v2
{
    public class TelemetryStream
    {
        public Stream Stream { get; private set; }
        
        public TelemetryStream(EndPoint endPoint)
        {
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);
            Stream = new NetworkStream(socket);
        }

        public TelemetryStream(Socket socket)
        {
            
        }
    }
}