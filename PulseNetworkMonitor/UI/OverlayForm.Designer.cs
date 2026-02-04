namespace PulseNetworkMonitor.UI
{
    partial class OverlayForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblUp;
        private System.Windows.Forms.Label lblDown;
        private System.Windows.Forms.Label lblLatency;
        private System.Windows.Forms.Label lblLastScan;

        private System.Windows.Forms.Label labelUp;
        private System.Windows.Forms.Label labelDown;
        private System.Windows.Forms.Label labelLatency;
        private System.Windows.Forms.Label labelLastScan;

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
            // Form Setup
            // -------------------------
            this.Text = "Pulse Overlay";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.TopMost = true;
            this.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(220, 140);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;

            // Enable dragging
            this.MouseDown += OverlayForm_MouseDown;
            this.MouseMove += OverlayForm_MouseMove;
            this.MouseUp += OverlayForm_MouseUp;

            // -------------------------
            // Static Labels
            // -------------------------
            labelUp = MakeLabel("Up:", 10, 10);
            labelDown = MakeLabel("Down:", 10, 40);
            labelLatency = MakeLabel("Latency:", 10, 70);
            labelLastScan = MakeLabel("Last Scan:", 10, 100);

            // -------------------------
            // Value Labels
            // -------------------------
            lblUp = MakeValueLabel(110, 10);
            lblDown = MakeValueLabel(110, 40);
            lblLatency = MakeValueLabel(110, 70);
            lblLastScan = MakeValueLabel(110, 100);

            // -------------------------
            // Add Controls
            // -------------------------
            this.Controls.Add(labelUp);
            this.Controls.Add(labelDown);
            this.Controls.Add(labelLatency);
            this.Controls.Add(labelLastScan);

            this.Controls.Add(lblUp);
            this.Controls.Add(lblDown);
            this.Controls.Add(lblLatency);
            this.Controls.Add(lblLastScan);
        }

        private System.Windows.Forms.Label MakeLabel(string text, int x, int y)
        {
            return new System.Windows.Forms.Label
            {
                Text = text,
                Left = x,
                Top = y,
                Width = 90,
                ForeColor = System.Drawing.Color.Gainsboro,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.Transparent
            };
        }

        private System.Windows.Forms.Label MakeValueLabel(int x, int y)
        {
            return new System.Windows.Forms.Label
            {
                Text = "-",
                Left = x,
                Top = y,
                Width = 100,
                ForeColor = System.Drawing.Color.LightSkyBlue,
                Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Regular),
                BackColor = System.Drawing.Color.Transparent
            };
        }
    }
}
