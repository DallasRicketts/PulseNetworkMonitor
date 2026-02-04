using System;

namespace PulseNetworkMonitor.Core
{
    public class DeviceModel
    {
        public string IP { get; set; } = "";
        public string Hostname { get; set; } = "";
        public bool IsUp { get; set; }
        public long LatencyMs { get; set; } = -1;
        public DateTime? LastSeen { get; set; }

        // Optional future fields
        public string DeviceType { get; set; } = "";
        public string MacAddress { get; set; } = "";
        public string Vendor { get; set; } = "";

        public override string ToString()
        {
            string status = IsUp ? "Up" : "Down";
            string latency = LatencyMs >= 0 ? $"{LatencyMs} ms" : "N/A";
            string seen = LastSeen?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never";

            return $"{IP} | {Hostname} | {status} | {latency} | Last Seen: {seen}";
        }
    }
}
