using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlayListGenerator
{
    public class ExtractDirectoryAndMaskException : Exception
    {
        public ExtractDirectoryAndMaskException()
        {
        }

        public ExtractDirectoryAndMaskException(string? message) : base(message)
        {
        }

        public ExtractDirectoryAndMaskException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ExtractDirectoryAndMaskException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
