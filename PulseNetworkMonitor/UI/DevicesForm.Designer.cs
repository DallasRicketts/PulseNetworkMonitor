namespace PulseNetworkMonitor.UI
{
    partial class DevicesForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListView deviceListView;
        private System.Windows.Forms.ColumnHeader colIP;
        private System.Windows.Forms.ColumnHeader colHostname;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colLatency;
        private System.Windows.Forms.ColumnHeader colLastSeen;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // -------------------------
            // Device ListView
            // -------------------------
            deviceListView = new System.Windows.Forms.ListView();
            deviceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            deviceListView.View = System.Windows.Forms.View.Details;
            deviceListView.FullRowSelect = true;
            deviceListView.GridLines = true;
            deviceListView.BackColor = System.Drawing.Color.FromArgb(24, 24, 24);
            deviceListView.ForeColor = System.Drawing.Color.Gainsboro;

            colIP = new System.Windows.Forms.ColumnHeader() { Text = "IP Address", Width = 140 };
            colHostname = new System.Windows.Forms.ColumnHeader() { Text = "Hostname", Width = 180 };
            colStatus = new System.Windows.Forms.ColumnHeader() { Text = "Status", Width = 100 };
            colLatency = new System.Windows.Forms.ColumnHeader() { Text = "Latency", Width = 100 };
            colLastSeen = new System.Windows.Forms.ColumnHeader() { Text = "Last Seen", Width = 180 };

            deviceListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                colIP, colHostname, colStatus, colLatency, colLastSeen
            });

            // -------------------------
            // Form Setup
            // -------------------------
            this.Text = "All Devices";
            this.BackColor = System.Drawing.Color.FromArgb(28, 28, 28);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(700, 500);

            this.Controls.Add(deviceListView);
            this.Load += DevicesForm_Load;
        }
    }
}
