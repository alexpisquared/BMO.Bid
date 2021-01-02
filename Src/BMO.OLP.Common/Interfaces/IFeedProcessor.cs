namespace BMO.OLP.Interfaces
{
	public interface IFeedProcessor
	{
		int MaxFpm { get; set; }
		int CurFpm { get; set; }
		string PrgLog { get; set; }
	}
}
