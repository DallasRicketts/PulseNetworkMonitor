using System;
using System.Drawing;
using System.Windows.Forms;

namespace PulseNetworkMonitor
{
    public partial class Form1 : Form
    {
        private TrayManager _trayManager;
        private NetworkMonitor _networkMonitor;
        private System.Windows.Forms.Timer _headerTimer;

        // External console form
        private FormConsole _consoleForm;

        // Smooth heartbeat fade
        private Color[] _statusColors = new Color[]
        {
            Color.LightCyan,
            Color.Yellow,
            Color.Orange,
            Color.Red
        };
        private int _currentColorIndex = 0;
        private int _nextColorIndex = 1;
        private float _fadeStep = 0f;
        private float _fadeSpeed = 0.03f;

        public Form1()
        {
            InitializeComponent();

            _trayManager = new TrayManager(notifyIcon1, contextMenuStrip1);
            _networkMonitor = new NetworkMonitor();

            HideMainForm();
            SetupHeaderTimer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HideMainForm();
        }

        private void HideMainForm()
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Hide();
        }

        #region Shared Methods

        private void DoScan()
        {
            MessageBox.Show("Scan Clicked");
            // TODO: implement scan logic
        }

        private void DoDevices()
        {
            MessageBox.Show("Devices Clicked");
            // TODO: implement devices logic
        }

        private void DoConsole()
        {
            if (_consoleForm == null || _consoleForm.IsDisposed)
                _consoleForm = new FormConsole();

            _consoleForm.Show();
            _consoleForm.BringToFront();
        }

        private void DoExit()
        {
            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();

            if (_consoleForm != null && !_consoleForm.IsDisposed)
                _consoleForm.Close();

            Application.Exit();
        }

        #endregion

        #region Designer Event Handlers

        // Tray menu handlers (assigned in designer)
        private void scanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoScan();
        }

        private void devicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoDevices();
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoConsole();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoExit();
        }

        // Docked MenuStrip handlers (assigned in designer)
        private void scanToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoScan();
        }

        private void devicesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoDevices();
        }

        private void consoleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoConsole();
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoExit();
        }

        #endregion

        private void ShowSettings()
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            BringToFront();
            Activate();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;   // prevent actual close
                HideMainForm();    // hide instead
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        #region Smooth Pulse Animation

        private void SetupHeaderTimer()
        {
            _headerTimer = new System.Windows.Forms.Timer();
            _headerTimer.Interval = 50;

            _headerTimer.Tick += (s, e) =>
            {
                if (contextMenuStrip1.Renderer is SteamMenuRenderer renderer)
                {
                    float eased = EaseInOut(_fadeStep);

                    renderer.PulseColor = LerpColor(
                        _statusColors[_currentColorIndex],
                        _statusColors[_nextColorIndex],
                        eased);

                    _fadeStep += _fadeSpeed;

                    if (_fadeStep >= 1f)
                    {
                        _fadeStep = 0f;
                        _currentColorIndex = _nextColorIndex;
                        _nextColorIndex = (_nextColorIndex + 1) % _statusColors.Length;
                    }

                    contextMenuStrip1.Invalidate();
                }
            };

            _headerTimer.Start();
        }

        private Color LerpColor(Color a, Color b, float t)
        {
            int r = (int)(a.R + (b.R - a.R) * t);
            int g = (int)(a.G + (b.G - a.G) * t);
            int b2 = (int)(a.B + (b.B - a.B) * t);
            return Color.FromArgb(r, g, b2);
        }

        private float EaseInOut(float t)
        {
            return 0.5f - 0.5f * (float)Math.Cos(t * Math.PI);
        }

        #endregion
    }

    #region Tray Manager

    public class TrayManager
    {
        private NotifyIcon _notifyIcon;
        private ContextMenuStrip _menu;

        public TrayManager(NotifyIcon notifyIcon, ContextMenuStrip menu)
        {
            _notifyIcon = notifyIcon;
            _menu = menu;
            InitializeTray();
        }

        private void InitializeTray()
        {
            _menu.Renderer = new SteamMenuRenderer();
            _menu.ShowImageMargin = false;

            foreach (ToolStripItem item in _menu.Items)
            {
                item.AutoSize = false;
                item.Height = 28;
                item.Padding = new Padding(8, 6, 8, 6);
            }

            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenuStrip = _menu;
        }
    }

    #endregion

    #region Placeholder Network Monitor

    public class NetworkMonitor
    {
        public NetworkMonitor()
        {
            // Placeholder
        }
    }

    #endregion
}
