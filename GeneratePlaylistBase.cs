using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlayListGenerator
{
    /// <summary>
    /// Base class to generate a playlist
    /// </summary>
    internal abstract class GeneratePlaylistBase
    {
        
        /// <summary>
        /// Return the file extension for the playlist.
        /// </summary>
        /// <returns></returns>
        public abstract string FileExtension
        {
            get;
        }
        
        /// <summary>
        /// Method that will be called to generate the playlist file
        /// </summary>
        /// <param name="aPlayListFileName">Filename of the playlist file that will be generated</param>
        /// <param name="aFileToInclureInThePlayList">List of filename that will compose the playlist</param>
        public virtual void GeneratePlayList(string aPlayListFileName, IEnumerable<string> aFileToInclureInThePlayList)
        {
            // Content of the playlist file
            StringBuilder fileContent = new StringBuilder();
            
            // Add the header
            fileContent.Append(GetHeader());

            // Loop on each files to include in the playlist
            foreach (var fileName in aFileToInclureInThePlayList)
                fileContent.Append(GetForOneFile(fileName));
            
            // Add the footer
            fileContent.Append(GetFooter());

            // Generate the playlist file
            File.WriteAllText(aPlayListFileName, fileContent.ToString(), Encoding.UTF8);
        }

        /// <summary>
        /// Header of the playlist. Specific on the file format.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetHeader();

        /// <summary>
        /// Footer of the playlist. Specific on the file format.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetFooter();
        
        /// <summary>
        /// Format for 1 file to include in the playlist. Specific on the file format.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetForOneFile(string aFileName);

    }
}
