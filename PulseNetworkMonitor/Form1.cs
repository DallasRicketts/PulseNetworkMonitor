using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

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

        // Async scan engine fields
        private SemaphoreSlim _concurrencyLimiter;
        private ConcurrentQueue<ScanJob> _jobQueue;
        private CancellationTokenSource _cts;

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
            // Cancel previous scan if running
            _cts?.Cancel();

            // Disable button while scanning
            scanBtn.Enabled = false;
            richTextBox1.Clear();
            progressBar1.Value = 0;

            _cts = new CancellationTokenSource();
            int ttl = (int)numericUpDown1.Value;
            int packetCount = (int)numericUpDown2.Value;

            _ = StartScanAsync(ttl, packetCount, _cts.Token)
                .ContinueWith(t =>
                {
                    Invoke(() => scanBtn.Enabled = true);

                    if (t.IsCanceled)
                    {
                        Invoke(() => richTextBox1.AppendText("Scan canceled.\n"));
                    }
                    else if (t.Exception != null)
                    {
                        Invoke(() => richTextBox1.AppendText($"Error: {t.Exception}\n"));
                    }
                });
        }

        private void DoDevices()
        {
            MessageBox.Show("Devices Clicked");
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

        private void scanToolStripMenuItem_Click(object sender, EventArgs e) => DoScan();
        private void scanToolStripMenuItem1_Click(object sender, EventArgs e) => DoScan();

        private void devicesToolStripMenuItem_Click(object sender, EventArgs e) => DoDevices();
        private void devicesToolStripMenuItem1_Click(object sender, EventArgs e) => DoDevices();

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e) => DoConsole();
        private void consoleToolStripMenuItem1_Click(object sender, EventArgs e) => DoConsole();

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) => ShowSettings();
        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e) => ShowSettings();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => DoExit();
        private void exitToolStripMenuItem1_Click(object sender, EventArgs e) => DoExit();

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
                e.Cancel = true;
                HideMainForm();
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

        #region Scan Button Handler

        private void scanBtn_Click(object sender, EventArgs e)
        {
            DoScan();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            // Cancel any running scan
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            // Reset UI
            progressBar1.Value = 0;
            richTextBox1.Clear();

            // Re-enable scan button
            scanBtn.Enabled = true;
        }

        #endregion

        #region Async LAN Scan Engine

        private async Task StartScanAsync(int ttl, int packetCount, CancellationToken token)
        {
            _jobQueue = new ConcurrentQueue<ScanJob>();
            _concurrencyLimiter = new SemaphoreSlim(50); // max concurrent jobs

            // Step 1: Discover hosts automatically on local subnet
            var hosts = DiscoverLocalSubnetHosts();

            foreach (var ip in hosts)
                _jobQueue.Enqueue(new ScanJob { IP = ip, Type = ScanType.ICMP, RetryCount = packetCount });

            int totalJobs = _jobQueue.Count;
            int completedJobs = 0;
            var tasks = new List<Task>();

            while (_jobQueue.TryDequeue(out var job))
            {
                await _concurrencyLimiter.WaitAsync(token);

                var t = RunIcmpPingAsync(job, ttl, token)
                    .ContinueWith(_ =>
                    {
                        _concurrencyLimiter.Release();
                        Interlocked.Increment(ref completedJobs);

                        if (!token.IsCancellationRequested)
                        {
                            // Update UI
                            Invoke(() =>
                            {
                                richTextBox1.AppendText($"{job.IP} scanned.\n");
                                progressBar1.Value = (int)((completedJobs / (double)totalJobs) * 100);
                            });
                        }
                    }, token);

                tasks.Add(t);
            }

            await Task.WhenAll(tasks);
        }

        private string[] DiscoverLocalSubnetHosts()
        {
            string localIp = GetLocalIPv4();
            if (string.IsNullOrEmpty(localIp))
                return Array.Empty<string>();

            var subnet = string.Join(".", localIp.Split('.')[0..3]);
            var hosts = new List<string>();

            for (int i = 1; i <= 254; i++)
                hosts.Add($"{subnet}.{i}");

            return hosts.ToArray();
        }

        private string GetLocalIPv4()
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up) continue;
                var props = ni.GetIPProperties();
                foreach (var addr in props.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return addr.Address.ToString();
                }
            }
            return null;
        }

        private async Task RunIcmpPingAsync(ScanJob job, int ttl, CancellationToken token)
        {
            using var ping = new Ping();

            for (int i = 0; i < job.RetryCount; i++)
            {
                if (token.IsCancellationRequested)
                    break;

                try
                {
                    var reply = await ping.SendPingAsync(job.IP, 1000); // 1s timeout
                    if (reply.Status == IPStatus.Success)
                        break; // host responded, stop retries
                }
                catch
                {
                    // Ignore individual ping errors
                }

                await Task.Delay(50, token);
            }
        }

        #endregion
    }

    #region Scan Job Model

    public class ScanJob
    {
        public string IP;
        public int Port;
        public ScanType Type;
        public int RetryCount;
    }

    public enum ScanType
    {
        ICMP,
        ARP,
        TCP
    }

    #endregion

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
