using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMO.OLP.Common
{
  public class OlpPresets
  {
    public const bool IgnoreCase = true;
    const string _delimStr = " !?.,;:^=-+*()[]<>&$_/\\@#'`\"\r\n\t";
    static char[] _delimAry =_delimStr.ToArray();

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