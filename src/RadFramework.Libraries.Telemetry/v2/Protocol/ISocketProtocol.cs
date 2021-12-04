using System.Net;
using System.Net.Sockets;

namespace RadFramework.Libraries.Telemetry.v2
{
    public interface ISocketProtocol
    {
        Socket CreateListenerSocket(IPEndPoint endPoint);
        
    }
}