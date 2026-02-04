using PulseNetworkMonitor.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PulseNetworkMonitor.UI
{
    public class ScanResultsForm : Form
    {
        private readonly PulseController _controller;

        // Dashboard labels (value fields)
        private Label lblTotalDevices;
        private Label lblDevicesUp;
        private Label lblDevicesDown;
        private Label lblAvgLatency;
        private Label lblSubnet;
        private Label lblLastScan;
        private Label lblNextScan;

        // Tray icon menu
        private ContextMenuStrip trayMenu;
        private NotifyIcon notifyIcon1;

        public ScanResultsForm(PulseController controller)
        {
            _controller = controller;

            BuildUI();
            BuildTrayIcon();

            // Subscribe to dashboard updates
            _controller.DashboardUpdated += OnDashboardUpdated;
        }

        // ---------------------------------------------------------
        // UI Construction (code-only, no Designer)
        // ---------------------------------------------------------
        private void BuildUI()
        {
            Text = "Pulse Network Monitor";
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.Gainsboro;
            Font = new Font("Segoe UI", 10);
            Size = new Size(600, 400);
            StartPosition = FormStartPosition.CenterScreen;

            int xLabel = 20;
            int xValue = 200;
            int y = 20;
            int spacing = 30;

            lblTotalDevices = MakeRow("Total Devices:", xLabel, xValue, y); y += spacing;
            lblDevicesUp = MakeRow("Devices Up:", xLabel, xValue, y); y += spacing;
            lblDevicesDown = MakeRow("Devices Down:", xLabel, xValue, y); y += spacing;
            lblAvgLatency = MakeRow("Average Latency:", xLabel, xValue, y); y += spacing;
            lblSubnet = MakeRow("Subnet:", xLabel, xValue, y); y += spacing;
            lblLastScan = MakeRow("Last Scan:", xLabel, xValue, y); y += spacing;
            lblNextScan = MakeRow("Next Scan:", xLabel, xValue, y);

            var upBullet = MakeStatusBullet(Color.LimeGreen, Color.Black, 20, 220);
            var downBullet = MakeStatusBullet(Color.Red, Color.Black, 20, 250);

        }

        private Label MakeRow(string labelText, int xLabel, int xValue, int y)
        {
            var lbl = new Label
            {
                Text = labelText,
                Left = xLabel,
                Top = y,
                AutoSize = true
            };
            Controls.Add(lbl);

            var value = new Label
            {
                Text = "...",
                Left = xValue,
                Top = y,
                AutoSize = true
            };
            Controls.Add(value);

            return value;
        }

        // ---------------------------------------------------------
        // System Tray Setup
        // ---------------------------------------------------------
        private void BuildTrayIcon()
        {
            trayMenu = new ContextMenuStrip
            {
                Renderer = new SteamMenuRenderer()
            };

            // Add custom header
            var header = new TrayHeader();
            trayMenu.Items.Add(new ToolStripControlHost(header));

            trayMenu.Items.Add(new ToolStripSeparator());

            trayMenu.Items.Add("Show", null, (s, e) =>
            {
                Show();
                WindowState = FormWindowState.Normal;
                Activate();
            });

            trayMenu.Items.Add("Hide", null, (s, e) => Hide());
            trayMenu.Items.Add("Exit", null, (s, e) => Application.Exit());

            notifyIcon1 = new NotifyIcon
            {
                Visible = true,
                Text = "Pulse Network Monitor",
                Icon = SystemIcons.Information,
                ContextMenuStrip = trayMenu
            };

            notifyIcon1.DoubleClick += (s, e) =>
            {
                Show();
                WindowState = FormWindowState.Normal;
                Activate();
            };
        }


        public class TrayHeader : Panel
        {
            public TrayHeader()
            {
                Height = 32;
                Width = 200;
                BackColor = Color.FromArgb(28, 28, 28);
                Margin = new Padding(0);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                var font = new Font("Segoe UI Semibold", 10f);
                string text = "Pulse Network Monitor";

                // Split for cyan highlight
                int pulseIndex = text.IndexOf("Pulse");
                string before = text.Substring(0, pulseIndex);
                string pulse = "Pulse";
                string after = text.Substring(pulseIndex + pulse.Length);

                int x = 8;
                int y = 8;

                // Draw "before"
                TextRenderer.DrawText(e.Graphics, before, font, new Point(x, y), Color.Gainsboro);
                x += TextRenderer.MeasureText(before, font).Width - 4;

                // Draw "Pulse" in cyan
                TextRenderer.DrawText(e.Graphics, pulse, font, new Point(x, y), Color.LightCyan);
                x += TextRenderer.MeasureText(pulse, font).Width - 4;

                // Draw "after"
                TextRenderer.DrawText(e.Graphics, after, font, new Point(x, y), Color.Gainsboro);
            }
        }



        // ---------------------------------------------------------
        // Dashboard Update Handler
        // ---------------------------------------------------------
        private void OnDashboardUpdated(DashboardSnapshot snapshot)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateDashboard(snapshot)));
                return;
            }

            UpdateDashboard(snapshot);
        }

        private Control MakeStatusBullet(Color fillColor, Color outlineColor, int x, int y)
        {
            var bullet = new Panel
            {
                Width = 14,
                Height = 14,
                Left = x,
                Top = y
            };

            bullet.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                var rect = new Rectangle(1, 1, bullet.Width - 3, bullet.Height - 3);

                using (var fill = new SolidBrush(fillColor))
                using (var pen = new Pen(outlineColor, 1.5f))
                {
                    e.Graphics.FillEllipse(fill, rect);
                    e.Graphics.DrawEllipse(pen, rect);
                }
            };

            Controls.Add(bullet);
            return bullet;
        }


        private void UpdateDashboard(DashboardSnapshot snapshot)
        {
            lblTotalDevices.Text = snapshot.TotalDevices.ToString();
            lblDevicesUp.Text = snapshot.DevicesUp.ToString();
            lblDevicesDown.Text = snapshot.DevicesDown.ToString();
            lblAvgLatency.Text = $"{snapshot.AverageLatency:0.0} ms";
            lblSubnet.Text = snapshot.Subnet;
            lblLastScan.Text = snapshot.LastScan.ToString("yyyy-MM-dd HH:mm:ss");
            lblNextScan.Text = snapshot.NextScan.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // ---------------------------------------------------------
        // Hide-to-tray behavior
        // ---------------------------------------------------------
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            notifyIcon1.Visible = false;
            base.OnFormClosing(e);
        }
    }
}
