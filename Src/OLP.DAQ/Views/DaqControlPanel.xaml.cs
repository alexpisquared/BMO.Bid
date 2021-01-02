using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OLP.DAQ.Views
{
	public partial class DaqControlPanel : Window
	{
		public DaqControlPanel()
		{
			InitializeComponent(); DataContext = this; MouseLeftButtonDown += (s, e) => DragMove(); KeyDown += (s, ves) => { switch (ves.Key) { case Key.Escape: Close(); App.Current.Shutdown(); break; } };

      			c00.CurPos = c01.CurPos = c02.CurPos = c10.CurPos = c11.CurPos = c12.CurPos = c13.CurPos = c20.CurPos = c21.CurPos = cf1.CurPos = cf2.CurPos = 0;
//#if DEBUG
//			Top = 1111;
//#endif
		}

		async void onRunNameMatcherDiff(object sender, RoutedEventArgs e) { g1.IsEnabled = ((Button)sender).IsEnabled = false; /*await Task.Run(() => OLP.DAQ.OdbcAPMS.FeedProcessorMain.RunFinalSteps(false));*/ g1.IsEnabled = ((Button)sender).IsEnabled = true; }
		async void onRunNameMatcherFull(object sender, RoutedEventArgs e) { g1.IsEnabled = ((Button)sender).IsEnabled = false; /*await Task.Run(() => OLP.DAQ.OdbcAPMS.FeedProcessorMain.RunFinalSteps(true)); */g1.IsEnabled = ((Button)sender).IsEnabled = true; }

		public static List<Task> TaskList = new List<Task>();
		async void onDoAll(object sender, RoutedEventArgs e)
		{
			c00.CurPos = c01.CurPos = c02.CurPos = c10.CurPos = c11.CurPos = c12.CurPos = c13.CurPos = c20.CurPos = c21.CurPos = cf1.CurPos = cf2.CurPos = 0;

			TaskList.Add(doLane0());
			TaskList.Add(doLane1());
			TaskList.Add(doLane2());

			await Task.WhenAll(TaskList.ToArray());

			await cf1.DoJob();
			await cf2.DoJob();
		}

		async Task doLane0() { await c00.DoJob(); await c01.DoJob(); await c02.DoJob(); }
		async Task doLane1() { await c10.DoJob(); await c11.DoJob(); await c12.DoJob(); await c13.DoJob(); }
		async Task doLane2() { await c20.DoJob(); await c21.DoJob(); }
	}
}
