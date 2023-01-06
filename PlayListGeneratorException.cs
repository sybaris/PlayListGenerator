using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlayListGenerator
{
    [Serializable]
    public class PlayListGeneratorException : Exception
    {
        public PlayListGeneratorException()
        {
        }

        public PlayListGeneratorException(string? message) : base(message)
        {
        }

        public PlayListGeneratorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PlayListGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
