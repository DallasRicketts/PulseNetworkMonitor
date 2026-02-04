using System;
using System.Collections.Generic;
using System.IO;

namespace PulseNetworkMonitor.Core
{
    public static class SettingsManager
    {
        private static readonly string SettingsPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pulse_settings.ini");

        // Default values
        public static int ScanIntervalMinutes { get; set; } = 5;
        public static bool OverlayEnabled { get; set; } = true;

        public static void Load()
        {
            if (!File.Exists(SettingsPath))
            {
                Save(); // create default file
                return;
            }

            var lines = File.ReadAllLines(SettingsPath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || !line.Contains("="))
                    continue;

                var parts = line.Split('=', 2);
                string key = parts[0].Trim().ToLower();
                string value = parts[1].Trim();

                switch (key)
                {
                    case "scan_interval_minutes":
                        if (int.TryParse(value, out int interval))
                            ScanIntervalMinutes = Math.Clamp(interval, 5, 60);
                        break;

                    case "overlay_enabled":
                        OverlayEnabled = value.Equals("true", StringComparison.OrdinalIgnoreCase);
                        break;
                }
            }
        }

        public static void Save()
        {
            var lines = new List<string>
            {
                "scan_interval_minutes=" + ScanIntervalMinutes,
                "overlay_enabled=" + OverlayEnabled
            };

            File.WriteAllLines(SettingsPath, lines);
        }
    }
}
