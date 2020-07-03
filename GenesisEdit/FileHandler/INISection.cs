using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GenesisEdit.FIleHandler
{
	internal class INISection
	{
		public string Header { get; private set; } = string.Empty;
		public Dictionary<string, string> Pairs { get; private set; }  = new Dictionary<string, string>();

		public INISection(string header, Dictionary<string, string> values)
		{
			Header = header ?? throw new ArgumentNullException(nameof(header));
			Pairs = values ?? throw new ArgumentNullException(nameof(values));
			if (Pairs.Count > 0 && Pairs.Any(kv => new string[] { "=", " " }.Any(s => kv.Key.Contains(s))))
			{
				throw new ArgumentException("Invalid values");
			}
		}

		public override string ToString()
		{
			string text = $"[{Header}]\n";
			foreach (KeyValuePair<string, string> kv in Pairs)
			{
				text += $"{kv.Key}={kv.Value}\n";
			}
			return text;

		}

		public static INISection FromText(string s)
		{
			if (!FileHandler.sectionRegex.IsMatch(s))
			{
				throw new ArgumentException("Invalid INI Section");
			}
			//Remove \r and split by \n
			string[] split = s.Replace("\r", string.Empty).Split('\n');
			int headerLoc = -1;
			for (int i = 0; i < split.Length; i++)
			{
				if (Utils.IsINIHeader(split[i]))
				{
					headerLoc = i;
					break;
				}
			}
			if (headerLoc == -1)
			{
				throw new InvalidOperationException("Invalid INI Section");
			}
			string head = Utils.RemovePadding(split[headerLoc]);
			INISection section = new INISection(head.Substring(1, head.Length - 2), new Dictionary<string, string>());
			for (int i = headerLoc + 1; i < split.Length; i++)
			{
				string line = Utils.RemovePadding(split[i]);
				//Comments
				if (line.StartsWith(";"))
				{
					continue;
				}
				if (Utils.IsINIHeader(line))
				{
					throw new InvalidOperationException("Cannot read multiple INI sections at once!");
				}	
				string[] lineSplit = line.Split('=');
				//Invalid kv pair
				if (lineSplit.Length < 2)
				{
					continue;
				}
				section.Pairs.Add(lineSplit.First(), string.Join("=", lineSplit.Skip(1)));
			}
			return section;
		}
	}
}