using BMO.OLP.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace OLP.DAQ.Views
{
	public partial class ProgressiveStepUsrCtrl : UserControl, IProgressible
	{
		public ProgressiveStepUsrCtrl()
		{
			InitializeComponent(); DataContext = this;
			//"PropertyMetadata is already registered for type DependencyObject"   ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue)); //tu: ToolTip ShowDuration !!!

			_timer = new DispatcherTimer(TimeSpan.FromSeconds(.02), DispatcherPriority.Background, new EventHandler((s, e) => tick()), Dispatcher.CurrentDispatcher);//tu: one-line timer
			_timer.Stop();
		}

		#region Copy for UI simulation only:
		DispatcherTimer _timer;
		async public Task DoJob()
		{
			await Start();
			do
			{
				await Task.Delay(199);
			} while (_timer.IsEnabled);
		}
		public async Task Start() { MaxPos = maximumPosition; CurPos = currentPosition = 0; _timer.Start(); await Task.Delay(9); }
		public void Stop() { _timer.Stop(); }
		public double MaximumPosition { get { return maximumPosition; } set { MaxPos = maximumPosition = value; } } double maximumPosition = 100, currentPosition = 0;
		public double CurrentPosition { get { return currentPosition; } set { currentPosition = value; } }
		public string ProcessLogStory { get { return processLogStory; } set { processLogStory = value; } } string processLogStory = "";
		public string StepNm { set { StpNme = stepNm = value; } }  string stepNm = "Not set", descrn = "Not set", report = "Not done";
		public string Descrn { get { return descrn; } set { descrn = value; } }
		public string Report { get { return report; } set { report = value; } }

		void tick()
		{
			currentPosition++;

			CurPos = currentPosition;
			if (currentPosition >= maximumPosition)
			{
				_timer.Stop();
				report = "All done";

        //CurPos = OlpPresets.Failed;
			}
		}
		#endregion

		public double MaxPos { get { return (double)GetValue(MaxPosProperty); } set { SetValue(MaxPosProperty, value); } }public static readonly DependencyProperty MaxPosProperty = DependencyProperty.Register("MaxPos", typeof(double), typeof(ProgressiveStepUsrCtrl), new PropertyMetadata(55d));
		public double CurPos { get { return (double)GetValue(CurPosProperty); } set { SetValue(CurPosProperty, value); } }public static readonly DependencyProperty CurPosProperty = DependencyProperty.Register("CurPos", typeof(double), typeof(ProgressiveStepUsrCtrl), new PropertyMetadata(11d));
		public string StpNme { get { return (string)GetValue(StpNmeProperty); } set { SetValue(StpNmeProperty, value); } }public static readonly DependencyProperty StpNmeProperty = DependencyProperty.Register("StpNme", typeof(string), typeof(ProgressiveStepUsrCtrl), new PropertyMetadata(""));
		public string StpDsc { get { return (string)GetValue(StpDscProperty); } set { SetValue(StpDscProperty, value); } }public static readonly DependencyProperty StpDscProperty = DependencyProperty.Register("StpDsc", typeof(string), typeof(ProgressiveStepUsrCtrl), new PropertyMetadata("Step description Step description Step description Step description Step description Step description Step description Step description "));
		public DateTime BatchTm { get; set; }

		public Task Start(DateTime batchT, bool isFull = false)
		{
			throw new NotImplementedException();
		}
	}
}
