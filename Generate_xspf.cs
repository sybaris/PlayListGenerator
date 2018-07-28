using System;

namespace PlayListGenerator
{
    /// <summary>
    /// Generate a xspf playlist file
    /// </summary>
    internal class Generate_xspf : GeneratePlaylistBase
    {
        public override string FileExtension
        {
            get
            {
                return "xspf";
            }
        }

        protected override string GetHeader()
        {
            return @"<?xml version=""1.0"" encoding=""UTF-8""?><playlist version=""1"" xmlns=""http://xspf.org/ns/0/""><title></title><trackList>"+Environment.NewLine;
        }

        protected override string GetFooter()
        {
            return @"</track></trackList></playlist>";
        }

        protected override string GetForOneFile(string aFileName)
        {
            return "<track><location>" + EncodeStringToXML(aFileName) + "</location></track>"+Environment.NewLine;
        }

        private string EncodeStringToXML(string aStringToEncore)
        {
            // From http://weblogs.sqlteam.com/mladenp/archive/2008/10/21/Different-ways-how-to-escape-an-XML-string-in-C.aspx
            return aStringToEncore.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
