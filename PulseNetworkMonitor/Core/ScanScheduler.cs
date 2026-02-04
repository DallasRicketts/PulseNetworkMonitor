using System;
using System.Threading;
using System.Threading.Tasks;

namespace PulseNetworkMonitor.Core
{
    public class ScanScheduler
    {
        private readonly Func<CancellationToken, Task> _scanCallback;

        private CancellationTokenSource? _cts;
        private Task? _loopTask;
        private bool _running;

        public ScanScheduler(Func<CancellationToken, Task> scanCallback)
        {
            _scanCallback = scanCallback;
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;
            _cts = new CancellationTokenSource();

            _loopTask = Task.Run(() => LoopAsync(_cts.Token));
        }

        public void Stop()
        {
            _running = false;

            try
            {
                _cts?.Cancel();
            }
            catch { }

            _cts = null;
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        private async Task LoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                int interval = SettingsManager.ScanIntervalMinutes;

                try
                {
                    await _scanCallback(token);
                }
                catch (OperationCanceledException)
                {
                    // Normal during shutdown
                }
                catch (Exception ex)
                {
                    LogManager.LogError("Scheduler scan error: " + ex.Message);
                }

                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(interval), token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
