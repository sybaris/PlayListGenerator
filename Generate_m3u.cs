using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayListGenerator
{
    /// <summary>
    /// Generate a m3u playlist file
    /// </summary>
    class Generate_m3u : GeneratePlaylistBase
    {

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
