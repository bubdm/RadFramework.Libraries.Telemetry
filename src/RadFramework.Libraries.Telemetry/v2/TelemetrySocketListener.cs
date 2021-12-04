using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RadFramework.Libraries.Telemetry.v2.Packages;
using RadFramework.Libraries.Telemetry.v2.PackageWriters;

namespace RadFramework.Libraries.Telemetry.v2
{
    public class TelemetrySocketListener<TSocketProtocol> : IDisposable where TSocketProtocol : ISocketProtocol, new()
    {
        private TSocketProtocol _protocol = new TSocketProtocol();
        private Socket _listener;
        private Thread _listenerThread;
        private bool shuttingDown = false;
        private Type _handshakeHeaderType = typeof(HeaderBase);
        private readonly ContractSerializer _defaultSerializer = new ContractSerializer();
        private PackageSerializer<HeaderBase> _handshakeSerializer;
        private Dictionary<Guid, TelemetryConnection> _connections = new Dictionary<Guid, TelemetryConnection>();

        public TelemetrySocketListener(IPEndPoint endPoint)
        {
            ISocketProtocol socketProtocol = new TSocketProtocol();
            _listener = socketProtocol.CreateListenerSocket(endPoint);

            _handshakeSerializer = new PackageSerializer<HeaderBase>(_defaultSerializer, _defaultSerializer);
        }

        public void Listen()
        {
            _listenerThread.Start();
        }

        private void ListenInternal()
        {
            while (!shuttingDown)
            {
                try
                {
                    AcceptConnection();
                }
                catch { }
            }
        }

        private void AcceptConnection()
        {
            Socket client = _listener.Accept();

            Stream clientStream = new NetworkStream(client);

            object package = PackageStreamUtil.ReadPackage(clientStream, _defaultSerializer);

            if (package is InitMultiplexConnection initMultiplexConnection)
            {
                InitMultiplexConnection(initMultiplexConnection, clientStream);
            }
            else if (package is InitMultiplexStream initMultiplexStream)
            {
                InitMultiplexStream(initMultiplexStream, clientStream);
            }
        }

        private void InitMultiplexConnection(InitMultiplexConnection package, Stream stream)
        {
            InitMultiplexConnectionResponse response = new InitMultiplexConnectionResponse();
            
            response.ClientId = Guid.NewGuid();
            
            if (package.CpuCores > Environment.ProcessorCount)
            {
                response.CpuCores = Environment.ProcessorCount;
            }
            else
            {
                response.CpuCores = package.CpuCores;
            }
            
            PackageStreamUtil.WritePackage(stream, _handshakeSerializer.Pack(response));
        }
        
        private void InitMultiplexStream(InitMultiplexStream package, Stream stream)
        {
            
        }

        public void Dispose()
        {
            shuttingDown = true;
            _listener.Dispose();
        }
    }
}