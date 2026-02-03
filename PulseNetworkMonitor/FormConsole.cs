using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PulseNetworkMonitor
{
    public class FormConsole : Form
    {
        private RichTextBox _console;
        private Process _psProcess;
        private StreamWriter _psInput;

        public FormConsole()
        {
            Text = "Pulse – Embedded PowerShell";
            Width = 800;
            Height = 500;
            StartPosition = FormStartPosition.CenterScreen;

            _console = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                ForeColor = Color.LightGreen,
                Font = new Font("Consolas", 10),
                Multiline = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            Controls.Add(_console);

            StartPowerShell();
            _console.AppendText("PS> ");

            _console.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;

                    string cmd = GetLastCommand();
                    if (!string.IsNullOrWhiteSpace(cmd))
                        await SendCommand(cmd);

                    _console.AppendText(Environment.NewLine + "PS> ");
                    _console.SelectionStart = _console.Text.Length;
                    _console.ScrollToCaret();
                }
            };

            FormClosing += (s, e) =>
            {
                try
                {
                    if (_psProcess != null && !_psProcess.HasExited)
                        _psProcess.Kill();
                }
                catch { }
            };
        }

        private void StartPowerShell()
        {
            _psProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-NoExit -Command -",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                }
            };

            _psProcess.OutputDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    AppendText(e.Data + Environment.NewLine);
            };

            _psProcess.ErrorDataReceived += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    AppendText("ERROR: " + e.Data + Environment.NewLine);
            };

            _psProcess.Start();
            _psInput = _psProcess.StandardInput;

            _psProcess.BeginOutputReadLine();
            _psProcess.BeginErrorReadLine();
        }

        private Task SendCommand(string command)
        {
            if (_psProcess == null || _psProcess.HasExited)
                return Task.CompletedTask;

            _psInput.WriteLine(command);
            _psInput.Flush();
            return Task.CompletedTask;
        }

        private void AppendText(string text)
        {
            if (InvokeRequired)
                Invoke(new Action(() => _console.AppendText(text)));
            else
                _console.AppendText(text);
        }

        private string GetLastCommand()
        {
            string line = _console.Lines.LastOrDefault() ?? "";
            return line.Replace("PS>", "").Trim();
        }
    }
}
