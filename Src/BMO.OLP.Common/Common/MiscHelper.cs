using System;
using System.Diagnostics;

namespace BMO.OLP.Common
{
	public class MiscHelper
	{
		public static string CurrentUser { get { return string.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName); } }
		public static string ProgressReport(Stopwatch sw, int cur, int ttl, string details = "")
		{
			if (cur == 0 || sw.Elapsed.TotalSeconds == 0) return "";

			var s = string.Format("{0,12:N0} /{1,7:N1} ={2,6:N0}  ~>{3,8:N1} @ {4:ddd HH:mm}    {5}", cur, sw.Elapsed.TotalMinutes, cur / sw.Elapsed.TotalMinutes, sw.Elapsed.TotalMinutes * (ttl - cur) / cur, DateTime.Now.AddMinutes(sw.Elapsed.TotalMinutes * (ttl - cur) / cur), details);
			//Trace.WriteLine(s);
			return s;
		}
		public static string ProgressReportHeader()
		{
			var s = string.Format("{0,12} /{1,7} ={2,4}  ~>{3,8} @ ETA            Saved/sec", "Rows", "t (s)", "r/s", "Left (min)");
			//Trace.WriteLine(s);
			return s;
		}
		public static DateTime MinuteRoundedNow()
		{
			var n = DateTime.Now;
			var m = new DateTime(n.Year, n.Month, n.Day, n.Hour, n.Minute, 0);
			return m;
		}
		public const int MinWordUsage = 1024;	// minimal acceptable word usage to participate in partial name comparing. 
	}
}
