using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisEdit.FileHandler
{
	internal class INIFile
	{
		public List<INISection> Sections { get; private set; } = new List<INISection>();

		public INISection this[string name] => Sections.ToList().Find(s => s.Header.Equals(name));

		public IEnumerable<INISection> GetSubSections(string parent) => Sections.Where(s => s.Header.StartsWith(parent + "."));

		public void AddSection(INISection section)
		{
			section = section ?? throw new ArgumentNullException(nameof(section));
			Sections.Add(section);
		}

		public override string ToString()
		{
			string text = string.Empty;
			foreach (INISection s in Sections)
			{
				text += $"{s}\n";
			}
			return text;
		}

		public static INIFile FromFile(FileStream fs)
		{
			fs = fs ?? throw new ArgumentNullException(nameof(fs));
			string text = string.Empty;
			byte[] buf = new byte[1];
			while (fs.Read(buf, 0, buf.Length) > 0)
			{
				text += Encoding.UTF8.GetString(buf);
			}
			INIFile ini = new INIFile
			{
				Sections = Utils.MatchCollectionToList(FileIOHandler.sectionRegex.Matches(text)).Select(m => INISection.FromText(m.Value)).ToList()
			};
			fs.Close();
			return ini;
		}
	}
}
