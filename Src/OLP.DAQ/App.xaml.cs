using OLP.DAQ.Views;
using System;
using System.Diagnostics;
using System.Windows;

namespace OLP.DAQ
{
  public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Trace.WriteLine(string.Format("\r\n*{0:MMdd HH:mm} The Start.", DateTime.Now));

			new DaqControlPanel().ShowDialog();

			App.Current.Shutdown();
		}
		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			Trace.WriteLine(string.Format("\r\n*{0:MMdd HH:mm} The End.     Took {1:d\\.hh\\:mm} (days.hh:mm).", DateTime.Now, SW.Elapsed));
		}

		public Stopwatch SW = Stopwatch.StartNew();
	}
}
// AC_TVT.ico needs to be dislodged. Mar 2016.