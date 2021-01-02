
namespace BMO.OLP.Common.Enums
{
	public class ApprovalStatus
	{
		public static ApprovalStatus UnknownS = new ApprovalStatus("U"); // Unknown State.
		public static ApprovalStatus NewMatch = new ApprovalStatus("N"); // New Match.
		public static ApprovalStatus Approved = new ApprovalStatus("A"); // Approved == Match.
		public static ApprovalStatus UserAprd = new ApprovalStatus("U"); // Approved == Match by the OL Studio User
		public static ApprovalStatus MisMatch = new ApprovalStatus("M"); // Missmatch by the match in the batch
		public static ApprovalStatus Rejected = new ApprovalStatus("R"); // Rejected == Mismatch.
		public static ApprovalStatus RejOther = new ApprovalStatus("O"); // Reject. Create new UEN under another connection.
		public static ApprovalStatus RejdSame = new ApprovalStatus("S"); // Reject. Create new UEN under the same connection.
		public static ApprovalStatus RWrongPM = new ApprovalStatus("W"); // Reject. I am not the right PM.

		private ApprovalStatus(string name) { Id = name; }
		public string Id { get; private set; }
		public override string ToString() { return Id; }
		public static bool IsApprovable(string aprStat)
		{
			var s =
				ApprovalStatus.Approved.Id +
				ApprovalStatus.MisMatch.Id;
			return !s.Contains(aprStat);
		}
		public static bool IsApprovable(ApprovalStatus aprStat)
		{
			return IsApprovable(aprStat.Id);
		}
	}
}
