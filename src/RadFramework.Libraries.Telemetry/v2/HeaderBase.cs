using MessagePack;

namespace RadFramework.Libraries.Telemetry.v2
{
    [MessagePackObject]
    public class HeaderBase
    {
        [Key(0)]
        public int PayloadSize { get; set; }
        
        [Key(1)]
        public string PayloadType { get; set; }
    }
}