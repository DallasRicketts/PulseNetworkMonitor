using PulseNetworkMonitor.UI;
using System;
using System.Windows.Forms;

namespace PulseNetworkMonitor.Core
{
    public static class AppBootstrap
    {
        private static PulseController? _controller;

        public static void Start()
        {
            // Load settings
            SettingsManager.Load();

            // Create controller
            _controller = new PulseController();

            // Apply settings (interval, overlay, etc.)
            _controller.ApplySettings();

            // Start scanning
            _controller.Start();

            // Launch main UI
            var mainForm = new ScanResultsForm(_controller);

            // Show overlay if enabled
            if (SettingsManager.OverlayEnabled)
                OverlayForm.ToggleInstance(_controller);

            Application.Run(mainForm);
        }

        public static void Stop()
        {
            try
            {
                _controller?.Stop();
            }
            catch
            {
                // Shutdown must never throw
            }
        }
    }
}
