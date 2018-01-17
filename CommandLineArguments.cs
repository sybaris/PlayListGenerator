using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayListGenerator
{


    /// <summary>
    /// Class that will be used as a result of command line parsing using library CommandLineParser (See also on nuget package)
    /// </summary>
    internal class CommandLineArguments
    {
        /*
            M3U : Format de liste de lecture basique développé par Winamp.
            PLS : Format de liste de lecture utilisé par WinAmp et itunes.
            XSPF : Format de liste de lecture basé sur le XML opensource soutenu par xiph.org
            ASX : Format de liste de lecture basé sur le XML développé par Microsoft pour Windows Media.
            WPL : Format de liste de lecture basé sur le XML développé par Microsoft pour Windows Media 9 et plus.         
         */
        internal enum FormatEnum { m3u, xspf }

        [Value(0, HelpText = "[drive:][path][filename_or_mask]", Required = true)]
        public string PathAndMask { get; set; }
        [Value(1, HelpText = "[filename]", Required = true)]
        public string PlayListFilename { get; set; }
        [Option('S', "SubDirectories", HelpText = "Include subdirectories (recursive)")]
        public bool Recursive { get; set; }
        [Option('F', "Format", HelpText = "Format of the generated playlist : m3u or xspf", Default = FormatEnum.m3u)]
        public FormatEnum Format { get; set; }
        [Option('R', "RelativePath", HelpText = "Generate relative paths.")]
        public bool RelativePath { get; set; }

        [Option('O', "OnePlaylistByFolder", HelpText = "Create 1 playlist by folder of the current folder")]
        public bool OnePlaylistByFolder { get; set; }

        [Usage()]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Normal scenario", new CommandLineArguments { PathAndMask = @".\*.mp3", PlayListFilename = "myPlayList.m3u" });
                //      yield return new Example("Logging warnings", UnParserSettings.WithGroupSwitchesOnly() , new CommandLineArguments { InputFile = "file.bin", LogWarning = true });
                //      yield return new Example("Logging errors", new[] { UnParserSettings.WithGroupSwitchesOnly(), UnParserSettings.WithUseEqualTokenOnly() }, new CommandLineArguments { InputFile = "file.bin", LogError = true });
            }
        }
    }
}
