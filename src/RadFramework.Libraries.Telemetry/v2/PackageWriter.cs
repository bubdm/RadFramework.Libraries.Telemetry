using System;
using System.IO;
using MessagePack;

namespace RadFramework.Libraries.Telemetry.v2
{
    public class PackageWriter<THeader> where THeader : HeaderBase
    {
        private Type _headerType = typeof(THeader);
        private readonly IContractSerializer _headerSerializer;
        private readonly IContractSerializer _packageSerializer;

        public PackageWriter(
            IContractSerializer headerSerializer,
            IContractSerializer packageSerializer)
        {
            _headerSerializer = headerSerializer;
            _packageSerializer = packageSerializer;
        }
        
        public void WritePackage(Stream stream, Type contract, THeader header, object package)
        {
            byte[] serializedPackage = _packageSerializer.Serialize(contract, package);
            
            header.PayloadSize = serializedPackage.Length;
            header.PayloadType = contract.AssemblyQualifiedName;
            
            byte[] serializedHeader = _headerSerializer.Serialize(_headerType, header);

            byte[] serializedHeaderSize = new byte[sizeof(int)];

            WriteInt32(ref serializedHeaderSize, 0, serializedHeader.Length);

            stream.Write(serializedHeaderSize);
            stream.Write(serializedHeader);
            stream.Write(serializedPackage);
            stream.Flush();
        }

        public object ReadPackage(Stream stream, object package)
        {
            byte[] headerSizeBuffer = new byte[sizeof(int)];
            
            stream.Read(headerSizeBuffer, 0, sizeof(int));
            
            int headerSize = ReadInt32(ref headerSizeBuffer, 0);

            byte[] serializedHeader = new byte[headerSize];

            stream.Read(serializedHeader, 0, headerSize);

            THeader header = (THeader)_headerSerializer.Deserialize(_headerType, serializedHeader);

            byte[] serializedPackage = new byte[header.PayloadSize];

            stream.Read(serializedPackage, 0, header.PayloadSize);
            
            return _packageSerializer.Deserialize(Type.GetType(header.PayloadType), serializedPackage);
        }
        
        public static unsafe int ReadInt32(ref byte[] bytes, int offset)
        {
            fixed (byte* numPtr = bytes)
                return *(int*) (numPtr + offset);
        }
        
        public static unsafe void WriteInt32(ref byte[] bytes, int offset, int value)
        {
            fixed (byte* numPtr = bytes)
                *(int*) (numPtr + offset) = value;
        }
    }
}