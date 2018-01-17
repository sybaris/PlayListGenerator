using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using System.Diagnostics;
using CommandLine.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace PlayListGenerator
{
    class Program
    {
        /// <summary>
        /// Main entry point of the program
        /// </summary>
        /// <param name="args">Command line arguments to be parsed</param>
        static void Main(string[] args)
        {
            // We create your own command line parser to setup wanted options
            CommandLine.Parser parser = new CommandLine.Parser(
                with =>
                {
                    with.HelpWriter = Console.Error;
                    //with.HelpWriter = null; // To display myself the help
                    with.IgnoreUnknownArguments = false;
                }
                );

            // Make the parsing
            ParserResult<CommandLineArguments> result = parser.ParseArguments<CommandLineArguments>(args);

            // In function of parser result, we run the program of we handle the errors
            result.WithParsed<CommandLineArguments>(opts => Run(opts))
                  .WithNotParsed<CommandLineArguments>((errs) => HandleParseError(errs, result)
                );

            // Only for debug, when the program is launch under Visual Studio.
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        private static void HandleParseError(IEnumerable<Error> errs, ParserResult<CommandLineArguments> parsingResult)
        {
            // Display a customized format of my Help
            Console.WriteLine("=============================================");
            HelpText myHelpText = HelpText.AutoBuild(parsingResult, onError =>
            {
                HelpText nHelpText = new HelpText(SentenceBuilder.Create());
                nHelpText.Heading = HeadingInfo.Default;
                nHelpText.Copyright = CopyrightInfo.Default;
                nHelpText.AdditionalNewLineAfterOption = false; // avoid the default behavior (new line between every options)
                nHelpText.AddDashesToOption = true;
                nHelpText.MaximumDisplayWidth = 4000;
                nHelpText.AddOptions(parsingResult);
                return HelpText.DefaultParsingErrorsHandler(parsingResult, nHelpText);
            },
               onExample => { return onExample; });
            Console.Error.WriteLine(myHelpText);
        }

        /// <summary>
        /// Convert a bool to SearchOption
        /// </summary>
        /// <param name="aRecursive">Want to recurse into all tree of subdirectories</param>
        /// <returns>SearchOption.AllDirectories if aRecursive else SearchOption.TopDirectoryOnly</returns>
        private static SearchOption ConvertToSearchOption(bool aRecursive)
        {
            if (aRecursive)
                return SearchOption.AllDirectories;
            else
                return SearchOption.TopDirectoryOnly;
        }

        /// <summary>
        /// Run the program with the parsed arguments of the command line
        /// </summary>
        /// <param name="args"></param>
        private static void Run(CommandLineArguments args)
        {
            // Create the file generator
            GeneratePlaylistBase playlistGenerator;
            switch (args.Format)
            {
                case CommandLineArguments.FormatEnum.m3u:
                    playlistGenerator = new Generate_m3u();
                    break;
                case CommandLineArguments.FormatEnum.xspf:
                    playlistGenerator = new Generate_xspf();
                    break;
                default:
                    throw new Exception("Unkown playlist format");
            }

            // Separate directory and file mask
            var dirAndMask = PathHelper.ExtractDirectoryandMask(args.PathAndMask);
            string directory = dirAndMask.Item1;
            string mask = dirAndMask.Item2;

            // In fonction of OnePlaylistByFolder option treatment is different
            if (!args.OnePlaylistByFolder)
            {
                // Only 1 playlist file will be generated
                Run(playlistGenerator, directory, mask, Path.Combine(directory, args.PlayListFilename), args.RelativePath, args.Recursive);
            }
            else
            {
                // Several playlists files will be generated (1 by folder)
                List<string> directories = new List<string>(Directory.EnumerateDirectories(directory));
                foreach (var dir in directories)
                {
                    // Generate playlist of the folder
                    Run(playlistGenerator, dir, mask, Path.Combine(dir, args.PlayListFilename), args.RelativePath, args.Recursive);
                }
            }
        }

        /// <summary>
        /// Generate 1 PlayList file
        /// </summary>
        /// <param name="aPlaylistGenerator">A class that implement the GeneratePlayList method to generate in function of the file format the wanted file</param>
        /// <param name="aDirectory">Directory from where we work</param>
        /// <param name="aMask">for example *.mp3</param>
        /// <param name="aPlayListFilename">name of the playlist file</param>
        /// <param name="aRelativePath">Works with relative or absolute paths</param>
        /// <param name="aRecursive">Include subfolders or not</param>
        private static void Run(GeneratePlaylistBase aPlaylistGenerator, string aDirectory, string aMask, string aPlayListFilename, bool aRelativePath, bool aRecursive)
        {
            // List the file to include in the playlist
            List<string> files = new List<string>(Directory.EnumerateFiles(aDirectory, aMask, ConvertToSearchOption(aRecursive)));

            //In case of relative paths, retreat the path of each founded filenames
            if (aRelativePath)
            {
                var referencePath = Path.GetFullPath(aDirectory);
                referencePath = referencePath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                for (int i = 0; i < files.Count; i++)
                    files[i] = PathHelper.MakeRelative(files[i], referencePath);
            }

            // Generate the playlist file
            aPlaylistGenerator.GeneratePlayList(aPlayListFilename, files);

            // Display a result on the console to inform user
            Console.WriteLine(string.Format("Playlist \"{0}\" generated with \"{1}\" files", aPlayListFilename, files.Count));
        }

    }
}

