using System;

namespace BMO.OLP.Common.Enums
{
	// [Obsolete] :why Copilot decides to mark it such?
	public class MatchStatus
	{
		public static MatchStatus NoMatchFound = new MatchStatus("m0");
		public static MatchStatus PerfectMatch = new MatchStatus("m1");
		public static MatchStatus MatchesFound = new MatchStatus("mm");
		public static MatchStatus PendngNewUEN = new MatchStatus("nu");
		public static MatchStatus PendgPMAprvl = new MatchStatus("pm");
		public static MatchStatus PendingNewPM = new MatchStatus("np");
		public static MatchStatus PendgStorage = new MatchStatus("ds");
		public static MatchStatus WorkFlowDone = new MatchStatus("zz");

		private MatchStatus(string name) { Id = name; }
		public string Id { get; private set; }
		public override string ToString() { return Id; }
	}
}
