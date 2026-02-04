using System;
using System.IO;
using System.Text;

namespace PulseNetworkMonitor.Core
{
    public static class LogManager
    {
        private static readonly string LogRoot =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        private static readonly object _lock = new();

        static LogManager()
        {
            if (!Directory.Exists(LogRoot))
                Directory.CreateDirectory(LogRoot);
        }

        // -------------------------------
        // App-level logging
        // -------------------------------
        public static void LogApp(string message)
        {
            WriteLog("app", message);
        }

        public static void LogError(string message)
        {
            WriteLog("errors", message);
        }

        // -------------------------------
        // Device-level logging
        // -------------------------------
        public static void LogDevice(DeviceModel device, string message)
        {
            string safeIp = device.IP.Replace(".", "_");
            WriteLog($"device_{safeIp}", message);
        }

        // -------------------------------
        // Core writer
        // -------------------------------
        private static void WriteLog(string category, string message)
        {
            try
            {
                lock (_lock)
                {
                    string date = DateTime.Now.ToString("yyyy-MM-dd");
                    string file = Path.Combine(LogRoot, $"{category}_{date}.log");

                    string line = $"[{DateTime.Now:HH:mm:ss}] {message}";

                    File.AppendAllText(file, line + Environment.NewLine, Encoding.UTF8);
                }
            }
            catch
            {
                // Logging must never crash the app
            }
        }
    }
}
