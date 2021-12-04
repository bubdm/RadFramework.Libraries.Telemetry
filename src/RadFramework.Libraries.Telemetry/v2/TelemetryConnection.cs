using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace RadFramework.Libraries.Telemetry.v2
{
    internal class TelemetryConnection
    {
        private ConcurrentDictionary<int, Stream> readerStreams = new ConcurrentDictionary<int, Stream>();
        private ConcurrentDictionary<int, Stream> writerStreams = new ConcurrentDictionary<int, Stream>();

        public void RegisterReadStream(Stream stream)
        {
            readerStreams.Add(());
        }

        public void RegisterWriteStream(Stream stream)
        {
            writerStreams.Add(());
        }

        private Thread CreateThread(Action<Stream> threadBody)
        {
            Thread thread = new Thread((stream) => threadBody((Stream)stream));
            
            thread.Start();
            
            return thread;
        }
        
        ConcurrentQueue<object> 

        private void Reader(Stream stream)
        {
            while (true)
            {
                
            }
        }

        private void Writer(Stream stream)
        {
            while (true)
            {
                
            }
        }
    }
}