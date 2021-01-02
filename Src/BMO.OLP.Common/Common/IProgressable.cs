
namespace BMO.OLP.Common
{
	public interface IProgressable
	{
		void UpdateProgress(int curIdx);
		void ShowMessage(string msg);

		bool IsProgressing { get; set; }
		int MapProgress { get; set; }
		int ActualMax1 { get; set; }
		string TitleText { get; set; }
	}
}