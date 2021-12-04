using System;

namespace RadFramework.Libraries.Telemetry.v2.Packages
{
    public class InitMultiplexConnectionResponse
    {
        public Guid ClientId { get; set; }
        public int CpuCores { get; set; }
    }
}