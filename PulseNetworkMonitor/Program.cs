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
            controller.ApplySettings();
            controller.Start();

            // Create main UI form (start hidden)
            var form = new ScanResultsForm(controller)
            {
                ShowInTaskbar = false,
                WindowState = FormWindowState.Minimized
            };

            // Hide BEFORE the form is ever shown
            form.Load += (s, e) =>
            {
                form.Hide();
                form.WindowState = FormWindowState.Normal; // Reset for when user opens it later
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
