using System;
using System.IO;
using MessagePack;

namespace RadFramework.Libraries.Telemetry.v2
{
    public class PackageSerializer<THeader> where THeader : HeaderBase, new()
    {
        private readonly Type _headerType = typeof(THeader);
        private readonly IContractSerializer _headerSerializer;
        private readonly IContractSerializer _packageSerializer;

        public PackageSerializer(
            IContractSerializer headerSerializer,
            IContractSerializer packageSerializer)
        {
            _headerSerializer = headerSerializer;
            _packageSerializer = packageSerializer;
        }

        public SerializedPackage Pack(object package)
        {
            return Pack(new THeader(), package);
        }

        public SerializedPackage Pack(THeader header, object package)
        {
            byte[] serializedHeader = _headerSerializer.Serialize(_headerType, header);
            
            byte[] serializedHeaderSize = new byte[sizeof(int)];

            WriteInt32(ref serializedHeaderSize, serializedHeader.Length);

            SerializedPackage serializedPackage = new SerializedPackage();
            
            serializedPackage.HeaderSize = serializedHeaderSize;

            serializedPackage.Header = serializedHeader;

            serializedPackage.Package = _packageSerializer.Serialize(header.PayloadType, package);
            
            return serializedPackage;
        }

        public object Unpack(SerializedPackage package)
        {
            THeader header = (THeader)_headerSerializer.Deserialize(_headerType, package.Header);
            
            return _packageSerializer.Deserialize(header.PayloadType, package.Package);
        }
        
        private static unsafe void WriteInt32(ref byte[] bytes, int value)
        {
            fixed (byte* numPtr = bytes)
                *(int*) (numPtr) = value;
        }
    }
}