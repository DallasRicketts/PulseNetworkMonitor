using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace PulseNetworkMonitor.Core
{
    public class ScanEngine
    {
        public string CurrentSubnet { get; private set; } = "0.0.0.0/24";

        public async Task<List<DeviceModel>> RunScanAsync(CancellationToken token)
        {
            string? localIp = GetLocalIPv4();
            if (localIp == null)
                return new List<DeviceModel>();

            string subnet = string.Join(".", localIp.Split('.')[0..3]);
            CurrentSubnet = $"{subnet}.0/24";

            var hosts = Enumerable.Range(1, 254)
                                  .Select(i => $"{subnet}.{i}")
                                  .ToList();

            var results = new List<DeviceModel>();
            var tasks = new List<Task>();

            SemaphoreSlim limiter = new SemaphoreSlim(50);

            foreach (string ip in hosts)
            {
                await limiter.WaitAsync(token);

                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        var device = await ScanHostAsync(ip, token);
                        lock (results)
                            results.Add(device);
                    }
                    finally
                    {
                        limiter.Release();
                    }
                }, token));
            }

            await Task.WhenAll(tasks);

            return results.OrderBy(d => d.IP).ToList();
        }

        private async Task<DeviceModel> ScanHostAsync(string ip, CancellationToken token)
        {
            long latency = await PingHostAsync(ip, token);
            bool isUp = latency >= 0;

            return new DeviceModel
            {
                IP = ip,
                Hostname = isUp ? ResolveHostname(ip) : "",
                IsUp = isUp,
                LatencyMs = latency,
                LastSeen = isUp ? DateTime.Now : null
            };
        }

        private async Task<long> PingHostAsync(string ip, CancellationToken token)
        {
            using var ping = new Ping();

            try
            {
                var reply = await ping.SendPingAsync(ip, 1000);
                if (reply.Status == IPStatus.Success)
                    return reply.RoundtripTime;
            }
            catch { }

            return -1;
        }

        private string ResolveHostname(string ip)
        {
            try
            {
                var entry = System.Net.Dns.GetHostEntry(ip);
                return entry.HostName;
            }
            catch
            {
                return "";
            }
        }

        private string? GetLocalIPv4()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;

                var props = ni.GetIPProperties();
                foreach (var addr in props.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                        return addr.Address.ToString();
                }
            }

            return null;
        }
    }
}
