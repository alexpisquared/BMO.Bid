using BMO.OLP.Common;
using BMO.OLP.UenMatcher.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace BMO.OLP.UenMatcher
{
	public class StatsBasedParser
	{
		List<WordStat> _wordStatCollection = new List<WordStat>();
		List<string> _trgList;

		public StatsBasedParser(List<string> trgList)
		{
			_trgList = trgList;
			var wsl = new WordStatLoader_Dict();
			wsl.LoadFromStrings(trgList);

			_wordStatCollection.Clear();
			foreach (var w in wsl.WordUsageDic.Keys) _wordStatCollection.Add(new WordStat(w, wsl.WordUsageDic[w]));
		}
		public void loadStatsFromNetezzaXxxRows_NOTUSED(int limit)
		{
			var wsl = new WordStatLoader_Dict();
			wsl.LoadFromNetezza_Timed(limit);

			_wordStatCollection.Clear();
			foreach (var w in wsl.WordUsageDic.Keys) _wordStatCollection.Add(new WordStat(w, wsl.WordUsageDic[w]));
		}

		public List<string> ClosestMatches(string src)
		{
			//wp>Debug.WriteLine(src, "\n::");

			var matches = new List<string>();

			//0: if there is an exact match:
			if (_trgList.Contains(src, StringComparer.OrdinalIgnoreCase)) //tu: !!!!!!!!! //todo: remove ToLower from word stats view !!!!!!!
			{
				var exactMatch = _trgList.FirstOrDefault(r => string.Compare(r, src, true) == 0);
				MatchBaseCsv = exactMatch;
				matches.Add(exactMatch);
				return matches;
			}

			//1a: order by FoU:
			var wa = src.Split(OlpPresets.DelimAll, StringSplitOptions.RemoveEmptyEntries);

			var srcWordsByFoU = GetWordStats(wa, _wordStatCollection);

			//1: if there is a SINGLE match by a single word with Fou==1:
			if (srcWordsByFoU.Count(r => r.Usage == 1) == 1)
			{
				var singleUseWord = srcWordsByFoU.First(r => r.Usage == 1);
				var trgMatch = _trgList.First(r => r.ToLower().Contains(singleUseWord.Word.ToLower()));
				matches.Add(trgMatch);
				MatchBaseCsv = string.Join("·", srcWordsByFoU.Select(r => r.Word));
				return matches;
			}

			//1: if there is more than 1 match by a single word with Fou==1:
			if (srcWordsByFoU.Count(r => r.Usage == 1) > 1)
			{
				foreach (var matchByFoU1 in srcWordsByFoU.Where(r => r.Usage == 1))
				{
					if (!matches.Contains(matchByFoU1.Word, StringComparer.OrdinalIgnoreCase))
						matches.Add(matchByFoU1.Word);
				}
				Debug.Assert(matches.Count == 1, "Need more wits to find a better way to tell which one matches");
				MatchBaseCsv = string.Join("·", srcWordsByFoU.Select(r => r.Word));
				return matches;
			}

			//3: if contains all the words from src name, ignoring the org order
			var matchByAllWords = _trgList.Where(r => StatsBasedParser.ContainsAllWordsOpt(r, wa));
			if (matchByAllWords.Any())
			{
				MatchBaseCsv = string.Join("·", wa);
				return matchByAllWords.ToList();
			}

			//3: if contains the words from src name and stats array
			if (srcWordsByFoU.Count() > 1)
			{
				for (int i = srcWordsByFoU.Count(); i > 0; i--)
				{
					var notAllOrdered = srcWordsByFoU.OrderBy(r => r.Usage).Take(i).Select(r => r.Word).ToArray();

					var matchBy1stWords = _trgList.Where(r => StatsBasedParser.ContainsAllWordsOpt(r, notAllOrdered));
					if (matchBy1stWords.Any())
					{
						MatchBaseCsv = string.Join("·", notAllOrdered);
						return matchBy1stWords.ToList();
					}
				}
			}

			return matches;
		}

		public static IOrderedEnumerable<WordStat> GetWordStats(string[] wordsFromName1, IEnumerable<WordStat> wordStatsFromLst2, int maxFoU = int.MaxValue)
		{
			var srcWordsByFoU = new List<WordStat>();

			foreach (var w in wordsFromName1)
				if (wordStatsFromLst2.Any(r => r.Usage < maxFoU && string.CompareOrdinal(r.Word, w) == 0))
					srcWordsByFoU.Add(wordStatsFromLst2.FirstOrDefault(r => r.Usage < maxFoU && string.CompareOrdinal(r.Word, w) == 0));

			//todo: need a case to justify:
			//if (srcWordsByFoU.Count() < 1 && wordsFromName1.Length > 1)
			//{
			//	foreach (var w in wordsFromName1)
			//		if (wordStatsFromLst2.Any(r => r.Usage < 10 * maxFoU && string.CompareOrdinal(r.Word, w) == 0))
			//			srcWordsByFoU.Add(wordStatsFromLst2.FirstOrDefault(r => r.Usage < 10 * maxFoU && string.CompareOrdinal(r.Word, w) == 0));

			//	if (srcWordsByFoU.Count() < 2) // must have at least 2 hight FoU words to work.
			//		srcWordsByFoU.Clear();
			//}

			Debug.WriteLine("\t matching words from Lst2 with stats:"); foreach (var w in srcWordsByFoU.OrderBy(r => r.Usage)) Debug.WriteLine("\t\t ## Usage: {0,6} - '{1}'", w.Usage, w.Word);

			return srcWordsByFoU.OrderBy(r => r.Usage);
		}

		public static void TestParser(string[] srcNameList, List<string> trgNameList)
		{
			var sbp = new StatsBasedParser(trgNameList);
			//int z = 0;			foreach (var srcName in srcNameList)			{				Debug.WriteLine("{0}", ++z);				foreach (var m in sbp.ClosestMatches(srcName))					Debug.WriteLine("{0,64} => {1}", srcName, m);			}
			if (Debugger.IsAttached) Debugger.Break();
		}

		public static bool ContainsAllParts(string name, string[] parts)
		{
			foreach (var word in parts)
				if (!name.Contains(word))
					return false;

			return true;
		}
		public static bool ContainsAllWordsOpt(string name, string[] words)
		{
			foreach (var word in words)
				if (!name.Contains(word))  //tu: !!! if (name.IndexOf(word) < 0)  IS 2.5 TIMES SLOWER THEN  if (!name.Contains(word)) !!!
					return false;
				else if (!Regex.Match(name, string.Format("{0}{1}{0}", @"\b", word), RegexOptions.IgnoreCase).Success)
					return false;

			return true;
		}
		public static bool ContainsAllWords(string name, string[] words)
		{
			foreach (var word in words)
				if (!Regex.Match(name, string.Format("{0}{1}{0}", @"\b", word), RegexOptions.IgnoreCase).Success)
					return false;

			return true;
		}
		public static bool ContainsWholeWord(string name, string word)
		{
			bool contains = Regex.Match(name, string.Format("{0}{1}{0}", @"\b", word), RegexOptions.IgnoreCase).Success;
			return contains;
		}


		public IEqualityComparer<string> eqcmpr()
		{
			return null; // new IEqualityComparer<string>();
		}

		public string MatchBaseCsv { get; set; }
	}
}
