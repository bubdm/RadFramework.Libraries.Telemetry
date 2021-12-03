using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace RadFramework.Libraries.Telemetry.v2
{
    public class SocketTelemetryListener<TSocketProtocol> : IDisposable where TSocketProtocol : ISocketProtocol, new()
    {
        private TSocketProtocol _protocol = new TSocketProtocol();
        private Socket _listener;
        private Thread _listenerThread = new Thread(ListenInternal);
        private bool shuttingDown = false;
        
        public SocketTelemetryListener(
            IPEndPoint endPoint, 
            IContractSerializer headerSerializer, 
            IContractSerializer packageSerializer)
        {
            _listener = Tcp.CreateListenerSocket(endPoint);
        }

        public void Listen()
        {
            _listenerThread.Start();
        }

        private void ListenInternal()
        {
            while (!shuttingDown)
            {
                
            }
        }

        public void Dispose()
        {
            shuttingDown = true;
            _listener.Dispose();
        }
    }
}