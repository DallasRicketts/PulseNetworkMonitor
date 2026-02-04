namespace PulseNetworkMonitor.UI
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label labelInterval;
        private System.Windows.Forms.NumericUpDown numScanInterval;
        private System.Windows.Forms.CheckBox chkOverlayEnabled;

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            this.Text = "Pulse Settings";
            this.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(360, 180);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            // -------------------------
            // Scan Interval Label
            // -------------------------
            labelInterval = new System.Windows.Forms.Label();
            labelInterval.Text = "Scan Interval (minutes):";
            labelInterval.Left = 20;
            labelInterval.Top = 20;
            labelInterval.Width = 200;
            labelInterval.ForeColor = System.Drawing.Color.Gainsboro;

            // -------------------------
            // NumericUpDown
            // -------------------------
            numScanInterval = new System.Windows.Forms.NumericUpDown();
            numScanInterval.Left = 220;
            numScanInterval.Top = 18;
            numScanInterval.Width = 80;
            numScanInterval.Minimum = 5;
            numScanInterval.Maximum = 60;
            numScanInterval.Value = 5;
            numScanInterval.BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            numScanInterval.ForeColor = System.Drawing.Color.Gainsboro;

            // -------------------------
            // Overlay Checkbox
            // -------------------------
            chkOverlayEnabled = new System.Windows.Forms.CheckBox();
            chkOverlayEnabled.Text = "Enable Overlay";
            chkOverlayEnabled.Left = 20;
            chkOverlayEnabled.Top = 60;
            chkOverlayEnabled.Width = 200;
            chkOverlayEnabled.ForeColor = System.Drawing.Color.Gainsboro;
            chkOverlayEnabled.BackColor = System.Drawing.Color.Transparent;

            // -------------------------
            // Save Button
            // -------------------------
            btnSave = new System.Windows.Forms.Button();
            btnSave.Text = "Save";
            btnSave.Left = 80;
            btnSave.Top = 110;
            btnSave.Width = 80;
            btnSave.Click += btnSave_Click;
            btnSave.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnSave.ForeColor = System.Drawing.Color.Gainsboro;

            // -------------------------
            // Cancel Button
            // -------------------------
            btnCancel = new System.Windows.Forms.Button();
            btnCancel.Text = "Cancel";
            btnCancel.Left = 180;
            btnCancel.Top = 110;
            btnCancel.Width = 80;
            btnCancel.Click += btnCancel_Click;
            btnCancel.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            btnCancel.ForeColor = System.Drawing.Color.Gainsboro;

            // -------------------------
            // Add Controls
            // -------------------------
            this.Controls.Add(labelInterval);
            this.Controls.Add(numScanInterval);
            this.Controls.Add(chkOverlayEnabled);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }
    }
}
