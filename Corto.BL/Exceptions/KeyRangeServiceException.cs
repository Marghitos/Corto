using System;
using System.Runtime.Serialization;

namespace Corto.BL.Exceptions
{
    public class KeyRangeServiceException : Exception
    {
        public KeyRangeServiceException()
        {
        }

        public KeyRangeServiceException(string message) : base(message)
        {
        }

        public KeyRangeServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected KeyRangeServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
