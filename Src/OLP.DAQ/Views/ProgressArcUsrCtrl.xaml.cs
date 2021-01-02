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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OLP.DAQ.Views
{
	public partial class ProgressArcUsrCtrl : UserControl
	{
		public ProgressArcUsrCtrl() { InitializeComponent(); DataContext = this; }

		public double MaxVal { get { return (double)GetValue(MaxValProperty); } set { SetValue(MaxValProperty, value); } }		public static readonly DependencyProperty MaxValProperty = DependencyProperty.Register("MaxVal", typeof(double), typeof(ProgressArcUsrCtrl), new PropertyMetadata(100.0));
		public double CurVal { get { return (double)GetValue(CurValProperty); } set { SetValue(CurValProperty, value); } }		public static readonly DependencyProperty CurValProperty = DependencyProperty.Register("CurVal", typeof(double), typeof(ProgressArcUsrCtrl), new PropertyMetadata(.0, new PropertyChangedCallback(recalc)));
		public double PgrDeg { get { return (double)GetValue(PgrDegProperty); } set { SetValue(PgrDegProperty, value); } }		public static readonly DependencyProperty PgrDegProperty = DependencyProperty.Register("PgrDeg", typeof(double), typeof(ProgressArcUsrCtrl), new PropertyMetadata(.0));

		static void recalc(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var pb = (d as ProgressArcUsrCtrl);
			if (pb.MaxVal == 0.0) return;
			pb.PgrDeg = 360.0 * pb.CurVal / pb.MaxVal;
		}
	}
}
