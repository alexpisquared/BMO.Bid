using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OLP.DAQ
{
	public partial class MainOperView : Window
	{
		public MainOperView() { InitializeComponent(); DataContext = this;   MouseLeftButtonDown += (s, e) => DragMove(); }

		async void onRunNameMatcherDiff(object sender, RoutedEventArgs e) { g1.IsEnabled = ((Button)sender).IsEnabled = false; /*await Task.Run(() => OLP.DAQ.OdbcAPMS.FeedProcessorMain.RunFinalSteps(false));*/ g1.IsEnabled = ((Button)sender).IsEnabled = true; }
		async void onRunNameMatcherFull(object sender, RoutedEventArgs e) { g1.IsEnabled = ((Button)sender).IsEnabled = false; /*await Task.Run(() => OLP.DAQ.OdbcAPMS.FeedProcessorMain.RunFinalSteps(true)); */g1.IsEnabled = ((Button)sender).IsEnabled = true; }
		void onExit(object sender, RoutedEventArgs e) { Close(); App.Current.Shutdown(); }
	}
}
