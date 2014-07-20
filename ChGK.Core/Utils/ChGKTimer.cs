using System;
using System.Threading.Tasks;
using System.Threading;

namespace ChGK.Core.Utils
{
	public sealed class ChGKTimer : IDisposable
	{
		public event EventHandler<TimerEventArgs> OneSecond;

		void OnOneSecond (int second)
		{
			var handler = OneSecond;
			if (handler != null) {
				handler (this, new TimerEventArgs (second));
			}
		}

		CancellationTokenSource tokenSource;

		int seconds = 0;

		public void Pause ()
		{
			tokenSource.Cancel ();
		}

		public void Resume ()
		{
			tokenSource = new CancellationTokenSource ();
			Task.Delay (1000, tokenSource.Token).ContinueWith (async (t, s) => {
				//Task.Delay (50 * 1000, Token).ContinueWith (async (t, s) => {
				var handler = ((Tuple<Action<int>>) s).Item1;
				while (true) {
					if (tokenSource.IsCancellationRequested)
						break;
					seconds++;

					handler (seconds);
					await Task.Delay (1000);
				}
			}, Tuple.Create<Action<int>> (OnOneSecond), CancellationToken.None,
				TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
				TaskScheduler.Default);

//			Task.Delay (60 * 1000, Token).ContinueWith (async (t, s) => {
//				var handler = ((Tuple<Action>) s).Item1;
//				if (IsCancellationRequested)
//					return;
//				await Task.Run (() => handler ());			
//			}, Tuple.Create<Action> (OnSixtySeconds), CancellationToken.None,
//				TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
//				TaskScheduler.Default);
		}

		public void Dispose ()
		{
			tokenSource.Cancel ();
		}
	}

	public class TimerEventArgs : EventArgs
	{
		public TimeSpan Seconds { get; private set; }

		public TimerEventArgs (int seconds)
		{
			Seconds = new TimeSpan (0, 0, seconds);
		}
	}
}

