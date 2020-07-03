using GenesisEdit.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenesisEdit
{
	internal static class Utils
	{
		// Regexes
		public static readonly Regex MACRO_IDENTIFIER = new Regex("%\\w+ ", RegexOptions.Multiline);
		private static readonly Regex SPLIT_BY_DOT = new Regex(".", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		private static readonly Regex SPLIT_BY_BACKSLASH_S = new Regex("\\s", RegexOptions.IgnoreCase | RegexOptions.Multiline);

		public static string[] SplitByRegex(Regex r, string s)
		{
			r = r ?? throw new ArgumentNullException(nameof(r));
			s = s ?? throw new ArgumentNullException(nameof(s));
			return r.Split(s);
		}

		public static string[] SplitByWhiteSpace(string s, bool includeNewLines = false)
		{
			s = s ?? throw new ArgumentNullException(nameof(s));
			return (includeNewLines ? SPLIT_BY_BACKSLASH_S : SPLIT_BY_DOT).Split(s);
		}
		// Makes colors vibrant when UseVibrantColors is enabled. Max brightness for the converted colorsis 224, so add 31 to make that 255.
		// Will if R, G, and B are the same and all less than it will not modify it as to not break grey colors.</para>
		// Does not effect the apperence in game!
		public static Color MakeVibrantColor(Color c)
		{
			// Get a list of lists of R G B values
			List<List<int>> colors = new List<List<int>>() { new List<int>() { c.R, c.G, c.B } };
			// Get a tuple of 2 conditions, if both are true then we consider it as grayscale
			Tuple<bool, bool> isGrayscale = colors.Select(l => new Tuple<bool, bool>(l.Distinct().Count() == 1, l.Sum() / 3 < 128)).First();
			return (Settings.Default.UseVibrantColors && !(isGrayscale.Item1 && isGrayscale.Item2)) ? Color.FromArgb(c.A, c.R + 31, c.G + 31, c.B + 31) : c;
		}

		public static Color FromUShort(ushort v)
		{
			// The genesis uses 16-bit 333 BGR
			// #$0BGR
			// 0b00000BBB0GGG0RRR
			byte r = (byte)((v & 0x000E) << 4);
			byte g = (byte)((v & 0x00E0) << 0);
			byte b = (byte)((v & 0x0E00) >> 4);
			return MakeVibrantColor(Color.FromArgb((v & 0x8000) >> 15 == 0 ? 255 : 0, r, g, b));
		}

		public static ushort FromColor(Color c)
		{
			int r = (c.R & 0xE0) >> 4;
			int g = (c.G & 0xE0) >> 0;
			int b = (c.B & 0xE0) << 4;
			return (ushort)(r | g | b);
		}

		public static string FormatEnum(string e) => string.Join(" ", e.Split('_').Select(s => s.Substring(0, 1).ToUpper() + (s.Length > 1 ? s.Substring(1).ToLower() : string.Empty)));

		public static List<Match> MatchCollectionToList(MatchCollection ms)
		{
			ms = ms ?? throw new ArgumentNullException(nameof(ms));
			List<Match> output = new List<Match>();
			foreach (Match m in ms)
			{
				output.Add(m);
			}
			return output;
		}

		public static bool IsINIHeader(string v)
		{
			string noPad = Utils.RemovePadding(v);
			return noPad.StartsWith("[") && noPad.EndsWith("]");
		}

		public static bool IsValidIdentifier(string name)
		{
			List<Func<string, bool>> funcs = new List<Func<string, bool>>()
			{
				new Func<string, bool>(s => s.StartsWith("GE_")),
				new Func<string, bool>(s => new Regex("[A-Z_]{1}[A-Z_0-9]*", RegexOptions.IgnoreCase).IsMatch(name) == false)
			};
			return !funcs.Any(f => f.Invoke(name));
		}

		public static IEnumerable<T> SkipLast<T>(IEnumerable<T> list, int num) => list.Reverse().Skip(num).Reverse();
		public static IEnumerable<T> TakeLast<T>(IEnumerable<T> list, int num) => list.Reverse().Take(num).Reverse();
		public static IEnumerable<T> SkipFirstAndLast<T>(IEnumerable<T> list, int num) => SkipLast(list.Skip(num), num);
		public static IEnumerable<T> TakeFirstAndLast<T>(IEnumerable<T> list, int num) => TakeLast(list.Take(num), num);

		private static string[] GetLines(string s) => s.Replace("\r", string.Empty).Split('\n');

		public static string FirstLine(string s)
		{
			string[] lines = GetLines(s);
			return lines.Count() > 0 ? lines.First() : null;
		}

		public static bool Validate(List<Func<bool>> funcs) => funcs.All(f => f.Invoke());
		public static bool Validate(params Func<bool>[] funcs) => Validate(funcs.ToList());

		public static string RemovePadding(string s) => new string(new string(s.SkipWhile(c => string.IsNullOrEmpty(c.ToString())).ToArray()).Reverse().SkipWhile(c => string.IsNullOrEmpty(c.ToString())).Reverse().ToArray()).Replace("\t", string.Empty);

		public static bool IsFullMatch(Regex r, string s) => r.Match(s).Value.Length == s.Length;
	}
}
