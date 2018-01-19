using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PlayListGenerator
{
    public static class PathHelper
    {

        /// <summary>
        /// Rebases file with path fromPath to folder with baseDir.
        /// </summary>
        /// <param name="_fromPath">Full file path (absolute)</param>
        /// <param name="_baseDir">Full base directory path (absolute)</param>
        /// <returns>Relative path to file in respect of baseDir</returns>
        public static String MakeRelative(String _fromPath, String _baseDir)
        {
            // From https://sourceforge.net/p/syncproj/code/HEAD/tree/syncProj.cs#l735
            String pathSep = "\\";
            String fromPath = Path.GetFullPath(_fromPath);
            String baseDir = Path.GetFullPath(_baseDir);            // If folder contains upper folder references, they gets lost here. "c:\test\..\test2" => "c:\test2"

            String[] p1 = Regex.Split(fromPath, "[\\\\/]").Where(x => x.Length != 0).ToArray();
            String[] p2 = Regex.Split(baseDir, "[\\\\/]").Where(x => x.Length != 0).ToArray();
            int i = 0;

            for (; i < p1.Length && i < p2.Length; i++)
                if (String.Compare(p1[i], p2[i], true) != 0)    // Case insensitive match
                    break;

            if (i == 0)     // Cannot make relative path, for example if resides on different drive
                return fromPath;

            String r = String.Join(pathSep, Enumerable.Repeat("..", p2.Length - i).Concat(p1.Skip(i).Take(p1.Length - i)));
            return r;
        }

        /// <summary>
        /// Separate the directory and the file mask
        /// </summary>
        /// <param name="aPathAndMask">for example : "d:\foo\*.bar"</param>
        /// <returns>for example : return a tuple with "d:\foo" and "*.bar"</returns>
        public static Tuple<string, string> ExtractDirectoryandMask(string aPathAndMask)
        {
            string directory = aPathAndMask;
            string mask = "*.*";
            if (!Directory.Exists(directory))
            {
                directory = Path.GetDirectoryName(directory);
                if (!Directory.Exists(directory) && directory != "")
                    throw new Exception("Wrong Path : " + aPathAndMask);
                mask = aPathAndMask.Remove(0, directory.Length);
                if (mask.StartsWith(Path.DirectorySeparatorChar.ToString()))
                    mask = mask.Remove(0, 1);
            }
            if (!(mask.Contains('*') || mask.Contains('?')))
                throw new Exception("Wrong Mask : " + aPathAndMask);
            return new Tuple<string, string>(directory, mask);
        }
    }
}
