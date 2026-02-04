using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PulseNetworkMonitor.Core
{
    public class PulseController
    {
        private readonly ScanScheduler _scheduler;
        private readonly ScanEngine _scanEngine;

        // Events for UI binding
        public event Action<List<DeviceModel>>? DevicesUpdated;
        public event Action<DashboardSnapshot>? DashboardUpdated;

        // Internal state
        private List<DeviceModel> _devices = new();
        private readonly object _lock = new();

        public PulseController()
        {
            _scanEngine = new ScanEngine();
            _scheduler = new ScanScheduler(RunScheduledScanAsync);

            // Load settings
            SettingsManager.Load();
        }

        public void Start()
        {
            _scheduler.Start();
        }

        public void Stop()
        {
            _scheduler.Stop();
        }

        // Called by scheduler
        private async Task RunScheduledScanAsync(CancellationToken token)
        {
            var results = await _scanEngine.RunScanAsync(token);

            lock (_lock)
            {
                _devices = results;
            }

            LogManager.LogApp($"Scan completed. {results.Count} devices discovered.");

            DevicesUpdated?.Invoke(results);
            DashboardUpdated?.Invoke(BuildDashboardSnapshot(results));
        }

        private DashboardSnapshot BuildDashboardSnapshot(List<DeviceModel> devices)
        {
            int up = 0;
            int down = 0;
            double totalLatency = 0;

            foreach (var d in devices)
            {
                if (d.IsUp) up++;
                else down++;

                if (d.LatencyMs > 0)
                    totalLatency += d.LatencyMs;
            }

            double avgLatency = up > 0 ? totalLatency / up : 0;

            return new DashboardSnapshot
            {
                TotalDevices = devices.Count,
                DevicesUp = up,
                DevicesDown = down,
                AverageLatency = avgLatency,
                Subnet = _scanEngine.CurrentSubnet,
                LastScan = DateTime.Now,
                NextScan = DateTime.Now.AddMinutes(SettingsManager.ScanIntervalMinutes)
            };
        }

        // Called by SettingsForm
        public void ApplySettings()
        {
            SettingsManager.Save();
            _scheduler.Restart();
        }

        // Called by Console
        public void ExecuteConsoleCommand(string cmd)
        {
            cmd = cmd.Trim().ToLower();

            if (cmd == "pulse scan")
            {
                _ = RunScheduledScanAsync(CancellationToken.None);
            }
            else if (cmd == "pulse settings")
            {
                // UI layer will handle opening settings
            }
            else if (cmd.StartsWith("pulse overlay"))
            {
                // UI layer will toggle overlay
            }
        }

        public List<DeviceModel> GetCurrentDevices()
        {
            lock (_lock)
            {
                return new List<DeviceModel>(_devices);
            }
        }
    }

    public class DashboardSnapshot
    {
        public int TotalDevices { get; set; }
        public int DevicesUp { get; set; }
        public int DevicesDown { get; set; }
        public double AverageLatency { get; set; }
        public string Subnet { get; set; } = "";
        public DateTime LastScan { get; set; }
        public DateTime NextScan { get; set; }
    }
}
