using PulseNetworkMonitor.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PulseNetworkMonitor.UI
{
    public partial class DevicesForm : Form
    {
        private readonly PulseController _controller;

        public DevicesForm(PulseController controller)
        {
            InitializeComponent();
            _controller = controller;

            // Subscribe to updates
            _controller.DevicesUpdated += OnDevicesUpdated;

            // Load initial state
            RefreshDeviceList(_controller.GetCurrentDevices());
        }

        private void DevicesForm_Load(object sender, EventArgs e)
        {
            // Nothing needed yet
        }

        private void OnDevicesUpdated(List<DeviceModel> devices)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => RefreshDeviceList(devices)));
                return;
            }

            RefreshDeviceList(devices);
        }

        private void RefreshDeviceList(List<DeviceModel> devices)
        {
            deviceListView.BeginUpdate();
            deviceListView.Items.Clear();

            foreach (var d in devices)
            {
                var item = new ListViewItem(d.IP);
                item.SubItems.Add(d.Hostname);
                item.SubItems.Add(d.IsUp ? "Up" : "Down");
                item.SubItems.Add(d.LatencyMs >= 0 ? $"{d.LatencyMs} ms" : "N/A");
                item.SubItems.Add(d.LastSeen?.ToString("yyyy-MM-dd HH:mm:ss") ?? "Never");

                if (d.IsUp)
                    item.ForeColor = System.Drawing.Color.LightGreen;
                else
                    item.ForeColor = System.Drawing.Color.IndianRed;

                deviceListView.Items.Add(item);
            }

            deviceListView.EndUpdate();
        }
    }
}
