using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayListGenerator
{
    public class Check_m3u
    {
        public string CheckM3uFile(string aFilename)
        {
            var allText = File.ReadAllLines(aFilename);
            if (allText.Count() <= 0)
                return "Empty file";
            if (!allText[0].Contains("#EXTM3U"))
                return "Header #EXTM3U not found in file";
            if (allText.Count() <= 1)
                return "The playlist is empty";

            string directory = Path.GetFullPath(Path.GetDirectoryName(aFilename));
            string mask = Path.Combine(Path.GetFileName(aFilename));

            for (int i = 1; i < allText.Count(); i++)
            {
                string s = allText[i].Trim();
                if (string.IsNullOrEmpty(s) || s.StartsWith("#"))
                    continue;
                string file = Path.Combine(directory, s);
                if (!File.Exists(s) && !File.Exists(file))
                    return $"The file \"{s}\" is not found";
            }
            return "";
        }
    }
}
