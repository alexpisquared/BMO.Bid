using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMO.OLP.Common.Enums
{
	public class MatchBasis
	{
		public static MatchBasis Name = new MatchBasis("n");
		public static MatchBasis Core = new MatchBasis("c");
		public static MatchBasis PCor = new MatchBasis("p");
		public static MatchBasis UnKn = new MatchBasis("u");

		private MatchBasis(string name) { Id = name; }
		public string Id { get; private set; }
		public override string ToString() { return Id; }
	}
}
