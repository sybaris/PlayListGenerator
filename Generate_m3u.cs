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
            return "#EXTM3U"+Environment.NewLine;
        }

        protected override string GetFooter()
        {
            return "";
        }

        protected override string GetForOneFile(string aFileName)
        {
            return aFileName + Environment.NewLine;
        }
    }
}
