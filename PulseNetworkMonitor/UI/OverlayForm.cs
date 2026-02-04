using PulseNetworkMonitor.Core;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PulseNetworkMonitor.UI
{
    public partial class OverlayForm : Form
    {
        private static OverlayForm? _instance;
        private readonly PulseController _controller;

        private bool _dragging = false;
        private Point _dragStart;

        public static void ToggleInstance(PulseController controller)
        {
            if (_instance == null || _instance.IsDisposed)
            {
                _instance = new OverlayForm(controller);
                _instance.Show();
            }
            else
            {
                _instance.Close();
                _instance = null;
            }
        }

        public OverlayForm(PulseController controller)
        {
            InitializeComponent();
            _controller = controller;

            // Subscribe to updates
            _controller.DashboardUpdated += OnDashboardUpdated;

            // Load initial state
            var snapshot = new DashboardSnapshot
            {
                TotalDevices = 0,
                DevicesUp = 0,
                DevicesDown = 0,
                AverageLatency = 0,
                Subnet = "",
                LastScan = DateTime.Now,
                NextScan = DateTime.Now
            };
            UpdateOverlay(snapshot);
        }

        // -----------------------------
        // Dashboard Update Handler
        // -----------------------------
        private void OnDashboardUpdated(DashboardSnapshot snapshot)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateOverlay(snapshot)));
                return;
            }

            UpdateOverlay(snapshot);
        }

        private void UpdateOverlay(DashboardSnapshot snapshot)
        {
            lblUp.Text = snapshot.DevicesUp.ToString();
            lblDown.Text = snapshot.DevicesDown.ToString();
            lblLatency.Text = $"{snapshot.AverageLatency:0.0} ms";
            lblLastScan.Text = snapshot.LastScan.ToString("HH:mm:ss");
        }

        // -----------------------------
        // Dragging Support
        // -----------------------------
        private void OverlayForm_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _dragStart = new Point(e.X, e.Y);
        }

        private void OverlayForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - _dragStart.X, p.Y - _dragStart.Y);
            }
        }

        private void OverlayForm_MouseUp(object sender, MouseEventArgs e)
        {
            _dragging = false;
        }
    }
}
