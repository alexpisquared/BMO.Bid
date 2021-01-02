
namespace BMO.OLP.Common.Enums
{
	public enum IdType { Cusip = 3, Isin = 5, Sedol = 7, Taec = 9 }

	public class SecIdTypePsEnum
	{
		public static SecIdTypePsEnum Cusip = new SecIdTypePsEnum("CUSIP", 3);
		public static SecIdTypePsEnum Isin = new SecIdTypePsEnum("ISIN", 5);
		public static SecIdTypePsEnum Sedol = new SecIdTypePsEnum("SEDOL", 7);
		public static SecIdTypePsEnum Taec = new SecIdTypePsEnum("TAEC", 9);

		private SecIdTypePsEnum(string name, int id) { Name = name; DbId = id; }
		public string Name { get; private set; }
		public int DbId { get; private set; }
		public override string ToString() { return Name; }

		//enum SecId { CUSIP = 3, ISIN = 5, SEDOL1 = 7 }
	}
}
