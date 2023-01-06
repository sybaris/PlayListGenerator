using System;

namespace PlayListGenerator
{
    /// <summary>
    /// Generate a m3u playlist file
    /// </summary>
    class Generate_m3u : GeneratePlaylistBase
    {

        public override string FileExtension
        {
            get
            {
                return "m3u";
            }
        }

        protected override string GetHeader()
        {
            return "#EXTM3U" + Environment.NewLine;
        }

        protected override string GetFooter()
        {
            return "";
        }

        private static string Convert(string stringToConvert)
        {
            // According to https://trac.videolan.org/vlc/ticket/21364?cversion=0&cnum_hist=2 if a # char is found in filename, we have to replace by %23
            return stringToConvert.Replace("#", "%23");
        }

        protected override string GetForOneFile(string aFileName)
        {
            return Convert(aFileName) + Environment.NewLine;
        }
    }
}
