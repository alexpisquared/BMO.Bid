using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMO.OLP.UenMatcher.Model
{
	public class WordStat
	{
		public WordStat(string w)
		{
			Word = w;
			Usage = 0;
		}

		public WordStat(string w, int c)
		{
			Word = w;
			Usage = c;
		}

		public string Word { get; set; }
		public int Usage { get; set; }
		public short Length { get { return (short)Word.Length; } }
	}
}

