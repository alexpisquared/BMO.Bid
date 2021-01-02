using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OLP.DAQ.Views
{
  public partial class ProgressColUsrCtrl : UserControl
  {
    public ProgressColUsrCtrl() { InitializeComponent(); DataContext = this; }

    public double MaxVal { get { return (double)GetValue(MaxValProperty); } set { SetValue(MaxValProperty, value); } }
    public static readonly DependencyProperty MaxValProperty = DependencyProperty.Register("MaxVal", typeof(double), typeof(ProgressColUsrCtrl), new PropertyMetadata(100.0));
    public double CurVal { get { return (double)GetValue(CurValProperty); } set { SetValue(CurValProperty, value); } }
    public static readonly DependencyProperty CurValProperty = DependencyProperty.Register("CurVal", typeof(double), typeof(ProgressColUsrCtrl), new PropertyMetadata(.0, new PropertyChangedCallback(recalc)));
    public double PgrDeg { get { return (double)GetValue(PgrDegProperty); } set { SetValue(PgrDegProperty, value); } }
    public static readonly DependencyProperty PgrDegProperty = DependencyProperty.Register("PgrDeg", typeof(double), typeof(ProgressColUsrCtrl), new PropertyMetadata(.0));

    static void recalc(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var pb = (d as ProgressColUsrCtrl);
      if (pb.MaxVal == 0.0) return;
      pb.PgrDeg = pb.ActualHeight * pb.CurVal / pb.MaxVal;


      if (pb.CurVal == OlpPresets.Failed)
        pb.progressRect.Fill = new SolidColorBrush(Color.FromArgb(64, 255, 0, 64));
      else if (pb.CurVal == OlpPresets.Skipped)
        pb.progressRect.Fill = new SolidColorBrush(Color.FromArgb(32, 255, 255, 0));
      else
      if (pb.CurVal == pb.MaxVal)
        pb.progressRect.Fill = new SolidColorBrush(Color.FromArgb(64, 16, 255, 48));
    }
  }

  public class OlpPresets
  {
    public const bool IgnoreCase = true;
    const string _delimStr = " !?.,;:^=-+*()[]<>&$_/\\@#'`\"\r\n\t";
    static char[] _delimAry = _delimStr.ToArray();

    public static string DelimStr { get { return _delimStr; } }
    public static char[] DelimAry { get { return _delimAry; } }


    public static bool StripMinzeCompare(string s1, string s2)
    {
      return string.Compare(OlpPresets.StripMinze(s1), OlpPresets.StripMinze(s2), true) == 0;
    }
    public static string StripMinze(string sIn)
    {
      if (sIn == null) return null;

      var sOut = sIn.Trim(OlpPresets.DelimAry).ToLower();
      if (sOut.Contains('&'))
      {
        sOut = sOut.Replace(" & ", " and ");
        sOut = sOut.Replace("&", " and ");
      }

      if (OlpPresets.DelimAry.Intersect(sOut).Any())
        foreach (var delim in OlpPresets.DelimAry)
          sOut = sOut.Replace(delim.ToString(), "");

      Debug.WriteLine("{0,32} => {1}", sIn, sOut);
      return sOut;
    }


    public const string EntTypeAdp = "ADAPTIV";
    public const string EntTypeBbg = "BBG";
    public const string ObligorLnk = "OwlApp";
    public const string JobSkipped = "° Skipped since marked as inactive.";
    public const int Skipped = -8711;
    public const int Failed = -7419;

    public const string HeartbitPeriodSec = "HeartbitPeriodSec";
    public const string HeartbitTaskCtrl = "HeartbitTaskCtrl";
    public const string LastHeartbit = "LastHeartbit";
    public const string ClosingTime = "ClosingTime";
    public const string DTOrchestration = "DT.Orchestration";
    public const string UnderContrn = "Sorry, this feature is\r\n   under construction.";
  }

}
