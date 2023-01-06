using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            using (CommandLine.Parser parser = new CommandLine.Parser(
                with =>
                {
                    //with.HelpWriter = Console.Error;
                    with.HelpWriter = null; // To display myself the help
                    with.IgnoreUnknownArguments = false;
                    with.CaseSensitive = false;
                }
                ))
            {
                // Make the parsing
                ParserResult<CommandLineArguments> result = parser.ParseArguments<CommandLineArguments>(args);

                // In function of parser result, we run the program of we handle the errors
                result.WithParsed<CommandLineArguments>(opts => Run(opts))
                      .WithNotParsed<CommandLineArguments>((errs) => HandleParseError(errs, result)
                    );
            }
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
            //Console.WriteLine("=============================================");
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
            if (args.CheckMode)
                RunCheckPlayList(args);
            else
                RunGeneratePlayList(args);
        }

        /// <summary>
        /// Check if the playlist file is correct
        /// </summary>
        /// <param name="aPlayListFilename">Name of the playlist to check</param>
        /// <returns>true = playlist is correct, false = a problem has been found</returns>
        private static bool RunCheckOneFile(string aPlayListFilename)
        {
            Debug.Assert(File.Exists(aPlayListFilename));
            var check_m3u = new Check_m3u();
            var result = check_m3u.CheckM3uFile(aPlayListFilename);
            if (!string.IsNullOrEmpty(result))
            {
                Console.WriteLine($"KO - File {aPlayListFilename} have this problem : {result}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Run the program with the parsed arguments of the command line to generate a playlist 
        /// </summary>
        /// <param name="args"></param>
        private static void RunCheckPlayList(CommandLineArguments args)
        {
            string playListFilename = args.PathAndMask;
            if (File.Exists(playListFilename))
            {
                var result = RunCheckOneFile(playListFilename);
                if (result)
                    Console.WriteLine($"All ok - {playListFilename}");
            }
            else if (args.Recursive)
            {
                // Separate directory and file mask
                var dirAndMask = PathHelper.ExtractDirectoryandMask(playListFilename);
                string directory = dirAndMask.Item1;
                string mask = dirAndMask.Item2;

                var playLists = Directory.GetFiles(directory, mask, SearchOption.AllDirectories);
                int count = playLists.Length;
                int countError = 0;
                foreach (var playlist in playLists)
                {
                    var result = RunCheckOneFile(playlist);
                    if (!result)
                        countError++;
                }

                if (countError == 0)
                    Console.WriteLine($"All the {count} files are ok.");
                else
                    Console.WriteLine($"{countError}/{count} files have a problem.");
            }
            else
                Console.WriteLine($"File {playListFilename} to check was not found");
        }

        /// <summary>
        /// Run the program with the parsed arguments of the command line to generate a playlist 
        /// </summary>
        /// <param name="args"></param>
        private static void RunGeneratePlayList(CommandLineArguments args)
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

            args.PlayListFilename = string.IsNullOrEmpty(args.PlayListFilename) ? $"default.{playlistGenerator.FileExtension}" : args.PlayListFilename;

            // Separate directory and file mask
            var dirAndMask = PathHelper.ExtractDirectoryandMask(args.PathAndMask);
            string directory = dirAndMask.Item1;
            string mask = dirAndMask.Item2;

            if (string.IsNullOrEmpty(directory))
                directory = Path.GetPathRoot(args.PlayListFilename);
            if (string.IsNullOrEmpty(directory))
                directory = ".";

            // In fonction of OnePlaylistByFolder option treatment is different
            if (!args.OnePlaylistByFolder)
            {
                // Only 1 playlist file will be generated
                RunGeneratePlayList(playlistGenerator, directory, mask, Path.Combine(directory, args.PlayListFilename), args.RelativePath, args.Recursive, args.MinimumSongByPlaylist, args.SkipIfFileAlreadyExists, args.NumericSort);
            }
            else
            {
                // Several playlists files will be generated (1 by folder)
                List<string> directories = new List<string>(Directory.EnumerateDirectories(directory));
                foreach (var dir in directories)
                {
                    if (args.UseCurrentFolderAsPlaylistName)
                    {
                        string playlistFilename = $"{dir.Split(Path.DirectorySeparatorChar).Last()}{args.PlayListSuffix}.{playlistGenerator.FileExtension}";
                        args.PlayListFilename = Path.Combine(Path.GetFullPath(dir), playlistFilename);
                    }

                    // Generate playlist of the folder
                    RunGeneratePlayList(playlistGenerator, dir, mask, Path.Combine(dir, args.PlayListFilename), args.RelativePath, args.Recursive, args.MinimumSongByPlaylist, args.SkipIfFileAlreadyExists, args.NumericSort);
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
        /// <param name="minimumSong">Minimum songs to be included in playlist</param>
        /// <param name="aSkipIfFileAlreadyExists">If file already exists, do not overwrite it</param>
        private static void RunGeneratePlayList(GeneratePlaylistBase aPlaylistGenerator, string aDirectory, string aMask, string aPlayListFilename, bool aRelativePath, bool aRecursive, int minimumSong, bool aSkipIfFileAlreadyExists, bool aNumericPrefixSort)
        {
            if (aSkipIfFileAlreadyExists && File.Exists(aPlayListFilename))
            {
                // Display on the console to inform user
                Console.WriteLine($"Skipped : Playlist \"{aPlayListFilename}\" because file already exists");
                return;
            }

            // List the file to include in the playlist
            List<string> files = new List<string>(Directory.EnumerateFiles(aDirectory, aMask, ConvertToSearchOption(aRecursive)));

            //In case of relative paths, retreat the path of each founded filenames
            if (aRelativePath)
            {
                var referencePath = Path.GetFullPath(aDirectory);
                referencePath = referencePath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                for (int i = 0; i < files.Count; i++)
                    files[i] = PathHelper.PathSlashToBackslash(PathHelper.MakeRelative(files[i], referencePath));
            }

            if (files.Count < minimumSong)
            {
                // Display on the console to inform user
                Console.WriteLine($"Minimun : Playlist \"{aPlayListFilename}\" not generated because threshold not reached");
                return;
            }

            // Sort files
            if (aNumericPrefixSort)
                files.Sort(new NumericPrefixSortComparer());

            // Generate the playlist file
            aPlaylistGenerator.GeneratePlayList(aPlayListFilename, files);

            // Display a result on the console to inform user
            Console.WriteLine($"Generated : Playlist \"{aPlayListFilename}\" with \"{files.Count}\" files from directory \"{aDirectory}\"");
        }
    }
}

