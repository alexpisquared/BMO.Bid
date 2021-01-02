using BMO.OLP.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace OLP.DAQ.Logic
{
	public class ProgrDemo : IProgressible
	{
		DispatcherTimer _timer;

		public ProgrDemo()
		{
			_timer = new DispatcherTimer(TimeSpan.FromSeconds(2), DispatcherPriority.Background, new EventHandler((s, e) => tick()), Dispatcher.CurrentDispatcher);//tu: one-line timer
		}

		async public Task DoJob()
		{
			await Start();
			do
			{
				await Task.Delay(999);
			} while (_timer.IsEnabled);
		}
		public async Task Start() { _timer.Start(); await Task.Delay(9); }
		public void Stop() { _timer.Stop(); }
		public double MaximumPosition { get { return maximumPosition; } set { maximumPosition = value; } } double maximumPosition = 100, currentPosition = 0;
		public double CurrentPosition { get { return currentPosition; } set { currentPosition = value; } }
		public string ProcessLogStory { get { return processLogStory; } set { processLogStory = value; } } string processLogStory = "";

		public string Descrn { get { return descrn; } set { descrn = value; } }  string descrn = "Not set", report = "Not done";
		public string Report { get { return report; } set { report = value; } }

		void tick()
		{
			currentPosition++;
			if (currentPosition >= maximumPosition - 1)
			{
				_timer.Stop();
				report = "All done";
			}
		}

		public Task Start(DateTime batchT, bool isFull = false)
		{
			throw new NotImplementedException();
		}

		public DateTime BatchTm
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
