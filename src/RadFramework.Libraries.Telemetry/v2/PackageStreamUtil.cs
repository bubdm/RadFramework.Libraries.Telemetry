using System;
using System.IO;

namespace RadFramework.Libraries.Telemetry.v2.PackageWriters
{
    public static class PackageStreamUtil
    {
        public static void WritePackage(Stream stream, SerializedPackage package)
        {
            stream.Write(package.HeaderSize);
            stream.Write(package.Header);
            stream.Write(package.Package);
        }

        public static THeader ReadPackageHeader<THeader>(Stream stream, Type headerType, IContractSerializer headerSerializer) where THeader : HeaderBase, new()
        {
            byte[] serializedHeaderSize = new byte[sizeof(int)];
            
            stream.Read(serializedHeaderSize, 0, sizeof(int));

            int headerSize = ReadInt32(ref serializedHeaderSize);
            
            byte[] serializedHeader = new byte[headerSize];
            
            stream.Read(serializedHeader, 0, headerSize);

            return (THeader)headerSerializer.Deserialize(headerType, serializedHeader);
        }

        public static object ReadPackage(Stream stream, IContractSerializer handshakeSerializer)
        {
            return ReadPackage(stream, handshakeSerializer, handshakeSerializer);
        }

        public static object ReadPackage(Stream stream, IContractSerializer headerSerializer, IContractSerializer packageSerializer)
        {
            HeaderBase header = ReadPackageHeader<HeaderBase>(stream, typeof(HeaderBase), headerSerializer);

            return ReadPackage(stream, header, packageSerializer);
        }
        
        public static object ReadPackage(Stream stream, HeaderBase header, IContractSerializer packageSerializer)
        {
            byte[] serializedPackage = new byte[header.PayloadSize];

            stream.Read(serializedPackage, 0, header.PayloadSize);

            return packageSerializer.Deserialize(header.PayloadType, serializedPackage);
        }
        
        private static unsafe int ReadInt32(ref byte[] bytes)
        {
            fixed (byte* numPtr = bytes)
                return *(int*) (numPtr);
        }
    }
}