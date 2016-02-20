using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChGK.Core.Utils
{
    public sealed class ChGKTimer : IDisposable
    {
        private int _seconds;

        private CancellationTokenSource _tokenSource;

        public void Dispose()
        {
            _tokenSource?.Cancel();
        }

        public event EventHandler<TimerEventArgs> OneSecond;

        private void OnOneSecond(int second)
        {
            OneSecond?.Invoke(this, new TimerEventArgs(second));
        }

        public void Pause()
        {
            _tokenSource?.Cancel();
        }

        public void Resume()
        {
            _tokenSource = new CancellationTokenSource();
            Task.Delay(1000, _tokenSource.Token).ContinueWith(async (t, s) =>
            {
                var handler = ((Tuple<Action<int>>) s).Item1;
                while (true)
                {
                    if (_tokenSource.IsCancellationRequested)
                        break;
                    _seconds++;

                    handler(_seconds);
                    await Task.Delay(1000);
                }
            }, Tuple.Create<Action<int>>(OnOneSecond), _tokenSource.Token,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }
    }

    public class TimerEventArgs : EventArgs
    {
        public TimerEventArgs(int seconds)
        {
            Seconds = new TimeSpan(0, 0, seconds);
        }

        public TimeSpan Seconds { get; private set; }
    }
}