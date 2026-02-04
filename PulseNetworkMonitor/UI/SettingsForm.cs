using PulseNetworkMonitor.Core;
using System;
using System.Windows.Forms;

namespace PulseNetworkMonitor.UI
{
    public partial class SettingsForm : Form
    {
        private readonly PulseController _controller;

        public SettingsForm(PulseController controller)
        {
            InitializeComponent();
            _controller = controller;

            // Load current settings
            numScanInterval.Value = SettingsManager.ScanIntervalMinutes;
            chkOverlayEnabled.Checked = SettingsManager.OverlayEnabled;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SettingsManager.ScanIntervalMinutes = (int)numScanInterval.Value;
            SettingsManager.OverlayEnabled = chkOverlayEnabled.Checked;

            SettingsManager.Save();
            _controller.ApplySettings();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
