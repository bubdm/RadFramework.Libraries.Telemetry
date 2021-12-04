using System;
using MessagePack;

namespace RadFramework.Libraries.Telemetry.v2
{
    [MessagePackObject]
    public class HeaderBase
    {
        [Key(0)]
        public int PayloadSize { get; internal set; }
        
        [Key(1)]
        public Type PayloadType { get; internal set; }
    }
}