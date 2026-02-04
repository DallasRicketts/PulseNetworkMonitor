using PulseNetworkMonitor.Core;
using PulseNetworkMonitor.UI;
using System;
using System.Windows.Forms;

namespace PulseNetworkMonitor
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // WinForms setup
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Load settings
            SettingsManager.Load();

            // Create controller
            var controller = new PulseController();

            // Apply settings (interval, overlay, etc.)
            controller.ApplySettings();

            // Start scanning
            controller.Start();

            // Create main UI form
            var form = new ScanResultsForm(controller);

            // Allow Windows to register the window ONCE, then hide it
            bool firstShow = true;
            form.Shown += (s, e) =>
            {
                if (firstShow)
                {
                    firstShow = false;
                    form.Hide();   // Hide immediately after first real show
                }
            };

            // Overlay logic
            if (SettingsManager.OverlayEnabled)
                OverlayForm.ToggleInstance(controller);

            // Run application
            Application.Run(form);

            // Clean shutdown
            controller.Stop();
        }
    }
}
