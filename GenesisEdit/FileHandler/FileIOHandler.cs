using GenesisEdit.Compiler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenesisEdit.FileHandler
{
	internal static class FileIOHandler
	{
		public static readonly Regex sectionRegex = new Regex("\\[[A-Z_]{1}([A-Z0-9_])*\\](\\r?\\n)(((([^\\r\\n= ]+=[^\\r\\n]+)|(\\s*;[^\\r\\n]*))(\\r?\\n)?))*", RegexOptions.Multiline | RegexOptions.IgnoreCase);

		public static void SaveFile(FileStream fs, ROMInfo info, List<Variable> vars, List<GenesisEvent> events)
		{
			INIFile file = new INIFile();
			//Write ROM Info
			file.AddSection(new INISection("Info", new Dictionary<string, string>()
			{
				{  "Title", info.Title },
				{  "AuthorAndDate", info.AuthorAndDate },
				{  "Subtitle", info.Subtitle },
				{  "Subtitle2", info.Subtitle2 },
				{  "ProductNo", info.ProductNo }
			}));
			//Write Variables
			Dictionary<string, string> iniVars = new Dictionary<string, string>();
			foreach (Variable v in vars)
			{
				iniVars.Add(v.Name, $"{v.GetVariableType().ToString().ToUpper()}|{v.Length}");
			}
			file.AddSection(new INISection("Variables", iniVars));
			//Write events
			foreach (GenesisEvent e in events)
			{
				file.AddSection(new INISection($"Events.{e.Name}", new Dictionary<string, string>()
				{
					{ "Code", $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(e.Code))}" },
					{ "Type", $"{e.Type}" },
					{ "Button", $"{e.Button}" }
				}));
			}
			//Write to file
			byte[] data = Encoding.UTF8.GetBytes(file.ToString());
			fs.Write(data, 0, data.Length);
			fs.Close();
		}
	}
}
