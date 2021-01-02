namespace BMO.OLP.Common
{
	public partial class AppUserSettings
	{
		public string ALT_TYPE1 { get; set; }
		public string MapFilter { get; set; }
		public bool IsSpdmtrVis { get; set; }
		public bool IsAudibleOn { get { return _IsAudibleOn; } set { _IsAudibleOn = value; } }		bool _IsAudibleOn = true;
		public int UiRowLimit { get { return _UiRowLimit; } set { _UiRowLimit = value; } }				int _UiRowLimit = 1000;
		public string MapBasisId { get; set; }
	}
}
